using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossSkills_BreathAttack", menuName = "BossSkills/BreathAttack")]
public class BreathAttackScriptableObject : ScriptableObject
{
    public GameObject breathObj;
    public float attackDistance;
    public int damage;
    public float tickDamage;
    public float subSequenceDelay;
    public bool isContinuousParticleAttack;
}
