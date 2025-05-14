using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Doors : MonoBehaviour
{
    public MenuControllers menuControllers;
    public string dungeon;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene().name == "TestePause")
                GameProgress.Instance.playerPosition = other.transform.position;
            
            menuControllers.ChangeSceneFade(dungeon);
        }
    }
}
