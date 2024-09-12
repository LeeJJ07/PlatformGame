using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEft/Potion/SpeedUP")]
public class TestSpeedPotion : TestItemEffect
{
    private GameObject player;
    public float speedPlus = 1.0f;
    public void SetPlayer(GameObject playerOb)
    {
        player = playerOb;
    }
    public override bool ExecuteRole()
    {
        if (player == null)
            return false;
        PlayerControlManagerFix playerController = player.GetComponent<PlayerControlManagerFix>();
        if (playerController != null)
        {
            playerController.IncreaseSpeed(speedPlus);
            return true;
        }
        else
        {
            return false;
        }
    }
}
