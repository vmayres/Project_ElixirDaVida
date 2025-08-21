using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPotion : PotionBase
{
    [SerializeField] private GameObject bombPrefab; // Prefab da bomba a ser instanciada
    [SerializeField] private Material lineMaterial; // Material para o círculo visual
    private float aoeDuration = 3.0f;

    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[EARTH] Po��o de terremoto lan�ada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);
        

        // Instancia o prefab da bomba no local do impacto
        GameObject bomb = Instantiate(bombPrefab, targetPosition, Quaternion.identity);

        // === VISUAL: cria c�rculo de efeito ao redor usando LineRenderer ===
        GameObject aoeVisual = new GameObject("AoEVisual");
        aoeVisual.transform.position = targetPosition;

        LineRenderer lineRenderer = aoeVisual.AddComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.widthMultiplier = 0.02f;
        lineRenderer.positionCount = 64; // N�mero de segmentos do c�rculo
        lineRenderer.material = lineMaterial; //new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        float radius = effectRadius;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / lineRenderer.positionCount;
            lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }

        // === L�GICA: transi��o de cor do c�rculo ===
        float elapsed = 0f;
        while (elapsed < aoeDuration)
        {
            float t = elapsed / aoeDuration; // Progresso de 0 a 1
            Color currentColor = Color.Lerp(Color.white, Color.red, t); // Transi��o de branco para vermelho
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // === L�GICA: detectar inimigos na �rea (2D) ===
        HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, effectRadius);
        foreach (var hit in hits)
        {
            if ((hit.CompareTag("Enemy")||hit.CompareTag("Boss")) && !hitEnemies.Contains(hit))
            {
                hitEnemies.Add(hit);
                Debug.Log($"Inimigo atingido: {hit.name}");

                var enemyManager = hit.GetComponent<EnemyManager>();
                if (enemyManager != null)
                {
                    enemyManager.TakeDamage(this.damage);
                    // Destroy(gameObject); // Destroi a bomba ap�s atingir
                    break;
                }
            }
            else if (hit.CompareTag("Player"))
            {
                Debug.Log($"Player atingido: {hit.name}");

                var playerControl = hit.GetComponent<PlayerControl>();
                if (playerControl != null)
                {
                    // Destroy(gameObject); // Destroi a bomba ap�s atingir
                    playerControl.TakeDamage(this.damage);
                    break;
                }
            }
            else if (hit.CompareTag("rock"))
            {
                Debug.Log($"Pedra atingida: {hit.name}");

                var rockManager = hit.GetComponent<RockManager>();
                if (rockManager != null)
                {
                    rockManager.DestroyRockAndActivateObject();
                    // Destroy(gameObject); // Destroi a bomba ap�s atingir
                    break;
                }
            }
        }



        // Remove o c�rculo visual e a bomba
        Destroy(aoeVisual);
        Destroy(bomb);

        Debug.Log("[EARTH] Explos�o conclu�da e dano aplicado.");
    }
}
