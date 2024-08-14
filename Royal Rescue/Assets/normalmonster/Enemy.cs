using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [Header("Enemy States")]
    [SerializeField] private PatrolState patrolState;
    [SerializeField] private ChaseState chaseState;
    [SerializeField] private AttackState attackState;
    [SerializeField] private DeathState deathState;

    private EnemyStateContext enemyStateContext;
    private EState curState;

    public GameObject player;

    public bool isDetect { get; set; }
    private bool isLiving;
    private float hp { get; set; }

    private float checkObstacleDistance = 0.5f;
    private float toGroundDistance = 1f;
    private float toWallDistance = 0.5f;

    private float speed = 3f;
    public float facingDir = 1f;

    private float detectingDistance = 4f;
    private float detectingAngle = 50f;

    private float chaseDistance = 6f;
    private float attackDistance = 1.5f;

    private int groundLayerMask;
    private int wallLayerMask;
    private int playerMastk;

    private void Start()
    {
        enemyStateContext = new EnemyStateContext(this);
        enemyStateContext.Transition(patrolState);
        curState = EState.PATROL;

        isDetect = false;
        isLiving = true;
        hp = 100f;

        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        playerMastk = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if (!isLiving)
            return;
        switch (curState)
        {
            case EState.PATROL:
                break;
            case EState.CHASE:
                break;
            case EState.ATTACK:
                break;
        }

        enemyStateContext.CurrentState.UpdateState();
    }

    private void UpdateState(EState nextState)
    {
        if (curState != nextState)
            curState = nextState;

        switch (curState)
        {
            case EState.PATROL:
                enemyStateContext.Transition(patrolState);
                break;
            case EState.CHASE:
                enemyStateContext.Transition(chaseState);
                break;
            case EState.ATTACK:
                enemyStateContext.Transition(attackState);
                break;
            case EState.DEATH:
                enemyStateContext.Transition(deathState);
                break;
        }
    }
}
