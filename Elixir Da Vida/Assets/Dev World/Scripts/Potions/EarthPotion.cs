using System.Collections;
using UnityEngine;

public class EarthPotion : PotionBase
{
    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[EARTH] Po��o de terremoto lan�ada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // L�gica ap�s o lan�amento
        Debug.Log("[EARTH] Efeito de Explosao aplicado ap�s impacto.");
    }
}