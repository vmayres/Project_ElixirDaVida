using System.Collections;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerControl player;
    [SerializeField] private CircleRenderer circleRenderer;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float potionCooldown = 0.5f;
    private float timeSinceLastPotion = Mathf.Infinity;

    private float MaxRange => circleRenderer != null ? circleRenderer.GetRadius() : 1f;

    void Update()
    {
        if (mainCamera == null || playerTransform == null || circleRenderer == null || player == null)
            return;

        timeSinceLastPotion += Time.deltaTime;

        // Pega posi��o do mouse e calcula dire��o
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direction = mouseWorldPos - playerTransform.position;

        // Restringe o alcance ao raio da po��o
        if (direction.magnitude > MaxRange)
            direction = direction.normalized * MaxRange;

        // Atualiza a posi��o da mira (cursor)
        transform.position = playerTransform.position + direction;

        // Lan�ar po��o
        if (Input.GetButtonDown("Fire1") && timeSinceLastPotion >= potionCooldown)
        {
            PotionBase potion = GetPotionPrefab(player.ActivePotion)?.GetComponent<PotionBase>();
            if (potion != null)
            {
                StartCoroutine(potion.LaunchPotion(transform.position, playerTransform.position));
                timeSinceLastPotion = 0f;
            }
            else
            {
                Debug.LogWarning("O prefab da po��o n�o cont�m um script que herda de PotionBase.");
            }
        }
    }

    private GameObject GetPotionPrefab(PlayerControl.PotionType type)
    {
        return player.GetPotionPrefab(type); // Agora vem do PlayerControl
    }
}
