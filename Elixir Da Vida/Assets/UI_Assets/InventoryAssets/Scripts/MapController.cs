using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour
{
    public Animator mapaAnimator;
    public RectTransform mapaRect;
    public GameObject mapPannel;  

    public Vector2 posicaoFechado = new Vector2(-500f, -200f); // ao lado do livro
    public Vector2 posicaoAberto = new Vector2(0f, 0f); // centro da tela
    public float velocidadeMovimento = 800f;

    public bool mapaAberto = false;
    public bool emTransicao = false;

    [Header("Menu")]
    public MenuControllers menuController;  // Referência ao MapController

    void Start()
    {
        mapaRect.anchoredPosition = posicaoFechado;
        // mapaAnimator.SetBool("Aberto", false);
        mapPannel.SetActive(false); // garantir que comece desativado
    }

    void Update()
    {
        if (menuController.inInventory && Input.GetKeyDown(KeyCode.M) && !emTransicao)
        {
            if (!mapaAberto)
                StartCoroutine(AbrirMapa());
            else
                StartCoroutine(FecharMapa());
        }
    }

    public void FunctionAbrirMapa(){
        StartCoroutine(AbrirMapa());
    }
    public void FunctionFecharMapa(){
        StartCoroutine(FecharMapa());
    }

    IEnumerator AbrirMapa()
    {
        emTransicao = true;

        // Move até o centro
        while (Vector2.Distance(mapaRect.anchoredPosition, posicaoAberto) > 0.1f)
        {
            mapaRect.anchoredPosition = Vector2.MoveTowards(mapaRect.anchoredPosition, posicaoAberto, velocidadeMovimento * Time.deltaTime);
            yield return null;
        }

        // Toca a animação de abrir
        mapaAnimator.SetBool("Aberto", true);

        // Espera a animação terminar (ajuste conforme necessário)
        yield return new WaitForSeconds(0.7f);

        mapaAberto = true;
        emTransicao = false;
        mapPannel.SetActive(true);
    }

    IEnumerator FecharMapa()
    {
        emTransicao = true;
        mapPannel.SetActive(false);

        // Toca a animação de fechar
        mapaAnimator.SetBool("Aberto", false);

        // Espera a animação terminar
        yield return new WaitForSeconds(0.5f);

        // Move de volta para posição ao lado do livro
        while (Vector2.Distance(mapaRect.anchoredPosition, posicaoFechado) > 0.1f)
        {
            mapaRect.anchoredPosition = Vector2.MoveTowards(mapaRect.anchoredPosition, posicaoFechado, velocidadeMovimento * Time.deltaTime);
            yield return null;
        }

        mapaAberto = false;
        emTransicao = false;
    }
}
