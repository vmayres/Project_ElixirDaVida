using UnityEngine;

public class UnlockItemEffect : CollectibleEffect
{
    public string id;
    public CategoriaItem categoria;
    public InventoryDisplay inventoryDisplay;
    
    public override void Apply()
    {
        inventoryDisplay.DesbloquearItem(id, categoria);
    }
}
