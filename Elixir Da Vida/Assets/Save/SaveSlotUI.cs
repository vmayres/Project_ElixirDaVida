using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;


public class SaveSlotUI : MonoBehaviour
{
    public int slotIndex;
    public TextMeshProUGUI saveInfoText;
    public GameObject delButton;
    public GameObject confirmDeletePanel;
    public Color loadToColor = Color.black;

    // [Header("Scripts")]
    // public InventoryDisplay inventoryDisplay;
    // public InventoryControll inventoryController;
    // public DashBar dashBar;


    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
            DisplaySaveInfo();

        if(SceneManager.GetActiveScene().name == "TestePause")
            OnClickSave();
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

            foreach (var item in GameProgress.Instance.pocoes)
            {
                item.desbloqueada = false;
            }

            foreach (var item in GameProgress.Instance.itens)
            {
                item.desbloqueada = false;
            }

            foreach (var item in GameProgress.Instance.equipamentos)
            {
                item.desbloqueada = false;
            }
            
            GameProgress.Instance.playerPosition = new Vector3(0, 0, 0);
            GameProgress.Instance.heartsMax = 3;
            GameProgress.Instance.heartsCurrent = 3;
            GameProgress.Instance.dashes = false;
            GameProgress.Instance.deathCount = 0;
            GameProgress.Instance.currentScene = "CutScene"; // ou fase inicial
            Initiate.Fade("CutScene", loadToColor, 0.5f);
        }

        InventoryControll.Instance.CarregarDoProgressoSalvo();
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

        GameProgress.Instance.pocoes = InventoryControll.Instance.pocoes.Select(p => new InventoryEntry(p)).ToList();
        GameProgress.Instance.itens = InventoryControll.Instance.itens.Select(i => new InventoryEntry(i)).ToList();
        GameProgress.Instance.equipamentos = InventoryControll.Instance.equipamentos.Select(e => new InventoryEntry(e)).ToList();


        GameProgress.Instance.Save();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // InventoryControll.Instance.currentHealth = InventoryControll.Instance.maxHealth;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerControl>().Heal(InventoryControll.Instance.maxHealth);
            OnClickSave();
        }
    }
}
