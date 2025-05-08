using System.Collections;
using UnityEngine;

public class EarthPotion : PotionBase
{
    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[EARTH] Poção de terremoto lançada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // Lógica após o lançamento
        Debug.Log("[EARTH] Efeito de Explosao aplicado após impacto.");
    }
}