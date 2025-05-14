using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float nodeReachThreshold = 0.1f;
    private Transform player;
    private List<Vector3> path = new List<Vector3>();
    private int currentPathIndex = 0;

    [Header("Grid Settings")]
    public float gridSize = 1f;
    public int maxSearchDistance = 50;

    [Header("Pathfinding")]
    [SerializeField] private float pathUpdateInterval = 1.5f;
    private float pathTimer = 0f;

    [Header("Ataque")]
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private float timeToDamage = 1.5f;
    [SerializeField] private int damage = 1;

    private GameObject attackZoneCircle;
    private LineRenderer attackRenderer;
    private Coroutine attackRoutine = null;

    [Header("Debug")]
    [SerializeField] private string currentRoomName;

    private Collider2D _currentRoom;
    public Collider2D CurrentRoom => _currentRoom;
    public string CurrentRoomAreaName => _currentRoom != null ? _currentRoom.gameObject.name : "Nenhuma";

    private EnemyManager enemyManager;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        enemyManager = GetComponent<EnemyManager>();

        CreateAttackCircle();
    }

    void Update()
    {
        currentRoomName = CurrentRoomAreaName;

        if (player == null || (enemyManager != null && enemyManager.frozen)) return;

        if (IsInSameRoom())
        {
            pathTimer += Time.deltaTime;

            if (path.Count == 0 || pathTimer >= pathUpdateInterval)
            {
                path = FindPath(transform.position, player.position);
                currentPathIndex = 0;
                pathTimer = 0f;
            }

            FollowPath();
        }
        else
        {
            path.Clear();
        }

        CheckAttackZone();
    }

    bool IsInSameRoom()
    {
        PlayerControl playerControl = player != null ? player.GetComponent<PlayerControl>() : null;
        if (playerControl == null) return false;
        return this.CurrentRoom != null && playerControl.CurrentRoom != null && this.CurrentRoom == playerControl.CurrentRoom;
    }

    void FollowPath()
    {
        if (path.Count == 0 || currentPathIndex >= path.Count)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 1f)
            return;

        Vector3 target = path[currentPathIndex];
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) < nodeReachThreshold)
            currentPathIndex++;
    }

    void CheckAttackZone()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= attackRadius)
        {
            if (attackRoutine == null)
            {
                attackRoutine = StartCoroutine(AttackCycle());
            }
        }
        else
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
                ResetAttackVisual();
            }
        }
    }

    IEnumerator AttackCycle()
    {
        float timer = 0f;

        while (timer < timeToDamage)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            float transparency = 1f - Mathf.Clamp01(distToPlayer / attackRadius);
            transparency = Mathf.Clamp(transparency, 0f, 1f);

            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / timeToDamage);

            Color color = Color.Lerp(Color.white, Color.red, progress);
            color.a = transparency;

            attackRenderer.startColor = color;
            attackRenderer.endColor = color;

            yield return null;
        }

        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        if (playerControl != null)
        {
            playerControl.TakeDamage(damage);
        }

        ResetAttackVisual();
        attackRoutine = null;
    }

    void CreateAttackCircle()
    {
        attackZoneCircle = new GameObject("AttackZone");
        attackZoneCircle.transform.SetParent(transform);
        attackZoneCircle.transform.localPosition = Vector3.zero;

        attackRenderer = attackZoneCircle.AddComponent<LineRenderer>();
        attackRenderer.loop = true;
        attackRenderer.useWorldSpace = false;
        attackRenderer.widthMultiplier = 0.025f;
        attackRenderer.positionCount = 64;
        attackRenderer.material = new Material(Shader.Find("Sprites/Default"));
        attackRenderer.startColor = new Color(1f, 1f, 1f, 0f);
        attackRenderer.endColor = new Color(1f, 1f, 1f, 0f);

        for (int i = 0; i < attackRenderer.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / attackRenderer.positionCount;
            attackRenderer.SetPosition(i, new Vector3(Mathf.Cos(angle) * attackRadius, Mathf.Sin(angle) * attackRadius, 0f));
        }
    }

    void ResetAttackVisual()
    {
        Color resetColor = new Color(1f, 1f, 1f, 0f);
        attackRenderer.startColor = resetColor;
        attackRenderer.endColor = resetColor;
    }

    List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector2Int start = WorldToGrid(startPos);
        Vector2Int goal = WorldToGrid(targetPos);

        if (start == goal)
            return new List<Vector3> { targetPos };

        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        openSet.Enqueue(start, 0);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> gScore = new Dictionary<Vector2Int, int>
        {
            [start] = 0
        };

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                int tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    int fScore = tentativeGScore + Heuristic(neighbor, goal);
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return new List<Vector3>();
    }

    List<Vector3> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector3> totalPath = new List<Vector3> { GridToWorld(current) };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, GridToWorld(current));
        }
        return totalPath;
    }

    int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x / gridSize), Mathf.RoundToInt(worldPos.y / gridSize));
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * gridSize, gridPos.y * gridSize, 0f);
    }

    List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        return new List<Vector2Int>
        {
            node + Vector2Int.up,
            node + Vector2Int.down,
            node + Vector2Int.left,
            node + Vector2Int.right
        };
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RoomArea"))
        {
            _currentRoom = other;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("RoomArea") && other == _currentRoom)
        {
            _currentRoom = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == _currentRoom)
        {
            _currentRoom = null;
        }
    }

    private class PriorityQueue<T>
    {
        private List<KeyValuePair<T, int>> elements = new List<KeyValuePair<T, int>>();
        public int Count => elements.Count;

        public void Enqueue(T item, int priority)
        {
            elements.Add(new KeyValuePair<T, int>(item, priority));
        }

        public T Dequeue()
        {
            if (elements.Count == 0)
                throw new System.InvalidOperationException("Priority queue is empty");

            int bestIndex = 0;
            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].Value < elements[bestIndex].Value)
                    bestIndex = i;
            }

            T bestItem = elements[bestIndex].Key;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }
}