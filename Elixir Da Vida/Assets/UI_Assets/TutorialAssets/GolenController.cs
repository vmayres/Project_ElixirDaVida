using UnityEngine;

public class GolenController : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueController.MostrarFalaDoGolem();
        }
    }

}
