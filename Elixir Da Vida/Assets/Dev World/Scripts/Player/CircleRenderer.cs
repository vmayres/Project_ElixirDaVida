using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    [SerializeField] private int segments = 64;
    [SerializeField] private float radius = 1f;

    // Prefabs das poções com seus valores
    [Header("Prefabs das Poções")]
    [SerializeField] private GameObject firePotionPrefab;
    [SerializeField] private GameObject icePotionPrefab;
    [SerializeField] private GameObject earthPotionPrefab;
    [SerializeField] private GameObject lightningPotionPrefab;

    private PlayerControl player;
    private LineRenderer lineRenderer;
    private PlayerControl.PotionType lastPotion;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;

        lineRenderer.widthMultiplier = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        // Sobe até o PlayerControl
        player = GetComponentInParent<PlayerControl>();
    }

    void Update()
    {
        if (player == null) return;

        var currentPotion = player.ActivePotion;

        if (currentPotion != lastPotion)
        {
            lastPotion = currentPotion;

            PotionBase potionBase = GetPotionBaseFromPrefab(currentPotion);
            if (potionBase != null)
            {
                SetRadius(potionBase.range);
            }
        }
    }

    private PotionBase GetPotionBaseFromPrefab(PlayerControl.PotionType type)
    {
        GameObject prefab = type switch
        {
            PlayerControl.PotionType.Fire => firePotionPrefab,
            PlayerControl.PotionType.Ice => icePotionPrefab,
            PlayerControl.PotionType.Earth => earthPotionPrefab,
            PlayerControl.PotionType.Lightning => lightningPotionPrefab,
            _ => null
        };

        return prefab != null ? prefab.GetComponent<PotionBase>() : null;
    }

    public void SetRadius(float newRadius)
    {
        if (Mathf.Approximately(radius, newRadius)) return;

        radius = newRadius;
        DrawCircle();
    }

    private void DrawCircle()
    {
        Vector3[] positions = new Vector3[segments + 1];
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            positions[i] = new Vector3(x, y, 0f);
            angle += 2 * Mathf.PI / segments;
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    public float GetRadius()
    {
        return radius;
    }
}
