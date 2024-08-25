using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossSkills_ScreamAttack", menuName = "BossSkills/ScreamAttack")]
public class ScreamAttackScriptableObject : ScriptableObject
{
    public ParticleSystem shockWave;
    public Transform transform;
    public GameObject[] objs;
    public int objSpawnCount = 0;
    public float ScreamAttackDistance = 0;
    public float damage = 0;
    public float pushPower = 0;
    public float subSequenceDelay;
}
