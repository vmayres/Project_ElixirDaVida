using System.Collections;
using UnityEngine;

/*
Essa � a classe abstrata que define a estrutura b�sica de uma po��o.
Ela cont�m propriedades comuns a todas as po��es, como nome, dano e tempo de recarga.
 */
public abstract class PotionBase : MonoBehaviour
{
    public string potionName;   // Nome da po��o
    public float damage;        // Dano causado pela po��o
    public float range;         // Alcance da po��o
    public float effectRadius;  // Raio do efeito da po��o
    public float cooldown;      // Tempo de recarga da po��o

    // Metodos da super classe
    public virtual IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        // Instancia o pr�prio prefab no ponto de spawn
        GameObject potionInstance = Instantiate(gameObject, spawnPosition, Quaternion.identity);

        // Move a po��o em linha reta at� o ponto de destino
        while (potionInstance != null && Vector3.Distance(potionInstance.transform.position, targetPosition) > 0.1f)
        {
            // Calcula a dire��o do movimento
            Vector3 direction = (targetPosition - potionInstance.transform.position).normalized;

            // Move a po��o na dire��o do alvo
            potionInstance.transform.position += direction * Time.deltaTime * 5f; // Ajuste a velocidade conforme necess�rio

            yield return null; // Espera at� o pr�ximo frame
        }

        // Destroi a po��o ao chegar no destino
        if (potionInstance != null)
        {
            Destroy(potionInstance);
        }
    }
}