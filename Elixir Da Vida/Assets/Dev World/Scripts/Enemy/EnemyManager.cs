using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 3;
    [SerializeField] private bool frozen = false;

    [Header("Efeitos Visuais")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private readonly Color hitColor = new Color(1f, 0f, 0f, 1f);     // Vermelho
    private readonly Color frozenColor = new Color(0f, 1f, 1f, 1f);  // Ciano
    private float flashDuration = 0.15f;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;

        if (frozen)
            spriteRenderer.color = frozenColor;
    }

    void Update()
    {
        if (enemyHealth <= 0)
        {
            Debug.Log("Inimigo derrotado!");
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (frozen)
        {
            enemyHealth -= damage * 2;
            frozen = false;
            Debug.Log("Inimigo congelado recebeu dano dobrado: " + damage * 2);
        }
        else
        {
            enemyHealth -= damage;
            Debug.Log("Inimigo recebeu dano: " + damage);
        }

        StartCoroutine(FlashDamageEffect());
    }

    private IEnumerator FlashDamageEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(flashDuration);

            // Volta ao estado normal (ciano se ainda congelado)
            spriteRenderer.color = frozen ? frozenColor : originalColor;
        }
    }

    public void FreezeEnemy()
    {
        frozen = true;
        if (spriteRenderer != null)
            spriteRenderer.color = frozenColor;
    }
}
