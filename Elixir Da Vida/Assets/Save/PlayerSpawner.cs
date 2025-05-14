using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameProgress.Instance != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = GameProgress.Instance.playerPosition;
        }
    }

}
