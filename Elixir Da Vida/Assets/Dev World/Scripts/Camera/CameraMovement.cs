using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private float defaultSize = 5f;

    [System.Serializable]
    private struct WorldBounds
    {
        public float minX, maxX, minY, maxY;
    }
    [SerializeField] private WorldBounds worldBounds;

    private Camera cam;
    private PlayerControl playerControl;
    private Collider2D currentRoom = null;
    private Vector3 fixedCameraPosition;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = defaultSize;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.192f, 0.301f, 0.474f, 1f);

        if (player != null)
        {
            playerControl = player.GetComponent<PlayerControl>();
        }
    }

    void Update()
    {
        if (playerControl != null && playerControl.CurrentRoom != null)
        {
            // Entrou numa sala nova
            if (playerControl.CurrentRoom != currentRoom)
            {
                FocusOnRoom(playerControl.CurrentRoom.bounds, playerControl.CurrentRoom);
            }

            // Enquanto estiver na sala, manter câmera fixa suavemente
            Vector3 newPosition = Vector3.Lerp(transform.position, fixedCameraPosition, cameraSpeed * Time.deltaTime);
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            // Saiu da sala: voltar a seguir o jogador
            if (currentRoom != null)
            {
                ResumeFollowPlayer(currentRoom);
            }

            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Lerp(newPosition.x, player.transform.position.x, cameraSpeed * Time.deltaTime);
            newPosition.y = Mathf.Lerp(newPosition.y, player.transform.position.y, cameraSpeed * Time.deltaTime);
            newPosition.z = transform.position.z;

            newPosition.x = Mathf.Clamp(newPosition.x, worldBounds.minX, worldBounds.maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, worldBounds.minY, worldBounds.maxY);

            transform.position = newPosition;
            cam.orthographicSize = defaultSize;
        }
    }

    public void FocusOnRoom(Bounds bounds, Collider2D room)
    {
        currentRoom = room;

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float roomWidth = bounds.size.x;
        float roomHeight = bounds.size.y;

        float targetOrthoSizeHeight = roomHeight / 2f;
        float targetOrthoSizeWidth = (roomWidth / screenAspect) / 2f;

        float targetOrthoSize = Mathf.Max(targetOrthoSizeHeight, targetOrthoSizeWidth);
        cam.orthographicSize = targetOrthoSize;

        fixedCameraPosition = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);
    }

    public void ResumeFollowPlayer(Collider2D exitedRoom)
    {
        if (exitedRoom == currentRoom)
        {
            currentRoom = null;
            cam.orthographicSize = defaultSize;
        }
    }
}
