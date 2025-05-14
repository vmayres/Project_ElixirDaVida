using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    [Header("Vida")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int lastHealth;

    [Header("Debug")]
    [SerializeField] private string currentRoomName;

    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 lastLookDirection = Vector2.down; // padr�o inicial

    [Header("Dash")]
    [SerializeField] public bool dashEnabled; // S� vira true se tiver com as botas
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private bool _canDash = true;
    public bool CanDash { get { return _canDash; } }

    [Header("Invulnerabilidade")]
    [SerializeField] private float invulnerabilityTimeAfterHit = 1.0f;
    [SerializeField] private bool isInvulnerable = false;

    // Lista das poss�veis po��es (precisa ser p�blica!)
    [Header("Prefabs das Po��es")]
    [SerializeField] public GameObject firePotionPrefab;
    [SerializeField] public GameObject icePotionPrefab;
    [SerializeField] public GameObject earthPotionPrefab;
    [SerializeField] public GameObject lightningPotionPrefab;

    public enum PotionType
    {
        Fire,
        Ice,
        Earth,
        Lightning,
        Null,
    }

    [SerializeField] private PotionType _activePotion = PotionType.Fire;        // Po��o ativa inicial
    public PotionType ActivePotion
    {
        get => _activePotion;
        set => _activePotion = value;
    }

    public GameObject GetPotionPrefab(PotionType type)
    {
        return type switch
        {
            PotionType.Fire => firePotionPrefab,
            PotionType.Ice => icePotionPrefab,
            PotionType.Earth => earthPotionPrefab,
            PotionType.Lightning => lightningPotionPrefab,
            _ => null
        };
    }

    // Current room
    private Collider2D _currentRoom;
    public Collider2D CurrentRoom => _currentRoom;

    //
    [SerializeField] private CircleRenderer circleRenderer;

    // 
    private HeartDisplay heartDisplay;
    void Start()
    {
        heartDisplay = FindAnyObjectByType<HeartDisplay>();

        maxHealth = InventoryControll.Instance.maxHealth;
        currentHealth = InventoryControll.Instance.currentHealth;
        lastHealth = currentHealth;
        heartDisplay.ResetHearts(maxHealth, currentHealth);
        heartDisplay.UpdateHearts(currentHealth, maxHealth);

        dashEnabled = InventoryControll.Instance.dashUnlocked;
    }

    //
    void Update()
    {

        UpdateCurrentRoomName();

        // === MOVIMENTO ===
        if (!isDashing)
        {
            //  retorna os inputs do eixo horizontal e vertical ( valores entre -1 e 1)
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Cria o vetor de movimento (vertor com as componentes x e y)
            Vector2 movementInput = new Vector2(horizontalInput, verticalInput);

            // Garante que a velocidade seja constante
            if (movementInput.magnitude > 1f)
            {
                movementInput = movementInput.normalized;
            }

            // Atualiza a dire��o "olhada" com base no input, mas normalizada para dire��o fixa (8 dire��es)
            if (movementInput != Vector2.zero)
            {
                lastLookDirection = GetMoveDirection(movementInput);
                // TODO: Atualizar anima��o do personagem
                //Debug.Log("Dire��o: " + lastLookDirection);
                
            }

            // Atualiza a posi��o com movimento proporcional e corrigido
            transform.position += new Vector3(movementInput.x * moveSpeed * Time.deltaTime, movementInput.y * moveSpeed * Time.deltaTime, 0);

            // === DASH ===
            if (dashEnabled && Input.GetButtonDown("Dash") && _canDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) { 

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


    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        lastHealth = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - damage);

        heartDisplay.UpdateHearts(currentHealth, lastHealth);
        InventoryControll.Instance.currentHealth = currentHealth;

        if(currentHealth == 0){
            GameProgress.Instance.deathCount += 1;
            GameProgress.Instance.heartsCurrent = GameProgress.Instance.heartsMax;
            GameProgress.Instance.Save();

            Time.timeScale = 1;
            Initiate.Fade("Death", Color.black, 0.8f);
        }

        isInvulnerable = true;
        // Inicia a corrotina para lidar com a invulnerabilidade
        StartCoroutine(HandleInvulnerability());
    }

    public void Heal(int healAmount)
    {
        lastHealth = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        heartDisplay.UpdateHearts(currentHealth, lastHealth);
        InventoryControll.Instance.currentHealth = currentHealth;
    }

    public void IncreaseMaxLife(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        heartDisplay.ResetHearts(maxHealth, currentHealth);
        InventoryControll.Instance.currentHealth = currentHealth;
        InventoryControll.Instance.maxHealth = maxHealth;
    }

    private IEnumerator HandleInvulnerability()
    {
        isInvulnerable = true;

        // Acessa o filho "Sprite" e muda a cor
        Transform spriteTransform = transform.Find("Sprite");
        if (spriteTransform != null)
        {
            SpriteRenderer sr = spriteTransform.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(0.8f, 0f, 0f, 1f); // vermelho com transpar�ncia
                yield return new WaitForSeconds(invulnerabilityTimeAfterHit);
                sr.color = Color.white; // volta ao normal
            }
        }

        isInvulnerable = false;
    }

    private Vector2 GetMoveDirection(Vector2 input)
    {
        return input.normalized; // mant�m dire��o completa, incluindo diagonais
    }


    private IEnumerator Dash()
    {
        isDashing = true;
        _canDash = false;

        float dashSpeed = dashDistance / dashDuration;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            transform.position += (Vector3)(lastLookDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    void UpdateCurrentRoomName()
    {
        Collider2D myRoom = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("roomArea"));
        currentRoomName = myRoom != null ? myRoom.gameObject.name : "Nenhuma";
    }
}
