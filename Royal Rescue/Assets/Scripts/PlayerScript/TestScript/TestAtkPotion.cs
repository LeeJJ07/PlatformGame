using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEft/Potion/PowerUp")]
public class TestAtkPotion : TestItemEffect
{
    private GameObject player;
    public int increaseAtk = 5;
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
            playerController.IncreaseAtk(increaseAtk);
            return true;
        }
        else
        {
            return false;
        }
    }
}
