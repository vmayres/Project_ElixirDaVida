using System.Collections.Generic;
using UnityEngine;

public class ButtonActivation : MonoBehaviour
{
    [Header("Botão")]
    [SerializeField] private Sprite buttonUpSprite;
    [SerializeField] private Sprite buttonDownSprite;
    [SerializeField] private bool toggle = false; // Se verdadeiro, alterna estados

    [Header("Portas Associadas")]
    [SerializeField] private List<GameObject> linkedDoors;

    private bool isPressed = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetState(false); // começa desativado
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!toggle && isPressed) return;
            SetState(!isPressed);
        }
    }

    private void SetState(bool pressed)
    {
        isPressed = pressed;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isPressed ? buttonDownSprite : buttonUpSprite;
        }

        foreach (var doorObj in linkedDoors)
        {
            if (doorObj == null) continue;
            room roomComponent = doorObj.GetComponentInParent<room>();
            if (roomComponent != null)
            {
                if (isPressed)
                    roomComponent.OpenDoor(doorObj);
                else
                    roomComponent.CloseDoor(doorObj);
            }
        }
    }
}
