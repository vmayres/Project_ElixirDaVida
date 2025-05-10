using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    public List<InventoryEntry> pocoes;
    public List<InventoryEntry> itens;
    public List<InventoryEntry> equipamentos;

    public string currentScene;
    public float playTime;
    public Vector3 playerPosition;
    // public List<string> unlockedItems = new List<string>();
    public int heartsMax = 3;
    public int heartsCurrent = 3;
    public bool dashes = false;
    public int currentSlot = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        playTime += Time.deltaTime;
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.currentScene = currentScene;
        data.playTime = playTime;
        data.playerPosition = new float[] { playerPosition.x, playerPosition.y, playerPosition.z };
        // data.unlockedItems = unlockedItems;
        data.heartsMax = heartsMax;
        data.heartsCurrent = heartsCurrent;
        data.dashes = dashes;
        data.saveSlot = currentSlot;

        data.pocoes = pocoes;
        data.itens = itens;
        data.equipamentos = equipamentos;

        SaveSystem.SaveGame(data);
    }

    public void Load(int slot)
    {
        SaveData data = SaveSystem.LoadGame(slot);

        if (data != null)
        {
            currentScene = data.currentScene;
            playTime = data.playTime;
            playerPosition = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
            // unlockedItems = data.unlockedItems;
            heartsMax = data.heartsMax;
            heartsCurrent = data.heartsCurrent;
            dashes = data.dashes;
            currentSlot = slot;

            pocoes = data.pocoes ?? new List<InventoryEntry>();
            itens = data.itens ?? new List<InventoryEntry>();
            equipamentos = data.equipamentos ?? new List<InventoryEntry>();

        }
    }

    public InventoryEntry GetItem(string id, CategoriaItem categoria) {
        switch (categoria) {
            case CategoriaItem.Pocao:
                return pocoes.FirstOrDefault(p => p.id == id);
            case CategoriaItem.Item:
                return itens.FirstOrDefault(i => i.id == id);
            case CategoriaItem.Equipamento:
                return equipamentos.FirstOrDefault(e => e.id == id);
            default:
                return null;
        }
    }

    public bool IsUnlocked(string id, CategoriaItem categoria) {
        var item = GetItem(id, categoria);
        return item != null && item.desbloqueada;
    }


}
