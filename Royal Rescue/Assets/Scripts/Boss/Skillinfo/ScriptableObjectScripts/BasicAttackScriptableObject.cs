using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSkills_BasicAttack", menuName = "BossSkills/BasicAttack")]
public class BasicAttackScriptableObject : ScriptableObject
{
    public Transform transform;
    public float BasicAttackDistance = 0;
    public float damage = 0;
    public float subSequenceDelay;
}
