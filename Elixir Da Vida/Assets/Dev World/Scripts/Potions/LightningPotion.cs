using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPotion : PotionBase
{
    private float aoeDuration = 0.5f;

    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[LIGHTNING] Poção de raio lançada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // Inicia a propagação do raio
        var hitEnemies = new HashSet<Collider2D>();
        yield return ApplyLightningEffect(targetPosition, 3, hitEnemies); // 3 inimigos em cadeia
    }

    private IEnumerator ApplyLightningEffect(Vector3 position, int chainCount, HashSet<Collider2D> hitEnemies)
    {
        // Procura inimigos ao redor da posição
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range);
        Collider2D currentTarget = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && !hitEnemies.Contains(hit))
            {
                currentTarget = hit;
                hitEnemies.Add(hit);
                break;
            }
        }

        if (currentTarget == null)
        {
            Debug.Log("Nenhum inimigo válido encontrado.");
            yield break;
        }

        Vector3 targetPos = currentTarget.transform.position;

        // Cria o efeito visual (círculo de raio)
        CreateAoECircle(targetPos, range, Color.yellow);

        // Aplica dano
        var enemyManager = currentTarget.GetComponent<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.TakeDamage(this.damage);
        }
        else
        {
            Debug.LogWarning($"Objeto com tag 'Enemy' não possui o componente 'EnemyManager': {currentTarget.name}");
        }

        yield return new WaitForSeconds(0.2f);

        // Busca o próximo inimigo mais próximo ainda não atingido
        Collider2D[] allHits = Physics2D.OverlapCircleAll(targetPos, range);
        Collider2D nextTarget = null;
        float closestDist = float.MaxValue;

        foreach (var hit in allHits)
        {
            if (hit.CompareTag("Enemy") && !hitEnemies.Contains(hit))
            {
                float dist = Vector2.Distance(targetPos, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    nextTarget = hit;
                }
            }
        }

        if (chainCount > 1 && nextTarget != null)
        {
            yield return ApplyLightningEffect(nextTarget.transform.position, chainCount - 1, hitEnemies);
        }
    }

    private GameObject CreateAoECircle(Vector3 position, float radius, Color color)
    {
        GameObject aoeVisual = new GameObject("AoEVisual");
        aoeVisual.transform.position = position;

        LineRenderer lineRenderer = aoeVisual.AddComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.widthMultiplier = 0.04f;
        lineRenderer.positionCount = 64;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / lineRenderer.positionCount;
            lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }

        Destroy(aoeVisual, aoeDuration);
        return aoeVisual;
    }
}
