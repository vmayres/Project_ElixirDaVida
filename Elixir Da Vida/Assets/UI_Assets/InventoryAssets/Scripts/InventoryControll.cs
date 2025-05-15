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

    public HashSet<int> falasJaLidasGolemFora;
    public HashSet<int> falasJaLidasGolemLab;

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

    public void CarregarDoProgressoSalvo()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogWarning("GameProgress.Instance está nulo. Certifique-se de carregar o progresso primeiro.");
            return;
        }

        // Atualiza as propriedades das poções existentes (sem substituir os objetos)
        foreach (var entry in pocoes)
        {
            var dadosSalvos = GameProgress.Instance.pocoes.FirstOrDefault(p => p.id == entry.id);
            if (dadosSalvos != null)
            {
                entry.desbloqueada = dadosSalvos.desbloqueada;
                // Se quiser copiar mais campos (como quantidade, upgrades etc), adicione aqui
            }
        }

        foreach (var entry in itens)
        {
            var dadosSalvos = GameProgress.Instance.itens.FirstOrDefault(i => i.id == entry.id);
            if (dadosSalvos != null)
            {
                entry.desbloqueada = dadosSalvos.desbloqueada;
            }
        }

        foreach (var entry in equipamentos)
        {
            var dadosSalvos = GameProgress.Instance.equipamentos.FirstOrDefault(e => e.id == entry.id);
            if (dadosSalvos != null)
            {
                entry.desbloqueada = dadosSalvos.desbloqueada;
            }
        }

        falasJaLidasGolemFora = GameProgress.Instance.falasJaLidasGolemFora;
        falasJaLidasGolemLab = GameProgress.Instance.falasJaLidasGolemLab;
        maxHealth = GameProgress.Instance.heartsMax;
        currentHealth = GameProgress.Instance.heartsCurrent;
        dashUnlocked = GameProgress.Instance.dashes;

        Debug.Log("Inventário carregado do GameProgress.");
    }

    public bool TemItemDesbloqueado(string id)
    {
        return itens.Any(p => p.id == id && p.desbloqueada);
    }

    public bool TemPocaoDesbloqueada(string id)
    {
        return pocoes.Any(p => p.id == id && p.desbloqueada);
    }
    
    public bool TemEquipDesbloqueado(string id)
    {
        return equipamentos.Any(p => p.id == id && p.desbloqueada);
    }

}
