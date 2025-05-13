using UnityEngine;

public class Tutorial_Doors : MonoBehaviour
{
    public MenuControllers menuControllers;
    public string dungeon;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            menuControllers.ChangeSceneFade(dungeon);
        }
    }
}
