using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPotion : PotionBase
{
    private float aoeDuration = 0.5f;

    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[LIGHTNING] Po��o de raio lan�ada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // Cria o efeito visual inicial no ponto da explos�o
        CreateAoECircle(targetPosition, effectRadius, Color.yellow);

        // Inicia a propaga��o do raio
        var hitEnemies = new HashSet<Collider2D>();
        yield return ApplyLightningEffect(targetPosition, 3, effectRadius, hitEnemies);
    }

    private IEnumerator ApplyLightningEffect(Vector3 position, int chainCount, float detectionRadius, HashSet<Collider2D> hitEnemies)
    {
        // Procura inimigos dentro do raio especificado
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, detectionRadius);
        Collider2D currentTarget = null;

        foreach (var hit in hits)
        {
            if ((hit.CompareTag("Enemy")||hit.CompareTag("Boss")) && !hitEnemies.Contains(hit))
            {
                currentTarget = hit;
                hitEnemies.Add(hit);
                break;
            }
        }

        if (currentTarget == null)
        {
            Debug.Log("Nenhum inimigo v�lido encontrado.");
            yield break;
        }

        Vector3 targetPos = currentTarget.transform.position;

        // Aplica dano
        var enemyManager = currentTarget.GetComponent<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.TakeDamage(this.damage);
        }
        else
        {
            Debug.LogWarning($"Objeto com tag 'Enemy' n�o possui o componente 'EnemyManager': {currentTarget.name}");
        }

        yield return new WaitForSeconds(0.2f);

        // Busca o pr�ximo inimigo mais pr�ximo
        Collider2D[] allHits = Physics2D.OverlapCircleAll(targetPos, range); // busca com range total
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

        // Encadeia o pr�ximo raio
        if (chainCount > 1 && nextTarget != null)
        {
            // Cria a linha entre os dois inimigos
            CreateLightningLine(targetPos, nextTarget.transform.position, Color.yellow);
            yield return ApplyLightningEffect(nextTarget.transform.position, chainCount - 1, range, hitEnemies);
        }
    }

    private void CreateLightningLine(Vector3 from, Vector3 to, Color color)
    {
        GameObject lineGO = new GameObject("LightningLine");
        LineRenderer line = lineGO.AddComponent<LineRenderer>();

        line.positionCount = 2;
        line.SetPosition(0, from);
        line.SetPosition(1, to);

        line.widthMultiplier = 0.06f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = color;
        line.endColor = color;

        // Opcional: pequeno "ziguezague" pode ser adicionado para parecer mais el�trico

        GameObject.Destroy(lineGO, aoeDuration);
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
