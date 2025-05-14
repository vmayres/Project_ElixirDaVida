using UnityEngine;
using UnityEngine.Tilemaps;

public class ToggleGameObject : MonoBehaviour
{
    //Colecar esse script no objeto a ser ativado/desativado
    public string idDoItem;

    void Start()
    {
        if (InventoryControll.Instance == null)
        {
            Debug.LogWarning("InventoryControll.Instance está nulo!");
            return;
        }

        var item = InventoryControll.Instance.itens.Find(i => i.id == idDoItem);
        if (item != null && item.desbloqueada)
        {
            gameObject.SetActive(false);
        }
    }

}
