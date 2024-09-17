using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalMonster Data", menuName = "Scriptable Object/NormalMonster Data")]
public class NormalMonsterData : ScriptableObject
{
    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }
    [SerializeField] 
    private float awakeRange;
    public float AwakeRange { get { return awakeRange; } }
    [SerializeField]
    private float sightRange;
    public float SightRange { get { return sightRange; } }
    [SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
    [SerializeField]
    private float runSpeed;
    public float RunSpeed { get { return runSpeed; } }
    [SerializeField]
    private float detectingAngle;
    public float DetectingAngle { get { return detectingAngle; } }
    [SerializeField]
    private string patrolSound;
    public string PatrolSound { get {  return patrolSound; } }
    [SerializeField]
    private string chaseSound;
    public string ChaseSound { get { return chaseSound; } }
    [SerializeField]
    private string attackSound;
    public string AttackSound { get { return attackSound; } }
    [SerializeField]
    private string dieSound;
    public string DieSound { get { return dieSound; } }
}