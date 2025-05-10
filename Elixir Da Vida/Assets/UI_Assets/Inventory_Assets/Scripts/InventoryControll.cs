using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;


public class InventoryControll : MonoBehaviour
{
    public static InventoryControll Instance { get; private set; }

    [Header("Save")]
    public List<InventoryEntry> pocoes;
    public List<InventoryEntry> itens;
    public List<InventoryEntry> equipamentos;

    public int maxHealth;
    public int currentHealth;
    public bool dashUnlocked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para manter o objeto entre cenas
        }
        else
        {
            Destroy(gameObject); // Garante que só exista uma instância do singleton
        }
        
    }

    void Start(){
        CarregarDoProgressoSalvo();
    }

    public void CarregarDoProgressoSalvo()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogWarning("GameProgress.Instance está nulo. Certifique-se de carregar o progresso primeiro.");
            return;
        }

        // Atualizando o estado de desbloqueio para os itens de poções
        foreach (var pocao in pocoes)
        {
            // Verifica se o id da poção está na lista de desbloqueados
            pocao.desbloqueada = GameProgress.Instance.pocoes.Any(p => p.id == pocao.id); // Comparando com o id de InventoryEntry
        }

        // Atualizando o estado de desbloqueio para os itens
        foreach (var item in itens)
        {
            item.desbloqueada = GameProgress.Instance.itens.Any(i => i.id == item.id); // Comparando com o id de InventoryEntry
        }

        // Atualizando o estado de desbloqueio para os equipamentos
        foreach (var equipamento in equipamentos)
        {
            equipamento.desbloqueada = GameProgress.Instance.equipamentos.Any(e => e.id == equipamento.id); // Comparando com o id de InventoryEntry
        }  

        maxHealth = GameProgress.Instance.heartsMax;
        currentHealth = GameProgress.Instance.heartsCurrent;
        dashUnlocked = GameProgress.Instance.dashes;

        Debug.Log("Inventário carregado do GameProgress.");
    }


    // [System.Serializable]
    // public class Slot
    // {
    //     public Image icone;
    //     public TextMeshProUGUI titulo;
    // }

    // [Header("Poções")]
    // public Slot[] slotsDasPocoes;

    // [Header("Itens do Elixir")]
    // public Slot[] slotsDosItens;

    // [Header("Equipamentos")]
    // public Slot[] slotsDosEquipamentos;

    // [Header("Elixir da Vida")]
    // public Image iconeElixirDaVida;
    // public TextMeshProUGUI tituloElixirDaVida;

    // [Header("Scripts")]
    // public PotionSelect potionSelect;
    // public InventoryDisplay inventoryDisplay;
    // public DashBar dashBar;
    // public bool dashUnlocked = false;

    // private Dictionary<string, InventoryEntry> dicionarioPocoes;
    // private Dictionary<string, InventoryEntry> dicionarioItens;
    // private Dictionary<string, InventoryEntry> dicionarioEquipamentos;

    // void Awake()
    // {
    //     if (Instance != null && Instance != this)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //     Instance = this;
    //     DontDestroyOnLoad(gameObject);
        // dicionarioPocoes = inventoryData.pocoes.ToDictionary(p => p.id);
        // dicionarioItens = inventoryData.itens.ToDictionary(i => i.id);
        // dicionarioEquipamentos = inventoryData.equipamentos.ToDictionary(e => e.id);
    // }


    // void Start()
    // {
        // VerificarDesbloqueio(inventoryData.pocoes);
        // VerificarDesbloqueio(inventoryData.itens);
        // VerificarDesbloqueio(inventoryData.equipamentos);

        // // inventoryDisplay.AtualizarInventario();

        // foreach (var entry in inventoryData.pocoes)
        // {
        //     if (entry.desbloqueada)
        //         potionSelect.UnlockPotionByID(entry.id);
        // }
    // }

    // void VerificarDesbloqueio(List<InventoryEntry> lista)
    // {
    //     foreach (var entry in lista)
    //         entry.desbloqueada = GameProgress.Instance.unlockedItems.Contains(entry.id);
    // }


    // public void DesbloquearItem(string id, CategoriaItem categoria)
    // {
    //     Dictionary<string, InventoryEntry> dicionario = null;

    //     switch (categoria)
    //     {
    //         case CategoriaItem.Pocao:
    //             dicionario = dicionarioPocoes;
    //             break;
    //         case CategoriaItem.Item:
    //             dicionario = dicionarioItens;
    //             break;
    //         case CategoriaItem.Equipamento:
    //             dicionario = dicionarioEquipamentos;
    //             break;
    //     }

    //     if (dicionario != null && dicionario.TryGetValue(id, out var entry))
    //     {
    //         entry.desbloqueada = true;
    //         // if (!GameProgress.Instance.unlockedItems.Contains(id))
    //         //     GameProgress.Instance.unlockedItems.Add(id);

    //         //Poções
    //         if (categoria == CategoriaItem.Pocao)
    //             potionSelect.UnlockPotionByID(id);

    //         //Equipamentos
    //         if (categoria == CategoriaItem.Equipamento && id == "boots")
    //         {
    //             dashBar.UnlockDash();
    //             dashUnlocked = true;
    //             Debug.Log("Dash desbloqueado!");
    //         }
    //         if (categoria == CategoriaItem.Equipamento && id == "armour")
    //         {
    //             TesteVida.Instance.IncreaseMaxLife(1);
    //         }
    //         if (categoria == CategoriaItem.Equipamento && id == "crossbow")
    //         {
    //             //codigo de desbloquear mais range
    //         }

    //         AtualizarInventario();
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"Item com id '{id}' não encontrado na categoria '{categoria}'");
    //     }
    // }

    // void AtualizarInventario()
    // {
    //     for (int i = 0; i < slotsDasPocoes.Length; i++)
    //         AtualizarSlot(slotsDasPocoes[i], inventoryData.pocoes, i);

    //     for (int i = 0; i < slotsDosItens.Length; i++)
    //         AtualizarSlot(slotsDosItens[i], inventoryData.itens, i);

    //     for (int i = 0; i < slotsDosEquipamentos.Length; i++)
    //         AtualizarSlot(slotsDosEquipamentos[i], inventoryData.equipamentos, i);

    //     AtualizarElixirDaVida();
    // }

    // void AtualizarSlot(Slot slot, List<InventoryEntry> lista, int i)
    // {
    //     if (i < 0 || i >= lista.Count) return;
    //     var entry = lista[i];
    //     slot.icone.sprite = entry.icone;
    //     slot.icone.color = entry.desbloqueada ? Color.white : Color.black;
    //     slot.titulo.text = entry.desbloqueada ? entry.titulo : "???";

    //     var tooltip = slot.icone.GetComponent<DescriptionSlot>();
    //     if (tooltip != null)
    //         tooltip.descricao = entry.desbloqueada ? entry.descricao : "???";
    // }

    // void AtualizarElixirDaVida()
    // {
    //     if (inventoryData.itens.All(c => c.desbloqueada))
    //     {
    //         iconeElixirDaVida.color = new Color(1f, 0f, 0.278f);
    //         tituloElixirDaVida.text = "Elixir da Vida";
    //     }
    //     else
    //     {
    //         iconeElixirDaVida.color = Color.black;
    //         tituloElixirDaVida.text = "????";
    //     }
    // }
}
