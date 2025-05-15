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
    public int deathCount;

    // Inventário
    public List<InventoryEntry> pocoes;
    public List<InventoryEntry> itens;
    public List<InventoryEntry> equipamentos;

    public HashSet<int> falasJaLidasGolemFora;
    public HashSet<int> falasJaLidasGolemLab;
}