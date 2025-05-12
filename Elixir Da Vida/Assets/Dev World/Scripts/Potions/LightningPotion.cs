using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPotion : PotionBase
{
    private float aoeDuration = 1.0f;

    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[LIGHTNING] Po��o de terremoto lan�ada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // L�gica ap�s o lan�amento
        Debug.Log("[LIGHTNING] Efeito de Explosao aplicado ap�s impacto.");

        // === VISUAL: cria c�rculo de efeito ao redor usando LineRenderer ===
        GameObject aoeVisual = new GameObject("AoEVisual");
        aoeVisual.transform.position = targetPosition;

        LineRenderer lineRenderer = aoeVisual.AddComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.widthMultiplier = 0.02f;
        lineRenderer.positionCount = 64; // N�mero de segmentos do c�rculo
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;

        float radius = effectRadius;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / lineRenderer.positionCount;
            lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }

        // === L�GICA: detectar inimigos na �rea (2D) ===
        HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();
        float elapsed = 0f;

        while (elapsed < aoeDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, effectRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy") && !hitEnemies.Contains(hit))
                {
                    hitEnemies.Add(hit);
                    Debug.Log($"Inimigo atingido: {hit.name}");
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(aoeVisual);

    }
}
