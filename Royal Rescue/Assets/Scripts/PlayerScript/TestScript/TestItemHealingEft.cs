using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Potion/Healing")]
public class TestItemHealingEft : TestItemEffect
{
    public int healingPoint = 0;
    public override bool ExecuteRole()
    {
        Debug.Log("PlayerHP ADD : " + healingPoint);
        return true;
    }
}
