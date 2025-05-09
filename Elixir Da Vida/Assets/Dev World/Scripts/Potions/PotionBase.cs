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

    // Metodos da super classe
    public virtual IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        GameObject potionInstance = Instantiate(gameObject, spawnPosition, Quaternion.identity);

        float distance = Vector3.Distance(spawnPosition, targetPosition);
        float duration = distance / 12f;
        float elapsedTime = 0f;

        float h0 = 1.55f;
        float a = 0.505f;
        float maxRange = range;

        float sixtyPercent = 0.6f * maxRange;
        float maxHeight = h0 + maxRange / 2f - a * Mathf.Pow(maxRange / 2f, 2);

        bool isShortRange = distance <= sixtyPercent;

        while (elapsedTime < duration && potionInstance != null)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Movimento linear (XY ou XZ, conforme o seu jogo usa)
            Vector3 newPosition = Vector3.Lerp(spawnPosition, targetPosition, t);
            potionInstance.transform.position = newPosition;

            // Escala visual
            float scaleFactor;

            if (isShortRange)
            {
                // Lançamento curto: escala de 1.0 até 0.5 linearmente
                scaleFactor = Mathf.Lerp(1f, 0.5f, t);
            }
            else
            {
                // Lançamento longo com arco visual
                if (t <= 0.5f)
                {
                    // Primeira metade: cresce de 1.0 até 1.2
                    scaleFactor = Mathf.Lerp(1f, 1.2f, t / 0.5f);
                }
                else
                {
                    // Segunda metade: diminui de 1.2 até 0.5
                    scaleFactor = Mathf.Lerp(1.2f, 0.5f, (t - 0.5f) / 0.5f);
                }
            }

            potionInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

            yield return null;
        }

        if (potionInstance != null)
        {
            Destroy(potionInstance);
        }
    }



}