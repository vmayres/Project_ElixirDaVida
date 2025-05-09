using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InventoryControll : MonoBehaviour
{
    [System.Serializable]
    public class SlotPocao
    {
        public Image icone;
        public TextMeshProUGUI titulo;
        public TextMeshProUGUI descricao;
    }
    [Header("Poções")]
    public Sprite sombraSprite;
    public Sprite[] iconesDasPocoes;
    public string[] titulosDasPocoes;
    public string[] descricoesDasPocoes;
    public SlotPocao[] slotsDasPocoes;
    [SerializeField] public bool[] pocaoDesbloqueada = new bool[4];


    [System.Serializable]
    public class SlotItem
    {
        public Image icone;
        public TextMeshProUGUI titulo;
    }
    [Header("Itens do Elixir")]
    public Sprite[] iconesDosItens;
    public string[] tituloDosItens;
    public string[] descricoesDosItens;
    public SlotItem[] slotsDosItens;
    [SerializeField] private bool[] itemColetado = new bool[3];

    
    [System.Serializable]
    public class SlotEquipamento
    {
        public Image icone;
        public TextMeshProUGUI titulo;
        public TextMeshProUGUI descricao;
    }
    [Header("Equipamentos")]
    public Sprite[] iconesDosEquipamentos;
    public string[] titulosDosEquipamentos;
    public string[] descricoesDosEquipamentos;
    public SlotEquipamento[] slotsDosEquipamentos;
    [SerializeField] private bool[] equipamentoColetado = new bool[3];

    [Header("Elixir da Vida")]
    public Image iconeElixirDaVida;
    public TextMeshProUGUI tituloElixirDaVida;

    [Header("Scripts")]
    public PotionSelect potionSelect;
    void Start()
    {
        for (int i = 0; i < titulosDasPocoes.Length; i++)
            pocaoDesbloqueada[i] = GameProgress.Instance.unlockedItems.Contains(titulosDasPocoes[i]);

        for (int i = 0; i < tituloDosItens.Length; i++)
            itemColetado[i] = GameProgress.Instance.unlockedItems.Contains(tituloDosItens[i]);

        for (int i = 0; i < titulosDosEquipamentos.Length; i++)
            equipamentoColetado[i] = GameProgress.Instance.unlockedItems.Contains(titulosDosEquipamentos[i]);

        AtualizarInventario();

        if(pocaoDesbloqueada.Length != 0){
            for(int i = 0; i < pocaoDesbloqueada.Length; i++){
                if (pocaoDesbloqueada[i])
                    potionSelect.UnlockNextPotion();
            }
        }

    }

    // void Update(){
    //     AtualizarInventario();
    // }

    public void DesbloquearPocao(int indice)
    {
        if (IndiceValido(indice, pocaoDesbloqueada.Length))
        {
            pocaoDesbloqueada[indice] = true;

            if (!GameProgress.Instance.unlockedItems.Contains(titulosDasPocoes[indice]))
                GameProgress.Instance.unlockedItems.Add(titulosDasPocoes[indice]);

            AtualizarInventario();
            potionSelect.UnlockNextPotion();
        }
    }

    public void ColetarItem(int indice)
    {
        if (IndiceValido(indice, itemColetado.Length))
        {
            itemColetado[indice] = true;

            if (!GameProgress.Instance.unlockedItems.Contains(tituloDosItens[indice]))
                GameProgress.Instance.unlockedItems.Add(tituloDosItens[indice]);

            AtualizarInventario();
        }
    }

    public void ColetarEquipamento(int indice)
    {
        if (IndiceValido(indice, equipamentoColetado.Length))
        {
            equipamentoColetado[indice] = true;

            if (!GameProgress.Instance.unlockedItems.Contains(titulosDosEquipamentos[indice]))
                GameProgress.Instance.unlockedItems.Add(titulosDosEquipamentos[indice]);

            AtualizarInventario();
        }
    }

    void AtualizarInventario()
    {
        for (int i = 0; i < slotsDasPocoes.Length; i++)
            AtualizarSlotPocao(slotsDasPocoes[i], i);

        for (int i = 0; i < slotsDosItens.Length; i++)
            AtualizarSlotItem(slotsDosItens[i], i);

        for (int i = 0; i < slotsDosEquipamentos.Length; i++)
            AtualizarSlotEquipamento(slotsDosEquipamentos[i], i);

        AtualizarElixirDaVida();
    }

    void AtualizarSlotPocao(SlotPocao slot, int i)
    {
        slot.icone.sprite = iconesDasPocoes[i];
        slot.icone.color = pocaoDesbloqueada[i] ? Color.white : Color.black;
        slot.titulo.text = pocaoDesbloqueada[i] ? titulosDasPocoes[i] : "???";
        // slot.descricao.text = pocaoDesbloqueada[i] ? descricoesDasPocoes[i] : "???";

        var tooltip = slot.icone.GetComponent<DescriptionSlot>();
        if (tooltip != null)
        {
            tooltip.descricao = pocaoDesbloqueada[i] ? descricoesDasPocoes[i] : "???";
        }
    }

    void AtualizarSlotItem(SlotItem slot, int i)
    {
        slot.icone.sprite = iconesDosItens[i];
        slot.icone.color = itemColetado[i] ? Color.white : Color.black;
        slot.titulo.text = itemColetado[i] ? tituloDosItens[i] : "???";

        var tooltip = slot.icone.GetComponent<DescriptionSlot>();
        if (tooltip != null)
        {
            tooltip.descricao = itemColetado[i] ? descricoesDosItens[i] : "???";
        }
    }

    void AtualizarSlotEquipamento(SlotEquipamento slot, int i)
    {
        slot.icone.sprite = iconesDosEquipamentos[i];
        slot.icone.color = equipamentoColetado[i] ? Color.white : Color.black;
        slot.titulo.text = equipamentoColetado[i] ? titulosDosEquipamentos[i] : "???";
        // slot.descricao.text = equipamentoColetado[i] ? descricoesDosEquipamentos[i] : "???";

        var tooltip = slot.icone.GetComponent<DescriptionSlot>();
        if (tooltip != null)
        {
            tooltip.descricao = equipamentoColetado[i] ? descricoesDosEquipamentos[i] : "???";
        }
    }

    void AtualizarElixirDaVida()
    {
        if (itemColetado.All(c => c))
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

    bool IndiceValido(int i, int tamanho) => i >= 0 && i < tamanho;
}
