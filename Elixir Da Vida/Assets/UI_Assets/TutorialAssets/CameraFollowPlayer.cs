using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float limSuperior = 45.9f;
    public float limInferior = -7.9f;
    public float limDireito = 44f;
    public float limEsquerdo = -56f;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Player não encontrado! Verifique se o GameObject do player tem a tag 'Player'.");
            }
        }
    }

    void LateUpdate() // melhor usar LateUpdate para seguir o player após o movimento dele
    {
        if (player == null) return;

        float clampedX = Mathf.Clamp(player.position.x, limEsquerdo, limDireito);
        float clampedY = Mathf.Clamp(player.position.y, limInferior, limSuperior);

        transform.position = new Vector3(clampedX, clampedY, -10);
    }
}
