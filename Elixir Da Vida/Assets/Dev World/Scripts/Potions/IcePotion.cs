using System.Collections;
using UnityEngine;

public class IcePotion : PotionBase
{
    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[ICE] Po��o de gelo lan�ada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // L�gica ap�s o lan�amento
        Debug.Log("[ICE] Efeito de Congelamento aplicado ap�s impacto.");
    }
}