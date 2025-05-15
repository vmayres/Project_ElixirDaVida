using UnityEngine;

public class PortaPorPocao : MonoBehaviour
{
    public string poçãoNecessária; // ID da poção que abre a porta
    public Sprite spriteFechado;
    public Sprite spriteAberto;

    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        PotionSelect.OnPotionChanged += VerificarPoção;
    }

    void OnDisable()
    {
        PotionSelect.OnPotionChanged -= VerificarPoção;
    }

    void VerificarPoção(string poçãoAtual)
    {
        bool aberta = (poçãoAtual == poçãoNecessária);

        // Atualiza sprite e colisor
        sr.sprite = aberta ? spriteAberto : spriteFechado;
        col.enabled = !aberta;
    }
}
