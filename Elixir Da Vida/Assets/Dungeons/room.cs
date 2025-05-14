using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class room : MonoBehaviour
{
    public enum DoorState
    {
        IsWall,
        IsOpen,
    }

    [SerializeField] public DoorState upDoorState = DoorState.IsWall;
    [SerializeField] public DoorState downDoorState = DoorState.IsWall;
    [SerializeField] public DoorState leftDoorState = DoorState.IsWall;
    [SerializeField] public DoorState rightDoorState = DoorState.IsWall;

    [SerializeField] public GameObject upDoor;
    [SerializeField] public GameObject downDoor;
    [SerializeField] public GameObject leftDoor;
    [SerializeField] public GameObject rightDoor;

    private void OnValidate()
    {
        UpdateDoorState(upDoor, upDoorState);
        UpdateDoorState(downDoor, downDoorState);
        UpdateDoorState(leftDoor, leftDoorState);
        UpdateDoorState(rightDoor, rightDoorState);
    }

    private void UpdateDoorState(GameObject door, DoorState state)
    {
        if (door == null) return;

        switch (state)
        {
            case DoorState.IsWall:
                door.SetActive(true); // Porta está ativa como parede
                break;

            case DoorState.IsOpen:
                door.SetActive(false); // Porta está desativada (aberta)
                break;
        }
    }

    public void OpenDoor(GameObject door)
    {
        if (door == null) return;
        UpdateDoorState(door, DoorState.IsOpen);
    }

    public void CloseDoor(GameObject door)
    {
        if (door == null) return;
        UpdateDoorState(door, DoorState.IsWall);
    }

    public void ChangeDoorState(GameObject door)
    {
        if (door == null) return;

        DoorState currentState = GetDoorState(door);

        if (currentState == DoorState.IsWall)
            UpdateDoorState(door, DoorState.IsOpen);
        else if (currentState == DoorState.IsOpen)
            UpdateDoorState(door, DoorState.IsWall);
    }

    private DoorState GetDoorState(GameObject door)
    {
        if (door == upDoor) return upDoorState;
        if (door == downDoor) return downDoorState;
        if (door == leftDoor) return leftDoorState;
        if (door == rightDoor) return rightDoorState;
        return DoorState.IsWall; // padrão de segurança
    }

}