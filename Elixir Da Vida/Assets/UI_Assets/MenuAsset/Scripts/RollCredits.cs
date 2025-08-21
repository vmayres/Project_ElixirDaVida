using UnityEngine;
using UnityEngine.SceneManagement;

public class RollCredits : MonoBehaviour
{
    public RectTransform creditsContainer; // arraste o CreditsContainer aqui no inspector
    public float scrollSpeed = 50f;        // pixels por segundo
    public float delayAfterEnd = 2f;       // segundos antes de permitir voltar

    private bool creditsFinished = false;
    private float timer = 0f;

    void Update()
    {
        // Rolar para cima até sair completamente da tela
        if (!creditsFinished)
        {
            creditsContainer.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            float screenHeight = Screen.height;
            float contentTopY = creditsContainer.anchoredPosition.y;
            float contentHeight = creditsContainer.sizeDelta.y;

            if (contentTopY >= contentHeight)
            {
                creditsFinished = true;
                timer = 0f;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > delayAfterEnd && Input.anyKeyDown)
            {
                Time.timeScale = 1;
                Initiate.Fade("Menu", Color.black, 0.8f);
            }
        }
    }
}
