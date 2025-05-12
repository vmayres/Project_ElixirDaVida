using UnityEngine;

public class HealEffect : CollectibleEffect
{
    public int amount = 1;

    public override void Apply()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerControl>().Heal(amount);
    }
}
