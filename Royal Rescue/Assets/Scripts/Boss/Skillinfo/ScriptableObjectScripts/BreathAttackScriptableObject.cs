using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossSkills_BreathAttack", menuName = "BossSkills/BreathAttack")]
public class BreathAttackScriptableObject : ScriptableObject
{
    public GameObject breathObj;
    public float damage;
    public float subSequenceDelay;
}
