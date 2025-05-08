using System.Collections;
using UnityEngine;

public class IcePotion : PotionBase
{
    public override IEnumerator LaunchPotion(Vector3 targetPosition, Vector3 spawnPosition)
    {
        Debug.Log("[ICE] Poção de gelo lançada em: " + targetPosition);

        // Chama o comportamento base
        yield return base.LaunchPotion(targetPosition, spawnPosition);

        // Lógica após o lançamento
        Debug.Log("[ICE] Efeito de Congelamento aplicado após impacto.");
    }
}