using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashBar : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject dashBarUI;         // O GameObject que contém o Slider
    public Slider slider;                // O componente Slider

    [Header("Configurações")]
    public float rechargeTime = 2f;      // Tempo total para recarregar
    private bool dashUnlocked = false;
    private bool isRecharging = false;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Start()
    {
        canvasGroup = dashBarUI.GetComponent<CanvasGroup>();
        rectTransform = dashBarUI.GetComponent<RectTransform>();

        // Inicializa escondido
        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.zero;
        dashBarUI.SetActive(false);
}

    public void UnlockDash()
    {
        dashUnlocked = true;
        dashBarUI.SetActive(true);
        slider.value = 1f;
        StartCoroutine(FadeInGrow());
    }

    private IEnumerator FadeInGrow()
    {
        float duration = 0.3f;
        float elapsed = 0f;

        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease out

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, smoothT);
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, smoothT);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        rectTransform.localScale = Vector3.one;
    }



    public bool TryUseDash()
    {
        if (!dashUnlocked || slider.value < 1f || isRecharging)
            return false;

        slider.value = 0f;
        StartCoroutine(RechargeBar());
        return true;
    }

    private IEnumerator RechargeBar()
    {
        isRecharging = true;

        float elapsed = 0f;
        while (elapsed < rechargeTime)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsed / rechargeTime);
            yield return null;
        }

        slider.value = 1f;
        isRecharging = false;
    }
}
