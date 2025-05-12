using UnityEngine;
using TMPro;

public class DescriptionController : MonoBehaviour
{
    public static DescriptionController instance;

    public GameObject panel;
    public TextMeshProUGUI texto;

    void Awake()
    {
        instance = this;
        Esconder();
    }

    public void Mostrar(string descricao, Vector2 posicao)
    {
        texto.text = descricao;
        // panel.GetComponent<RectTransform>().anchoredPosition = posicao;
        panel.GetComponent<RectTransform>().anchoredPosition = posicao + new Vector2(0.5f, -1);

        panel.SetActive(true);
    }

    public void Esconder()
    {
        panel.SetActive(false);
    }
}
