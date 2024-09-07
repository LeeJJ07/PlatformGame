using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
[CreateAssetMenu(fileName = "BossSkills_ScreamAttack", menuName = "BossSkills/ScreamAttack")]
public class ScreamAttackScriptableObject : ScriptableObject
{
    public GameObject shockWaveObj;
    public GameObject[] objs;
    public int maxSpawnCount = 0;
    public int objSpawnCount = 0;
    public float ScreamAttackDistance = 0;
    public int damage = 0;
    public float pushPower = 0;
    public float subSequenceDelay;
}
