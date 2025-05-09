using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class room : MonoBehaviour
{
    public enum DoorState
    {
        IsWall,
        IsOpen,
        IsFragile,
        Islocked,
    }

    [SerializeField] public DoorState upDoorState = DoorState.IsWall;
    [SerializeField] public DoorState downDoorState = DoorState.IsWall;
    [SerializeField] public DoorState leftDoorState = DoorState.IsWall;
    [SerializeField] public DoorState rightDoorState = DoorState.IsWall;

    [SerializeField] public GameObject upDoor;
    [SerializeField] public GameObject downDoor;
    [SerializeField] public GameObject leftDoor;
    [SerializeField] public GameObject rightDoor;

    [SerializeField] private Sprite wallSprite; // Sprite para portas do tipo Wall
    [SerializeField] private Sprite fragileSprite; // Sprite para portas do tipo Fragile
    [SerializeField] private Sprite lockedSprite; // Sprite para portas do tipo Locked

    private void OnValidate()
    {
        // Atualiza o estado das portas com base no DoorState
        UpdateDoorState(upDoor, upDoorState);
        UpdateDoorState(downDoor, downDoorState);
        UpdateDoorState(leftDoor, leftDoorState);
        UpdateDoorState(rightDoor, rightDoorState);
    }

    private void UpdateDoorState(GameObject door, DoorState state)
    {
        if (door == null) return;

        var spriteRenderer = door.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        switch (state)
        {
            case DoorState.IsWall:
                door.SetActive(true);
                spriteRenderer.sprite = wallSprite;
                break;

            case DoorState.IsOpen:
                door.SetActive(false);
                break;

            case DoorState.IsFragile:
                door.SetActive(true);
                spriteRenderer.sprite = fragileSprite;
                break;

            case DoorState.Islocked:
                door.SetActive(true);
                spriteRenderer.sprite = wallSprite; // Ou outro sprite para portas trancadas
                break;
        }
    }

    // Função para destruir/desativar uma porta do tipo Fragile
    public void OpenDoor(GameObject door)
    {
        if (door == null) return;
        door.SetActive(false);
        Debug.Log($"{door.name} foi aberta//destruida!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenDoor(upDoor);
        }
    }
}
