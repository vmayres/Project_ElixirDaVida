using UnityEngine;

public class GolenController : MonoBehaviour
{
    public GameObject TEXT; // arraste aqui o objeto de texto no inspector
    // public Vector3 offset = new Vector3(0, -10f, 0); // ajuste para posicionar abaixo

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // // Calcula a posição abaixo do Golem
            // Vector3 worldPos = transform.position;
            // Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos + offset);

            // Aplica a posição na UI
            // TEXT.GetComponent<RectTransform>().position = transform.position + offset;

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
