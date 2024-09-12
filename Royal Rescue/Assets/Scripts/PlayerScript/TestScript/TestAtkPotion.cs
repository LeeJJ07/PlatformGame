using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEft/Potion/PowerUp")]
public class TestAtkPotion : TestItemEffect
{
    private GameObject player;
    public int[] increaseAtk = {-5, 5 };
    public int minAtk = 5;
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
            int randomPower = increaseAtk[Random.Range(0, increaseAtk.Length)];

            if (playerController.playerBasicATK + randomPower < 5)
            {
                return false;
            }
            playerController.IncreaseAtk(randomPower);
            return true;
        }
        else
        {
            return false;
        }
    }
}
