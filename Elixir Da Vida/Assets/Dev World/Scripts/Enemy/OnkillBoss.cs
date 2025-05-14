using System.Collections.Generic;
using UnityEngine;

public class OnkillBoss : MonoBehaviour
{
    [Header("Portas Associadas")]
    [SerializeField] private List<GameObject> linkedDoors;

    [Header("Objeto a ser ativado ao morrer (opcional)")]
    [SerializeField] private GameObject objectToActivate;

    [Header("Estado da ação (true = abrir portas, false = fechar)")]
    [SerializeField] private bool isPressed = true;

    private bool alreadyTriggered = false;

    private void OnDestroy()
    {
        if (alreadyTriggered) return;
        alreadyTriggered = true;

        // Ativa o objeto, se definido
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Para cada porta associada, abre ou fecha conforme o estado
        foreach (var doorObj in linkedDoors)
        {
            if (doorObj == null) continue;
            room roomComponent = doorObj.GetComponentInParent<room>();
            if (roomComponent != null)
            {
                if (isPressed)
                    roomComponent.OpenDoor(doorObj);
                else
                    roomComponent.CloseDoor(doorObj);
            }
        }
    }
}
