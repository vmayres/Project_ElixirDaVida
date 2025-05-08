using System.Collections;
using UnityEngine;

/*
Essa é a classe abstrata que define a estrutura básica de uma poção.
Ela contém propriedades comuns a todas as poções, como nome, dano e tempo de recarga.
 */
public abstract class PotionBase : MonoBehaviour
{
    public string potionName;   // Nome da poção
    public float damage;        // Dano causado pela poção
    public float range;         // Alcance da poção
    public float effectRadius;  // Raio do efeito da poção
    public float cooldown;      // Tempo de recarga da poção

    // Metodos da super classe
    public virtual IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        // Instancia o próprio prefab no ponto de spawn
        GameObject potionInstance = Instantiate(gameObject, spawnPosition, Quaternion.identity);

        // Move a poção em linha reta até o ponto de destino
        while (potionInstance != null && Vector3.Distance(potionInstance.transform.position, targetPosition) > 0.1f)
        {
            // Calcula a direção do movimento
            Vector3 direction = (targetPosition - potionInstance.transform.position).normalized;

            // Move a poção na direção do alvo
            potionInstance.transform.position += direction * Time.deltaTime * 5f; // Ajuste a velocidade conforme necessário

            yield return null; // Espera até o próximo frame
        }

        // Destroi a poção ao chegar no destino
        if (potionInstance != null)
        {
            Destroy(potionInstance);
        }
    }
}