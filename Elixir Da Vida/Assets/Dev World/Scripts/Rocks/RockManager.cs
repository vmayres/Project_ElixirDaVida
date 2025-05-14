using UnityEngine;

public class RockManager : MonoBehaviour
{
    [Header("Objeto para ativar após destruir a pedra")]
    [SerializeField] private GameObject objectToActivate;

    public void DestroyRockAndActivateObject()
    {
        // Ativa o objeto associado, se existir
        if (objectToActivate != null)
        {
            objectToActivate.transform.position = transform.position;
            objectToActivate.SetActive(true);
        }

        // Destroi a pedra atual
        Destroy(gameObject);
    }
}
