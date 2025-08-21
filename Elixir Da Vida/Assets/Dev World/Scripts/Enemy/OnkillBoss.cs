using System.Collections.Generic;
using UnityEngine;

public class OnkillBoss : MonoBehaviour
{
    [Header("Portas Associadas")]
    [SerializeField] private List<GameObject> linkedDoors;

    [Header("Objeto a ser ativado ao morrer (opcional)")]
    [SerializeField] private GameObject objectToActivate;

    [Header("Estado da a��o (true = abrir portas, false = fechar)")]
    [SerializeField] private bool isPressed = true;

    private bool alreadyTriggered = false;

    public void onKill()
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

        if (TryGetComponent<EnemyManager>(out var enemyManager))
            enemyManager.enabled = false;

        if (TryGetComponent<EnemyAI>(out var enemyAI))
            enemyAI.enabled = false;

        // foreach (var col in GetComponents<Collider2D>())
        //     col.enabled = false;

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

    }
}
