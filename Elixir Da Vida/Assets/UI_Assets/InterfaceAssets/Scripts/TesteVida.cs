using UnityEngine;

public class TesteVida : MonoBehaviour
{
    public HeartDisplay heartDisplay;
    public DashBar dashBar;
    public PotionSelect potionSelector;
    public InventoryControll inventoryControll;
    // public HeartControll heartControll;
    public int maxLife;
    public int lastlife;
    public int currentLife;
    public int contadorPotion = 0;
    public int contadorEquip = 0;


    void Start()
    {
        maxLife = GameProgress.Instance.heartsMax;
        currentLife = GameProgress.Instance.heartsCurrent;
        lastlife = currentLife;
        heartDisplay.ResetHearts(maxLife, currentLife);
        heartDisplay.UpdateHearts(currentLife, maxLife);

        contadorPotion = potionSelector.unlockedCount;
    }

    void Update()
    {
        // Pressiona A para perder vida
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(3);
        }

        // Pressiona D para curar vida
        if (Input.GetKeyDown(KeyCode.D))
        {
            Heal(1);
        }

        // Pressiona W para ganhar um novo coração extra
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseMaxLife(1);
            inventoryControll.ColetarEquipamento(contadorEquip);
            contadorEquip+=1;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dashBar.UnlockDash();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            dashBar.TryUseDash();
        }

        if (Input.GetKeyDown(KeyCode.R))
            potionSelector.RotateRight();

        if (Input.GetKeyDown(KeyCode.Y))
            potionSelector.RotateLeft();

        if (Input.GetKeyDown(KeyCode.N)){ // Teste: ganha uma poção}
            inventoryControll.DesbloquearPocao(contadorPotion);
            contadorPotion+=1;
        }
    }

    void TakeDamage(int amount)
    {
        int previousLife = currentLife;
        currentLife = Mathf.Max(0, currentLife - amount);
        heartDisplay.UpdateHearts(currentLife, previousLife);
        GameProgress.Instance.heartsCurrent = currentLife;
    }

    void Heal(int amount)
    {
        int previousLife = currentLife;
        currentLife = Mathf.Min(maxLife, currentLife + amount);
        heartDisplay.UpdateHearts(currentLife, previousLife);
        GameProgress.Instance.heartsCurrent = currentLife;
    }

    void IncreaseMaxLife(int amount)
    {
        maxLife += amount;
        int previousLife = currentLife;
        currentLife = maxLife; 

        // heartDisplay.SetupHearts(maxLife);
        // heartDisplay.UpdateHearts(currentLife, previousLife);
        heartDisplay.ResetHearts(maxLife, currentLife);
        GameProgress.Instance.heartsMax = currentLife;
    }
}
