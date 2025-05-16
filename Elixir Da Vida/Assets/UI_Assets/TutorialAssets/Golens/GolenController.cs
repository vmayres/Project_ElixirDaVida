using UnityEngine;

public class GolenController : MonoBehaviour
{
    public GameObject TEXT;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TEXT.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TEXT.SetActive(false);
        }
    }
}
