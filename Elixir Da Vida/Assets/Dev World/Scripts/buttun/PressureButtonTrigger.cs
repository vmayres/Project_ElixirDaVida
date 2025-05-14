using System.Collections.Generic;
using UnityEngine;

public class PressureButtonTrigger : MonoBehaviour
{
    [Header("Botão")]
    [SerializeField] private Sprite buttonUpSprite;
    [SerializeField] private Sprite buttonDownSprite;

    [Header("Portas Associadas")]
    [SerializeField] private List<GameObject> linkedDoors;

    private SpriteRenderer spriteRenderer;
    private int overlappingCount = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetState(false); // começa desativado
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            overlappingCount++;
            SetState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            overlappingCount = Mathf.Max(0, overlappingCount - 1);
            if (overlappingCount == 0)
                SetState(false);
        }
    }

    private void SetState(bool pressed)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = pressed ? buttonDownSprite : buttonUpSprite;
        }

        foreach (var doorObj in linkedDoors)
        {
            if (doorObj == null) continue;
            room roomComponent = doorObj.GetComponentInParent<room>();
            if (roomComponent != null)
            {
                if (pressed)
                    roomComponent.OpenDoor(doorObj);
                else
                    roomComponent.CloseDoor(doorObj);
            }
        }
    }
}
