using UnityEngine;

public class TesteVida : MonoBehaviour
{
    public HeartDisplay heartDisplay;

    public int maxLife;
    public int lastlife;
    public int currentLife;

    void Start()
    {
        maxLife = InventoryControll.Instance.maxHealth;
        currentLife = InventoryControll.Instance.currentHealth;
        lastlife = currentLife;
        heartDisplay.ResetHearts(maxLife, currentLife);
        heartDisplay.UpdateHearts(currentLife, maxLife);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) TakeDamage(3);
        if (Input.GetKeyDown(KeyCode.D)) Heal(1);
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseMaxLife(1);
        }
        // if (Input.GetKeyDown(KeyCode.Q)) dashBar.UnlockDash();
        // if (Input.GetKeyDown(KeyCode.S)) dashBar.TryUseDash();
        // if (Input.GetKeyDown(KeyCode.R)) potionSelector.RotateRight();
        // if (Input.GetKeyDown(KeyCode.Y)) potionSelector.RotateLeft();
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     inventoryControll.DesbloquearPocao(contadorPotion);
        //     contadorPotion += 1;
        // }
    }

    public void TakeDamage(int amount)
    {
        int previousLife = currentLife;
        currentLife = Mathf.Max(0, currentLife - amount);
        heartDisplay.UpdateHearts(currentLife, previousLife);
        InventoryControll.Instance.currentHealth = currentLife;
    }

    public void Heal(int amount)
    {
        int previousLife = currentLife;
        currentLife = Mathf.Min(maxLife, currentLife + amount);
        heartDisplay.UpdateHearts(currentLife, previousLife);
        InventoryControll.Instance.currentHealth = currentLife;
    }

    public void IncreaseMaxLife(int amount)
    {
        maxLife += amount;
        currentLife = maxLife;
        heartDisplay.ResetHearts(maxLife, currentLife);
        InventoryControll.Instance.currentHealth = currentLife;
        InventoryControll.Instance.maxHealth = maxLife;
    }
}
