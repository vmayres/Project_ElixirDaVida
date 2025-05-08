using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth = 3;

    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 lastLookDirection = Vector2.down; // padr�o inicial

    [Header("Dash")]
    [SerializeField] private bool dashEnabled = false; // S� vira true se tiver com as botas
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
    public enum PotionType
    {
        Fire,
        Ice,
        Earth,
        Lightning,
    }
    [Header("Po��es")]
    [SerializeField] private PotionType _activePotion = PotionType.Fire;        // Po��o ativa inicial
    private HashSet<PotionType> unlockedPotions = new HashSet<PotionType>();    // Po��es desbloqueadas
    public PotionType ActivePotion{ get => _activePotion; }

    // Current room
    private Collider2D _currentRoom;
    public Collider2D CurrentRoom => _currentRoom;

    //
    [SerializeField] private CircleRenderer circleRenderer;

    // 
    void Start()
    {
        circleRenderer.SetRadius(2.5f);

    }

    //
    void Update()
    {
        if (!isDashing)
        {
            // === MOVIMENTO ===
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

    private void OnCollisionEnter2D(Collision2D other) //// esse vai sofrer mudan�as (para verificar odano vai o zumbi no seu tempo de ataque
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // L�gica de dano ao jogador
            TakeDamage(1);
        }
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
        currentHealth -= damage;
        isInvulnerable = true;
        // Inicia a corrotina para lidar com a invulnerabilidade
        StartCoroutine(HandleInvulnerability());
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
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


}
