using System.Collections;
using UnityEngine;

public class LightningPotion : PotionBase
{
    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[LIGHTNING] Poção de terremoto lançada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // Lógica após o lançamento
        Debug.Log("[LIGHTNING] Efeito de Explosao aplicado após impacto.");
        
    }
}
