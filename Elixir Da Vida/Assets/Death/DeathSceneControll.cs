using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DeathSceneControll : MonoBehaviour
{
    public TMP_Text deathNumber;
    public TMP_Text deathText;

    private List<string> frases = new List<string>
    {
        "A morte é só o começo...",
        "Mais uma tentativa fracassada.",
        "Você já deveria estar acostumado com isso.",
        "Ups... tente outra vez!",
        "A prática leva à perfeição. Eventualmente.",
        "O mestre ficaria decepcionado.",
        "A poção da imortalidade ainda não está pronta.",
        "Zumbis 1, você 0.",
        "Sua determinação é admirável... ou tola.",
        "Você morreu. De novo."
    };

    void Start()
    {
        int mortes = GameProgress.Instance.deathCount; // contador de mortes no seu save
        string fraseAleatoria = frases[Random.Range(0, frases.Count)];

        deathNumber.text = $"{mortes}";
        deathText.text = $"{fraseAleatoria}";

        StartCoroutine(VoltarParaUltimoCheckpoint());
    }

    private IEnumerator VoltarParaUltimoCheckpoint()
    {
        yield return new WaitForSecondsRealtime(2f); // tempo para ler a frase
        // GameProgress.Instance.Load(GameProgress.Instance.currentSlot);
        // GameProgress.Instance.heartsCurrent = GameProgress.Instance.heartsMax;
        InventoryControll.Instance.CarregarDoProgressoSalvo();
        Initiate.Fade(GameProgress.Instance.currentScene, Color.black, 0.7f);
    }
}
