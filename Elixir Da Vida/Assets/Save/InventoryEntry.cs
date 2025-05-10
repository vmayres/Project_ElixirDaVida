using UnityEngine;

[System.Serializable]
public class InventoryEntry {
    public string id;
    public string titulo;
    public string descricao;
    public bool desbloqueada;
    public CategoriaItem categoria;

    public InventoryEntry(InventoryEntry other)
    {
        id = other.id;
        titulo = other.titulo;
        descricao = other.descricao;
        desbloqueada = other.desbloqueada;
        categoria = other.categoria;
    }
}

public enum CategoriaItem
{
    Pocao,
    Item,
    Equipamento
}