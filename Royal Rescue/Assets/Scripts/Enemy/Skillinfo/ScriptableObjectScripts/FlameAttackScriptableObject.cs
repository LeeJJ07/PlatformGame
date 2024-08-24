using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSkills_FlameAttack", menuName = "BossSkills/FlameAttack")]
public class FlameAttackScriptableObject : ScriptableObject
{
    public Transform transform;
    public float flameAttackDistance = 0;
    public float damage = 0;
    public float flameSpeed = 0;
    public float subSequenceDelay;
}
