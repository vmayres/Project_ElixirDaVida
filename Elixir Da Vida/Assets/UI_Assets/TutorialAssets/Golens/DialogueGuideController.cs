using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;


public class DialogueGuideController : MonoBehaviour
{
    // [System.Serializable]
    // public class DialogueLine
    // {
    //     public string speakerName;
    //     public Sprite speakerSprite;
    //     [TextArea(2, 4)]
    //     public string text;
    // }

    [System.Serializable]
    public class FalaCondicional
    {
        public int index;             // índice da fala
        public string tipo;           // "equip", "item", "pocao"
        public string chave;          // nome da chave no inventário (ex: "crossbow")
    }


    public string nomeDoNPC; // Único por NPC
    public Sprite speakerSprite;
    public List<string> lines;


    // public DialogueLine[] lines;

    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI speakerNameText;
    public Image speakerImage;

    public float timeWrite;
    private int index;

    private class CondicaoDeFala
    {
        public int index;
        public Func<bool> condicao;

        public CondicaoDeFala(int index, Func<bool> condicao)
        {
            this.index = index;
            this.condicao = condicao;
        }
    }

    private HashSet<int> falasJaLidas;
    private List<CondicaoDeFala> condicoesDeFala;

    public List<int> falasLivres; // Escolhidas manualmente no Inspector
    public List<FalaCondicional> falasComCondicao; // Também editável no Inspector

    
    private int falaAtual = -1;

    public GameObject pocao;
    public GameObject panel;
    public GameObject zumbi1;
    public GameObject zumbi2;

    public int falaDeafault;

    private void Start()
    {
        // if (nomeDoNPC == "GolemFora")
        // {
        //     falasJaLidas = InventoryControll.Instance.falasJaLidasGolemFora;
        // }
        // else if (nomeDoNPC == "GolemLab")
        // {
        //     falasJaLidas = InventoryControll.Instance.falasJaLidasGolemLab;
        // }
        // else
        // {
        falasJaLidas = new HashSet<int>();
        // }

        condicoesDeFala = new List<CondicaoDeFala>();

        foreach (var fala in falasComCondicao)
        {
            Func<bool> condicao;

            switch (fala.tipo.ToLower())
            {
                case "equip":
                    condicao = () => InventoryControll.Instance.TemEquipDesbloqueado(fala.chave);
                    break;
                case "item":
                    condicao = () => InventoryControll.Instance.TemItemDesbloqueado(fala.chave);
                    break;
                case "pocao":
                    condicao = () => InventoryControll.Instance.TemPocaoDesbloqueada(fala.chave);
                    break;
                default:
                    condicao = () => false;
                    break;
            }

            condicoesDeFala.Add(new CondicaoDeFala(fala.index, condicao));
        }

        // // Lista de falas SEM condição (sempre podem aparecer uma vez)
        // falasLivres = new List<int>() { 20, 21, 22 }; // indices sem condição

        // condicoesDeFala = new List<CondicaoDeFala>()
        // {
        //     new CondicaoDeFala(10, () => InventoryControll.Instance.TemEquipDesbloqueado("crossbow")),
        //     new CondicaoDeFala(9, () => InventoryControll.Instance.TemEquipDesbloqueado("boots")),
        //     new CondicaoDeFala(8, () => InventoryControll.Instance.TemEquipDesbloqueado("armour")),
        //     new CondicaoDeFala(7, () => InventoryControll.Instance.TemItemDesbloqueado("memory")),
        //     new CondicaoDeFala(6, () => InventoryControll.Instance.TemItemDesbloqueado("blood")),
        //     new CondicaoDeFala(5, () => InventoryControll.Instance.TemItemDesbloqueado("root")),
        //     new CondicaoDeFala(4, () => InventoryControll.Instance.TemPocaoDesbloqueada("Lightning")),
        //     new CondicaoDeFala(3, () => InventoryControll.Instance.TemPocaoDesbloqueada("Ice")),
        //     new CondicaoDeFala(2, () => InventoryControll.Instance.TemPocaoDesbloqueada("Earth")),
        //     new CondicaoDeFala(1, () => InventoryControll.Instance.TemPocaoDesbloqueada("Fire")),
        // };
    }


    private bool podeFalar = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            podeFalar = true;

            Debug.Log("Deu trigger e encontrou player");
        // dialogueController.MostrarFalaDoGolem();
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            podeFalar = false;
            panel.SetActive(false);
    }

    private void Update()
    {
        if (podeFalar && Input.GetKeyDown(KeyCode.F))
        {
            panel.SetActive(true);
            MostrarProximaFala();
        }
    }

    // private void MostrarProximaFala()
    // {
    //     // Primeiro, verifica se há falas livres ainda não lidas
    //     foreach (int falaIndex in falasLivres)
    //     {
    //         if (!falasJaLidas.Contains(falaIndex))
    //         {
    //             falaAtual = falaIndex;
    //             falasJaLidas.Add(falaIndex);
    //             MostrarFalaEspecifica(falaIndex);
    //             return;
    //         }
    //     }

    //     // Tenta encontrar a próxima fala ainda não lida
    //     foreach (var fala in condicoesDeFala)
    //     {
    //         if (!falasJaLidas.Contains(fala.index) && fala.condicao())
    //         {
    //             falaAtual = fala.index;
    //             falasJaLidas.Add(fala.index);
    //             MostrarFalaEspecifica(fala.index);
    //             return;
    //         }
    //     }

    //     // Se todas as falas já foram lidas, mostra só a default
    //     falaAtual = 0;
    //     MostrarFalaEspecifica(0);
    // }

    private void MostrarProximaFala()
    {
        // Primeiro, fala livre não lida
        if (falasLivres != null)
        {
            Debug.Log("falas livrtes!");
            foreach (int i in falasLivres)
            {
                if (!falasJaLidas.Contains(i))
                {
                    falaAtual = i;
                    falasJaLidas.Add(i);
                    MostrarFalaEspecifica(i);
                    return;
                }
            }
        }


        // Depois, fala condicional
        if (condicoesDeFala != null)
        {
            Debug.Log("falas coms!");
            foreach (var fala in condicoesDeFala)
            {
                if (!falasJaLidas.Contains(fala.index) && fala.condicao())
                {
                    falaAtual = fala.index;
                    falasJaLidas.Add(fala.index);
                    MostrarFalaEspecifica(fala.index);
                    return;
                }
            }
        }
        
        Debug.Log("falas!");
        // Se tudo foi lido, pode repetir a fala default se quiser
        falaAtual = falaDeafault;
        MostrarFalaEspecifica(falaDeafault);
    }


    public void MostrarFalaEspecifica(int i)
    {
        if (nomeDoNPC == "GolemFora")
        {
            if (i == 5)
            {
                pocao.SetActive(true);
            }

            if (i == 6)
            {
                if (zumbi1 != null && zumbi2 != null)
                {
                    zumbi1.SetActive(true);
                    zumbi2.SetActive(true);
                }
            }
        }
        


        if (lines == null)
        {
            Debug.LogError("Lines está nulo!");
            return;
        }

        if (i < 0 || i >= lines.Count)
        {
            Debug.LogWarning($"Índice inválido: {i}, tamanho de lines: {lines.Count}");
            return;
        }

        if (textComponent == null)
        {
            Debug.LogError("TextComponent está nulo!");
            return;
        }

        index = i;
        SetSpeakerInfo();
        textComponent.text = lines[index];
        // StartCoroutine(TypeLine());
    }


    // void Update()
    // {
    //     if (Input.anyKeyDown)
    //     {
    //         if (textComponent.text == lines[index].text)
    //         {
    //             // timelineControll.ContinueTimeline();
    //             NextLine();
    //         }
    //         else
    //         {
    //             StopAllCoroutines();
    //             textComponent.text = lines[index].text;
    //         }
    //     }
    // }

    // void StartDialog()
    // {
    //     index = 0;
    //     SetSpeakerInfo();
    //     StartCoroutine(TypeLine());
    // }

    // IEnumerator TypeLine()
    // {
    //     textComponent.text = "";

    //     foreach (char c in lines[index].ToCharArray())
    //     {
    //         textComponent.text += c;
    //         yield return new WaitForSecondsRealtime(timeWrite);
    //     }
    // }

    // void NextLine()
    // {
    //     if (index < lines.Length - 1)
    //     {
    //         index++;

    //         SetSpeakerInfo();
    //         StartCoroutine(TypeLine());
    //     }
    //     else
    //     {
    //         gameObject.SetActive(false);
    //         Time.timeScale = 1f;
    //     }
    // }

    void SetSpeakerInfo()
    {
        if (speakerNameText != null)
        {
            speakerNameText.text = nomeDoNPC;
            speakerNameText.gameObject.SetActive(true);
        }

        if (speakerImage != null)
        {
            speakerImage.sprite = speakerSprite;
            speakerImage.gameObject.SetActive(true);
        }
    }

}

