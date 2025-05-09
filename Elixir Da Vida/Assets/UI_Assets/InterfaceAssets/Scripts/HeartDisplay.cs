using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    [Header("Configurações")]
    public GameObject heartPrefab;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;
    private List<HeartControll> hearts = new List<HeartControll>();

    // public void SetupHearts(int maxLife)
    // {
    //     while (hearts.Count < maxLife)
    //     {
    //         GameObject heart = Instantiate(heartPrefab, transform);
    //         HeartControll heartControll = heart.GetComponent<HeartControll>();
    //         hearts.Add(heartControll);
    //     }
    // }

    public void ResetHearts(int maxLife, int currentHealth)
    {
        // Destroi todos os corações existentes
        foreach (var heart in hearts)
        {
            Destroy(heart.gameObject);
        }

        hearts.Clear();

        // Cria novos corações
        for (int i = 0; i < maxLife; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            // heart.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            HeartControll heartControll = heart.GetComponent<HeartControll>();
            hearts.Add(heartControll);

            // heartControll.Pop();
            Animator animator = heart.GetComponent<Animator>();
            animator.SetTrigger("Pop");

        }
    }

    public void UpdateHearts(int newHealth, int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i >= newHealth && i < currentHealth)
                hearts[i].Lose();
            else if (i < newHealth && i >= currentHealth)
                hearts[i].Gain();
        }
    }

}
