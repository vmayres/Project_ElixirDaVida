using UnityEngine;
using UnityEngine.UI;

public class HeartControll : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public Sprite fullHeartSprite;     // Sprite de coração cheio
    public Sprite emptyHeartSprite;    // Sprite de coração vazio
    public Sprite intermediateHeartSprite; // Sprite de coração intermediário

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Chama quando o jogador perde vida
    public void Lose()
    {
        animator.SetTrigger("Lose");  // Aciona a animação de perder vida
        spriteRenderer.sprite = emptyHeartSprite;  // Garantir que o sprite final é o vazio
    }

    // Chama quando o jogador ganha vida
    public void Gain()
    {
        animator.SetTrigger("Gain"); // Aciona a animação de ganhar vida
        spriteRenderer.sprite = fullHeartSprite; // Garantir que o sprite final é o cheio
    }

    public void SetEmpty()
    {
        animator.ResetTrigger("Gain");
        animator.ResetTrigger("Lose");
        spriteRenderer.sprite = emptyHeartSprite;
    }

    // public void Pop()
    // {
    //     animator.SetTrigger("Pop");
    // }
}
