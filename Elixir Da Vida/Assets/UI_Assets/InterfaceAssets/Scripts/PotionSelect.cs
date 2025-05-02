using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class PotionSelect : MonoBehaviour
{
    public List<GameObject> potions; // Ordem das poções
    public List<Vector3> positions;  // Posições ao redor da roda (4 pontos)
    public List<Vector3> scales;     // Escalas para cada ponto (ex: [pequena, média, GRANDE, média])
    public List<float> alphas; // ex: [0.3f, 0.6f, 1f, 0.6f]


    private int unlockedCount = 0;
    private int currentIndex = 0;

    public void UnlockNextPotion()
    {
        if (unlockedCount < potions.Count)
        {
            potions[unlockedCount].SetActive(true);
            unlockedCount++;
            UpdatePotionPositions();
        }
    }

    public void RotateLeft()
    {
        if (unlockedCount <= 1) return;
        currentIndex = (currentIndex + 1) % unlockedCount;
        UpdatePotionPositions();
    }

    public void RotateRight()
    {
        if (unlockedCount <= 1) return;
        currentIndex = (currentIndex - 1 + unlockedCount) % unlockedCount;
        UpdatePotionPositions();
    }

    void UpdatePotionPositions()
    {
        for (int i = 0; i < unlockedCount; i++)
        {
            int posIndex = (i - currentIndex + unlockedCount) % unlockedCount;
            StartCoroutine(AnimatePotion(potions[i], positions[posIndex], scales[posIndex], alphas[posIndex]));
        }
    }

    IEnumerator AnimatePotion(GameObject potion, Vector3 targetPos, Vector3 targetScale, float targetAlpha)
    {
        RectTransform rt = potion.GetComponent<RectTransform>();
        Image img = potion.GetComponent<Image>();

        Vector3 startPos = rt.anchoredPosition;
        Vector3 startScale = potion.transform.localScale;
        float startAlpha = img.color.a;

        float duration = 0.25f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, t / duration);

            rt.anchoredPosition = Vector3.Lerp(startPos, targetPos, progress);
            potion.transform.localScale = Vector3.Lerp(startScale, targetScale, progress);

            Color c = img.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
            img.color = c;

            yield return null;
        }

        // Garantir que termina exatamente no alvo
        rt.anchoredPosition = targetPos;
        potion.transform.localScale = targetScale;
        Color finalColor = img.color;
        finalColor.a = targetAlpha;
        img.color = finalColor;
    }

}
