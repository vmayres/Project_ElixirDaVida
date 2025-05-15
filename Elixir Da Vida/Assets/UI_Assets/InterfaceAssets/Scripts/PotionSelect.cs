using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PotionSelect : MonoBehaviour
{
    public enum PotionType
    {
        Fire,
        Ice,
        Earth,
        Lightning,
    }

    [System.Serializable]
    public class PotionData
    {
        public string id;
        // public GameObject potionPrefab;
        public GameObject uiImage;
    }

    public PotionData[] potions;
    private int selectedPotionIndex = 0;

    [Header("Configurações de Animação")]
    public Vector3[] positions;
    public Vector3[] scales;
    public float[] alphas;

    private PlayerControl player;

    public static event System.Action<string> OnPotionChanged;


    void Start()
    {
        player = FindAnyObjectByType<PlayerControl>();
        AtualizarSelecaoVisual();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SelecionarProxima();
        else if (Input.GetKeyDown(KeyCode.Q))
            SelecionarAnterior();
    }

    void SelecionarProxima()
    {
        int total = GetUnlockedPotionIndices().Count;
        if (total == 0) return;
        selectedPotionIndex = (selectedPotionIndex + 1) % total;
        AtualizarSelecaoVisual();
    }

    void SelecionarAnterior()
    {
        int total = GetUnlockedPotionIndices().Count;
        if (total == 0) return;
        selectedPotionIndex = (selectedPotionIndex - 1 + total) % total;
        AtualizarSelecaoVisual();
    }

    void AtualizarSelecaoVisual()
    {
        // Lista de poções ativas (desbloqueadas)
        var poçõesDesbloqueadas = new List<int>();
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i].uiImage != null && potions[i].uiImage.activeSelf)
            {
                poçõesDesbloqueadas.Add(i);
            }
        }

        // Impede index inválido
        if (selectedPotionIndex >= poçõesDesbloqueadas.Count)
            selectedPotionIndex = 0;

        for (int i = 0; i < poçõesDesbloqueadas.Count && i < positions.Length; i++)
        {
            int indexNaLista = (i - selectedPotionIndex + poçõesDesbloqueadas.Count) % poçõesDesbloqueadas.Count;
            int indexReal = poçõesDesbloqueadas[i];

            GameObject uiObj = potions[indexReal].uiImage;
            RectTransform rt = uiObj.GetComponent<RectTransform>();
            Image img = uiObj.GetComponent<Image>();

            if (rt != null && img != null)
            {
                StartCoroutine(AnimatePotion(rt, positions[indexNaLista], scales[indexNaLista], alphas[indexNaLista], img));
            }
        }

        // Define a poção ativa no Player
        if (player != null && poçõesDesbloqueadas.Count > 0)
        {
            int indexReal = poçõesDesbloqueadas[selectedPotionIndex];
            string potionId = potions[indexReal].id;

            // Converte string ID para enum, se os nomes forem iguais
            if (System.Enum.TryParse(potionId, out PlayerControl.PotionType tipo))
            {
                player.ActivePotion = tipo;
                Debug.Log("Poção ativa do player agora é: " + tipo);

                OnPotionChanged?.Invoke(potionId);
            }
            else
            {
                Debug.LogWarning("ID de poção não corresponde ao enum: " + potionId);
            }
        }
    }


    IEnumerator AnimatePotion(RectTransform rt, Vector3 targetPos, Vector3 targetScale, float targetAlpha, Image img)
    {
        Vector3 startPos = rt.anchoredPosition;
        Vector3 startScale = rt.localScale;
        float startAlpha = img.color.a;

        float duration = 0.25f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, t / duration);

            rt.anchoredPosition = Vector3.Lerp(startPos, targetPos, progress);
            rt.localScale = Vector3.Lerp(startScale, targetScale, progress);

            Color c = img.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
            img.color = c;

            yield return null;
        }

        rt.anchoredPosition = targetPos;
        rt.localScale = targetScale;
        Color finalColor = img.color;
        finalColor.a = targetAlpha;
        img.color = finalColor;

    }

    // public GameObject GetSelectedPotionPrefab()
    // {
    //     return potions[selectedPotionIndex].potionPrefab;
    // }

    public void UnlockPotionByID(string id)
    {
        foreach (var potion in potions)
        {
            if (potion.id == id && potion.uiImage != null)
            {
                potion.uiImage.SetActive(true);
                AtualizarSelecaoVisual();
                Debug.Log($"Poção '{id}' desbloqueada.");
                return;
            }
        }

        Debug.LogWarning($"Nenhuma poção com ID '{id}' foi encontrada no PotionSelect.");
    }

    List<int> GetUnlockedPotionIndices()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i].uiImage != null && potions[i].uiImage.activeSelf)
                list.Add(i);
        }
        return list;
    }
}
