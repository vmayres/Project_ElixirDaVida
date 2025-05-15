using UnityEngine;

public class GolenController : MonoBehaviour
{
    public GameObject pocao;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Deu trigger e encontrou player");
            // dialogueController.MostrarFalaDoGolem();
            pocao.SetActive(true);
        }
    }

}
