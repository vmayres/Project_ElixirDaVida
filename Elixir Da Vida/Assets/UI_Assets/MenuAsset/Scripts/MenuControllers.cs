using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuControllers : MonoBehaviour
{
    [Header("Painéis de Pause")]
    public GameObject pausePanel;         // Menu de pausa in-game
    public GameObject mainPanel;         // Menu de pausa in-game
    public GameObject settingsPanel;      // Painel de configurações
    public GameObject controlsPanel;      // Painel de controles
    public GameObject quitPanel;          // Painel de Are You Sure
    public GameObject savesPanel;         // Painel dos saves do jogo
    private bool isPaused = false;
    private bool inSettings = false;
    private bool inControls = false;
    private bool inAreYourSure = false;
    private bool inSavePage = false;

    [Header("Controle de Som")]
    public SpriteRenderer somButton;
    public Image somUIImage;
    public Sprite somLigadoSprite;
    public Sprite somDesligadoSprite;
    private bool somLigado = true;    

    [Header("Paineis de Inventario")]
    public GameObject inventoryPanel;
    
    public RectTransform inventarioTransform;
    public float tempoDeTransicao = 0.3f;

    public Vector2 posicaoFechado = new Vector2(-500f, -200f); 
    public Vector2 posicaoAberto = new Vector2(0f, 0f);         // Centro da tela
    public bool inInventory = false;

    [Header("Mapa")]
    public MapController mapController;  // Referência ao MapController

    [Header("Pause")]
    public RectTransform pausePanelRect;
    public float transitionDuration = 0.5f;

    private Vector2 centerPos = Vector2.zero;
    private Vector2 leftOffset = new Vector2(-4f, 0); // Ajuste isso conforme sua resolução

    public Color loadToColor = Color.black;

    void Start()
    {
        // somLigado = PlayerPrefs.GetInt("SomLigado", 1) == 1;
        AtualizarSom();

        if (inventarioTransform != null)
        {
            inventarioTransform.anchoredPosition = posicaoFechado;
            inventarioTransform.localScale = Vector3.zero;
            inventarioTransform.gameObject.SetActive(false);
        }

    }

    void Update()
    {
        if (inventoryPanel != null && !isPaused && !inSettings && !inControls && Input.GetKeyDown(KeyCode.E)){
            if (!inInventory)
                StartCoroutine(AbrirInventario());
            else
                StartCoroutine(FecharInventario());
        }
        
        if (!inInventory && Input.GetKeyDown(KeyCode.Escape))
        {
            if (inControls)
                ToggleSettingsSection(); // Volta de controles para settings
            else if (inSettings && pausePanel != null)
                ToggleMainSettings();    // Volta de settings para pause
            else if (inSettings && pausePanel == null)
                ToggleMenuSettings();
            else if (inInventory)
                StartCoroutine(FecharInventario());
                // ToggleInvetory();
            else
                if (pausePanel != null)
                {
                    TogglePause();  // Abre ou fecha o pause
                }
        }

    }

    // public void ToggleInvetory(){
    //     inInventory = !inInventory;
    //     inventoryPanel.SetActive(inInventory);
    // }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if(!isPaused){
            settingsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }
    }

    public void ToggleSound()
    {
        somLigado = !somLigado;
        PlayerPrefs.SetInt("SomLigado", somLigado ? 1 : 0);
        PlayerPrefs.Save();
        AtualizarSom();
    }

    void AtualizarSom()
    {
        AudioListener.volume = somLigado ? 1f : 0f;
        Sprite spriteAtual = somLigado ? somLigadoSprite : somDesligadoSprite;

        if (somUIImage != null)
        {
            somUIImage.sprite = spriteAtual;
        }
        else if (somButton != null)
        {
            somButton.sprite = spriteAtual;
        }
    }

    public void ChangeSceneFade(string sceneName)
    {
        Time.timeScale = 1;
        Initiate.Fade(sceneName, loadToColor, 0.5f);
    }
    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void MovePauseToLeft()
    {
        StopAllCoroutines();
        StartCoroutine(DeslizarPainel(pausePanelRect, centerPos, leftOffset));
    }

    public void MovePauseToCenter()
    {
        StopAllCoroutines();
        StartCoroutine(DeslizarPainel(pausePanelRect, leftOffset, centerPos));
    }

    IEnumerator DeslizarPainel(RectTransform painel, Vector2 startPos, Vector2 endPos)
    {
        float t = 0f;
        painel.anchoredPosition = startPos;

        while (t < transitionDuration)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / transitionDuration;
            painel.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            yield return null;
        }

        painel.anchoredPosition = endPos;
    }

    public void TogglePlay()
    {
        inSavePage = !inSavePage;
        savesPanel.SetActive(inSavePage);
        mainPanel.SetActive(!inSavePage);
    }

    // Alterna entre Menu ou PauseMenu e Configurações
    public void ToggleMainSettings()
    {
        inSettings = !inSettings;

        if (inSettings)
        {
            StartCoroutine(MoverPauseDepoisMostrarSettings());
        }
        else
        {
            settingsPanel.SetActive(false);
            MovePauseToCenter();
        }
    }

    IEnumerator MoverPauseDepoisMostrarSettings()
    {
        // Move para a esquerda
        yield return StartCoroutine(DeslizarPainel(pausePanelRect, centerPos, leftOffset));

        // Ativa o painel de settings somente depois da transição
        settingsPanel.SetActive(true);
    }

    public void ToggleMenuSettings()
    {
        inSettings = !inSettings;

        settingsPanel.SetActive(inSettings);
        mainPanel.SetActive(!inSettings);
    }

    // Alterna entre Configurações e Controles
    public void ToggleSettingsSection()
    {
        inControls = !inControls;
        controlsPanel.SetActive(inControls);
        settingsPanel.SetActive(!inControls);
    }

    public void ToggleInventory(){
        // inInventory = !inInventory;

        if(!inInventory){
            StartCoroutine(AbrirInventario());
        }
        else{
            StartCoroutine(FecharInventario());
        }
        
    }
    IEnumerator AbrirInventario()
    {
        if (mapController != null)
        {
            mapController.mapPannel.SetActive(false);
            mapController.mapaAberto = false;
            mapController.emTransicao = false;
            mapController.mapaRect.anchoredPosition = mapController.posicaoFechado;
        }

        inventarioTransform.gameObject.SetActive(true);

        float t = 0f;
        Vector3 escalaInicial = Vector3.zero;
        Vector3 escalaFinal = Vector3.one;

        Vector2 posInicial = posicaoFechado;
        Vector2 posFinal = posicaoAberto;

        while (t < tempoDeTransicao)
        {
            t += Time.deltaTime;
            float progress = t / tempoDeTransicao;

            inventarioTransform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, progress);
            inventarioTransform.anchoredPosition = Vector2.Lerp(posInicial, posFinal, progress);

            yield return null;
        }

        inventarioTransform.localScale = escalaFinal;
        inventarioTransform.anchoredPosition = posFinal;

        inInventory = true;
    }

    IEnumerator FecharInventario()
    {

        float t = 0f;
        Vector3 escalaInicial = Vector3.one;
        Vector3 escalaFinal = Vector3.zero;

        Vector2 posInicial = posicaoAberto;
        Vector2 posFinal = posicaoFechado;

        while (t < tempoDeTransicao)
        {
            t += Time.deltaTime;
            float progress = t / tempoDeTransicao;

            inventarioTransform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, progress);
            inventarioTransform.anchoredPosition = Vector2.Lerp(posInicial, posFinal, progress);

            yield return null;
        }

        inventarioTransform.localScale = escalaFinal;
        inventarioTransform.anchoredPosition = posFinal;
        inventarioTransform.gameObject.SetActive(false);

        inInventory = false;
    }

    public void Quit()
    {
        // Se o jogo estiver rodando no editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Se for o jogo compilado, fecha o aplicativo
            Application.Quit();
        #endif
    }
    
    public void ToggleAreYouSure(){
        inAreYourSure = !inAreYourSure;
        mainPanel.SetActive(!inAreYourSure);
        quitPanel.SetActive(inAreYourSure);
    }
}
