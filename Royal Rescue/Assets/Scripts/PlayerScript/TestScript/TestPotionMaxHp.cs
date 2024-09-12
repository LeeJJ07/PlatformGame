using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEft/Potion/HealthUp")]
public class TestPotionMaxHp : TestItemEffect
{
    private GameObject player;
    public int[] increaseMaxHp = { -20, 20 };
    public int minHP = 40;
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
            int randomMaxHp = increaseMaxHp[Random.Range(0, increaseMaxHp.Length)];

            if (playerController.playerMaxHP + randomMaxHp < 40)
            {
                return false;
            }
            playerController.IncreaseMaxHp(randomMaxHp);
            return true;
        }
        else
        {
            return false;
        }
    }
}
