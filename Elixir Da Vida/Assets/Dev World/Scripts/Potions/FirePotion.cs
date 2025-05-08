using System.Collections;
using UnityEngine;

public class FirePotion : PotionBase
{
    [SerializeField] private Sprite aoeSprite; // Arraste um sprite circular vermelho no inspector
    [SerializeField] private float aoeDuration = 1.0f;

    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[FIRE] Poção de fogo lançada em: " + targetPosition);
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        Debug.Log("[FIRE] Efeito de queimadura iniciado.");

        // === VISUAL: cria sprite de efeito ao redor ===
        GameObject aoeVisual = new GameObject("FireAoEVisual");
        aoeVisual.transform.position = targetPosition;

        SpriteRenderer renderer = aoeVisual.AddComponent<SpriteRenderer>();
        renderer.sprite = aoeSprite;
        renderer.sortingOrder = 10;
        renderer.color = new Color(1f, 0f, 0f, 0.5f); // vermelho translúcido

        float scale = effectRadius * 2f;
        aoeVisual.transform.localScale = new Vector3(scale, scale, 1f);

        // === LÓGICA: detectar inimigos na área (2D) ===
        float elapsed = 0f;

        while (elapsed < aoeDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, effectRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Debug.Log($" Inimigo atingido: {hit.name}");
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(aoeVisual);

        Debug.Log("[FIRE] Área de efeito finalizada.");
    }
}
