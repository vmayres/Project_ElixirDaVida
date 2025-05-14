
// using System.Collections;
// using TMPro;
// using UnityEngine.UI;

// public class DialogueGuideController : MonoBehaviour
// {
//     [System.Serializable]
//     public class DialogueLine
//     {
//         public string speakerName;
//         public Sprite speakerSprite;
//         [TextArea(2, 4)]
//         public string text;
//     }

//     public DialogueLine[] lines;

//     public TextMeshProUGUI textComponent;
//     public TextMeshProUGUI speakerNameText;
//     public Image speakerImage;

//     public float timeWrite;
//     private int index;

//     void Start()
//     {
//     }

//     void Update()
//     {
//         if (Input.anyKeyDown)
//         {
//             if (textComponent.text == lines[index].text)
//             {
//                 timelineControll.ContinueTimeline();
//                 NextLine();
//             }
//             else
//             {
//                 StopAllCoroutines();
//                 textComponent.text = lines[index].text;
//             }
//         }
//     }

//     void StartDialog()
//     {
//         index = 0;
//         SetSpeakerInfo();
//         StartCoroutine(TypeLine());
//     }

//     IEnumerator TypeLine()
//     {
//         textComponent.text = "";

//         foreach (char c in lines[index].text.ToCharArray())
//         {
//             textComponent.text += c;
//             yield return new WaitForSecondsRealtime(timeWrite);
//         }
//     }

//     void NextLine()
//     {
//         if (index < lines.Length - 1)
//         {
//             index++;

//             SetSpeakerInfo();
//             StartCoroutine(TypeLine());
//         }
//         else
//         {
//             gameObject.SetActive(false);
//             Time.timeScale = 1f;
//         }
//     }

//     void SetSpeakerInfo()
//     {
//         if (speakerNameText != null)
//         {
//             if (!string.IsNullOrEmpty(lines[index].speakerName))
//             {
//                 speakerNameText.text = lines[index].speakerName;
//                 speakerNameText.gameObject.SetActive(true);
//             }
//             else
//             {
//                 speakerNameText.gameObject.SetActive(false);
//             }
//         }

//         if (speakerImage != null)
//         {
//             if (lines[index].speakerSprite != null)
//             {
//                 speakerImage.sprite = lines[index].speakerSprite;
//                 speakerImage.gameObject.SetActive(true);
//             }
//             else
//             {
//                 speakerImage.gameObject.SetActive(false);
//             }
//         }
//     }



//     public void MostrarFalaEspecifica(int i)
//     {
//         if (i < 0 || i >= lines.Length) return;

//         timelineControll.ShowDialog();
//         index = i;
//         SetSpeakerInfo();
//         textComponent.text = lines[index].text;
//         StartCoroutine(TypeLine());
//     }

//     public void MostrarFalaDoGolem()
//     {
//         int falaIndex = 0;

//         if (PlayerProgress.instance.pegouIngrediente3)
//             falaIndex = 4;
//         else if (PlayerProgress.instance.pegouIngrediente2)
//             falaIndex = 3;
//         else if (PlayerProgress.instance.pegouIngrediente1)
//             falaIndex = 2;
//         else if (PlayerProgress.instance.desbloqueouLaboratorio)
//             falaIndex = 1;
//         else
//             falaIndex = 0;

//         MostrarFalaEspecifica(falaIndex);
//     }


// }

