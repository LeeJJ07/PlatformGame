using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSkills_BasicAttack", menuName = "BossSkills/BasicAttack")]
public class BasicAttackScriptableObject : ScriptableObject
{
    public string soundClipName;
    public float BasicAttackDistance = 0;
    public int damage = 0;
    public float subSequenceDelay;
}
