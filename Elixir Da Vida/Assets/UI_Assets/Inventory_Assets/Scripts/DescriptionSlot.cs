using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string descricao;

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform meuRect = GetComponent<RectTransform>();
        DescriptionController.instance.Mostrar(descricao, meuRect.anchoredPosition);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionController.instance.Esconder();
    }
}
