using System.Collections;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerControl player;
    [SerializeField] private CircleRenderer circleRenderer;
    [SerializeField] private Camera mainCamera;

    [Header("Prefabs das Poções")]
    [SerializeField] private GameObject firePotionPrefab;
    [SerializeField] private GameObject icePotionPrefab;
    [SerializeField] private GameObject earthPotionPrefab;
    [SerializeField] private GameObject lightningPotionPrefab;

    [SerializeField] private float potionCooldown = 2.0f;
    private float timeSinceLastPotion = Mathf.Infinity;

    private float MaxRange => circleRenderer != null ? circleRenderer.GetRadius() : 1f;

    void Update()
    {
        if (mainCamera == null || playerTransform == null || circleRenderer == null || player == null)
            return;

        timeSinceLastPotion += Time.deltaTime;

        // Pega posição do mouse e calcula direção
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direction = mouseWorldPos - playerTransform.position;

        // Restringe o alcance ao raio da poção
        if (direction.magnitude > MaxRange)
            direction = direction.normalized * MaxRange;

        // Atualiza a posição da mira (cursor)
        transform.position = playerTransform.position + direction;

        // Lançar poção
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
                Debug.LogWarning("O prefab da poção não contém um script que herda de PotionBase.");
            }
        }
    }

    private GameObject GetPotionPrefab(PlayerControl.PotionType type)
    {
        return type switch
        {
            PlayerControl.PotionType.Fire => firePotionPrefab,
            PlayerControl.PotionType.Ice => icePotionPrefab,
            PlayerControl.PotionType.Earth => earthPotionPrefab,
            PlayerControl.PotionType.Lightning => lightningPotionPrefab,
            _ => null
        };
    }
}
