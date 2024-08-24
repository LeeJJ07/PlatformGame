using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSkills_RushAttack", menuName = "BossSkills/RushAttack")]
public class RushAttackScriptableObject : ScriptableObject
{
    public GameObject[] objs;
    public Transform transform;
    public LayerMask playerLayer;
    public LayerMask WallLayer;
    public string playerTag;
    public string wallTag;
    public float warningDelay = 0;
    public float RushAttackDelay = 0;
    public float RushAttackDuration = 0;
    public float damage = 0;
    public float pushPower = 0;
    public float rushSpeed = 0;
    public float subSequenceDelay;
}
