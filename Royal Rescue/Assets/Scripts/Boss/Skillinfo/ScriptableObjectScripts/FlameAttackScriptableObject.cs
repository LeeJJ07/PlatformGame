using UnityEngine;

[CreateAssetMenu(fileName = "BossSkills_FlameAttack", menuName = "BossSkills/FlameAttack")]
public class FlameAttackScriptableObject : ScriptableObject
{
    public GameObject flameObj;
    public float flameAttackDistance = 0;
    public int flameCount;
    public float damage = 0;
    public float flameSpeed = 0;
    public float subSequenceDelay;
}
