using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameProgress.Instance != null)
        {
            transform.position = GameProgress.Instance.playerPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
