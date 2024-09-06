using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSkills_RushAttack", menuName = "BossSkills/RushAttack")]
public class RushAttackScriptableObject : ScriptableObject
{
    public GameObject[] objs;
    public LayerMask DetectLayers;
    public string playerTag;
    public string wallTag;
    public float warningDelay = 0;
    public float RushAttackDelay = 0;
    public float RushAttackDuration = 0;
    public float pushPower = 0;
    public float rushSpeed = 0;
    public float hitRange = 0;
    public float subSequenceDelay;
    public int damage = 0;
}
