using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class InventoryDisplay : MonoBehaviour
{

    [System.Serializable]
    public class Slot
    {
        public Image icone;
        public TextMeshProUGUI titulo;
    }

    [Header("Poções")]
    public Slot[] slotsDasPocoes;
    public List<Sprite> pocoesSprites;

    [Header("Itens do Elixir")]
    public Slot[] slotsDosItens;
    public List<Sprite> itensSprites;

    [Header("Equipamentos")]
    public Slot[] slotsDosEquipamentos;
    public List<Sprite> equipSprites;

    [Header("Elixir da Vida")]
    public Image iconeElixirDaVida;
    public TextMeshProUGUI tituloElixirDaVida;

    [Header("Scripts")]
    public PotionSelect potionSelect;
    public DashBar dashBar;
    public bool dashUnlocked;

    private Dictionary<string, InventoryEntry> dicionarioPocoes;
    private Dictionary<string, InventoryEntry> dicionarioItens;
    private Dictionary<string, InventoryEntry> dicionarioEquipamentos;


    void Start()
    {
        var inventoryData = InventoryControll.Instance;

        dicionarioPocoes = inventoryData.pocoes.ToDictionary(p => p.id);
        dicionarioItens = inventoryData.itens.ToDictionary(i => i.id);
        dicionarioEquipamentos = inventoryData.equipamentos.ToDictionary(e => e.id);

        dashUnlocked = inventoryData.dashUnlocked;

        AtualizarInventario();

        foreach (var entry in inventoryData.pocoes)
        {
            if (entry.desbloqueada)
                potionSelect.UnlockPotionByID(entry.id);
        }
    }


    public void DesbloquearItem(string id, CategoriaItem categoria)
    {
        var inventoryData = InventoryControll.Instance;

        Dictionary<string, InventoryEntry> dicionario = null;

        switch (categoria)
        {
            case CategoriaItem.Pocao:
                dicionario = dicionarioPocoes;
                break;
            case CategoriaItem.Item:
                dicionario = dicionarioItens;
                break;
            case CategoriaItem.Equipamento:
                dicionario = dicionarioEquipamentos;
                break;
        }

        if (dicionario != null && dicionario.TryGetValue(id, out var entry))
        {
            entry.desbloqueada = true;
            // if (!GameProgress.Instance.unlockedItems.Contains(id))
            //     GameProgress.Instance.unlockedItems.Add(id);

            //Poções
            if (categoria == CategoriaItem.Pocao)
                potionSelect.UnlockPotionByID(id);

            //Equipamentos
            if (categoria == CategoriaItem.Equipamento && id == "boots")
            {
                dashBar.UnlockDash();
                dashUnlocked = true;

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerControl>().dashEnabled = true;

                InventoryControll.Instance.dashUnlocked = dashUnlocked;

                Debug.Log("Dash desbloqueado!");
            }
            if (categoria == CategoriaItem.Equipamento && id == "armour")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerControl>().IncreaseMaxLife(1);
            }
            if (categoria == CategoriaItem.Equipamento && id == "crossbow")
            {
                //codigo de desbloquear mais range
            }

            AtualizarInventario();
        }
        else
        {
            Debug.LogWarning($"Item com id '{id}' não encontrado na categoria '{categoria}'");
        }
    }

    public void AtualizarInventario()
    {
        var inventoryData = InventoryControll.Instance;

        for (int i = 0; i < slotsDasPocoes.Length; i++)
            AtualizarSlot(slotsDasPocoes[i], inventoryData.pocoes, i, pocoesSprites);

        for (int i = 0; i < slotsDosItens.Length; i++)
            AtualizarSlot(slotsDosItens[i], inventoryData.itens, i, itensSprites);

        for (int i = 0; i < slotsDosEquipamentos.Length; i++)
            AtualizarSlot(slotsDosEquipamentos[i], inventoryData.equipamentos, i, equipSprites);

        AtualizarElixirDaVida();
    }

    public void AtualizarSlot(Slot slot, List<InventoryEntry> lista, int i, List<Sprite> listaSprites)
    {
        if (i < 0 || i >= lista.Count) return;
        var entry = lista[i];
        slot.icone.sprite = listaSprites[i];
        slot.icone.color = entry.desbloqueada ? Color.white : Color.black;
        slot.titulo.text = entry.desbloqueada ? entry.titulo : "???";

        var tooltip = slot.icone.GetComponent<DescriptionSlot>();
        if (tooltip != null)
            tooltip.descricao = entry.desbloqueada ? entry.descricao : "???";
    }

    public void AtualizarElixirDaVida()
    {
        var inventoryData = InventoryControll.Instance;

        if (inventoryData.itens.All(c => c.desbloqueada))
        {
            iconeElixirDaVida.color = new Color(1f, 0f, 0.278f);
            tituloElixirDaVida.text = "Elixir da Vida";
        }
        else
        {
            iconeElixirDaVida.color = Color.black;
            tituloElixirDaVida.text = "????";
        }
    }
}
