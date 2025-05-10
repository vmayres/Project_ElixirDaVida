using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    public int slotIndex;
    public TextMeshProUGUI saveInfoText;
    public GameObject delButton;
    public GameObject confirmDeletePanel;
    public Color loadToColor = Color.black;

    [Header("Scripts")]
    public TesteVida testeVida;
    public InventoryDisplay inventoryDisplay;
    // public InventoryControll inventoryController;
    // public DashBar dashBar;


    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
            DisplaySaveInfo();
    }

    public void DisplaySaveInfo()
    {
        SaveData data = SaveSystem.LoadGame(slotIndex);

        if (data != null)
        {
            string time = System.TimeSpan.FromSeconds(data.playTime).ToString(@"hh\:mm\:ss");
            saveInfoText.text = $"Fase: {data.currentScene}\nTempo: {time}\nCorações: {data.heartsCurrent}";
            delButton.SetActive(true);
        }
        else
        {
            saveInfoText.text = "New Game";
            delButton.SetActive(false);
        }
    }

    public void OnClickSlot()
    {
        SaveData data = SaveSystem.LoadGame(slotIndex);

        if (data != null)
        {
            GameProgress.Instance.Load(slotIndex);
            Initiate.Fade(data.currentScene, loadToColor, 0.5f);
        }
        else
        {
            GameProgress.Instance.currentSlot = slotIndex;
            GameProgress.Instance.playTime = 0f;
            // GameProgress.Instance.unlockedItems.Clear();
            GameProgress.Instance.pocoes.Clear();
            GameProgress.Instance.itens.Clear();
            GameProgress.Instance.equipamentos.Clear();
            GameProgress.Instance.heartsMax = 3;
            GameProgress.Instance.heartsCurrent = 3;
            GameProgress.Instance.dashes = false;
            GameProgress.Instance.currentScene = "CutScene"; // ou fase inicial
            Initiate.Fade("CutScene", loadToColor, 0.5f);
        }
    }

    public void OnClickDeleteSlot()
    {
        confirmDeletePanel.SetActive(true); // exibe o painel de confirmação
    }

    public void ConfirmDeleteYes()
    {
        SaveSystem.Delete(slotIndex);
        DisplaySaveInfo();
        confirmDeletePanel.SetActive(false);
    }

    public void ConfirmDeleteNo()
    {
        confirmDeletePanel.SetActive(false);
    }

    public void OnClickSave()
    {
        GameProgress.Instance.currentScene = SceneManager.GetActiveScene().name;
        GameProgress.Instance.heartsMax = InventoryControll.Instance.maxHealth;
        GameProgress.Instance.heartsCurrent = InventoryControll.Instance.currentHealth;
        GameProgress.Instance.dashes = InventoryControll.Instance.dashUnlocked;
        
        GameProgress.Instance.pocoes = InventoryControll.Instance.pocoes;
        GameProgress.Instance.itens = InventoryControll.Instance.itens;
        GameProgress.Instance.equipamentos = InventoryControll.Instance.equipamentos;

        Vector2 posicao = new Vector2(0f, 0f);
        GameProgress.Instance.playerPosition = posicao;

        GameProgress.Instance.Save();
    }
}
