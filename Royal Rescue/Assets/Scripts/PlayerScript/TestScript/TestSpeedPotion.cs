using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEft/Potion/SpeedUP")]
public class TestSpeedPotion : TestItemEffect
{
    private GameObject player;
    public float[] speedUpValue = {-1.0f, 1.0f, 2.0f };
    public float maxSpeed = 15.0f;
    public float minSpeed = 4.0f;
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
            float randomSpeed = speedUpValue[Random.Range(0, speedUpValue.Length)];


            if(playerController.moveSpeed + randomSpeed < 4)
            {
                return false;
            }

            playerController.IncreaseSpeed(randomSpeed);
            return true;
        }
        else
        {
            return false;
        }
    }
}
