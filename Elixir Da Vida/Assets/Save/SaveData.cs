using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string currentScene;
    public float playTime;
    public float[] playerPosition; // x, y, z
    // public List<string> unlockedItems = new List<string>();
    public int heartsMax;
    public int heartsCurrent;
    public bool dashes;
    public int saveSlot;

    // Inventário
    public List<InventoryEntry> pocoes = new List<InventoryEntry>();
    public List<InventoryEntry> itens = new List<InventoryEntry>();
    public List<InventoryEntry> equipamentos = new List<InventoryEntry>();
}