using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossAI : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] GameObject player;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform startPlayerPosition;

    INode checkDead;
    INode deadAction;
    INode detectPlayer;
    INode returnAction;
    INode lookPlayer;
    INode checkSkill1Probability;
    INode checkSkill1Range;
    INode skill1AttackAction;
    INode checkSkill2Probability;
    INode checkSkill2Range;
    INode skill2AttackAction;
    INode checkBaseAttackRange;
    INode baseAttackAction;
    INode followPlayer;

    Selector root;
    Sequence deadSequence;
    Sequence returnSequence;
    Selector attackSelector;
    Sequence skill1Sequence;
    Sequence skill2Sequence;
    Sequence baseAttackSequence;

    bool isDie = false;
    BehaviorTreeRunner bt;

    private void Awake()
    {
        checkDead = new CheckMiniBossHp();
        deadAction = new DeadAction();
        detectPlayer = new DetectPlayer(player.transform, startPlayerPosition);
        returnAction = new ReturnAction(transform, startPosition, speed);
        lookPlayer = new LookPlayer(transform, player.transform);
        checkSkill1Probability = new CheckProbability();
        checkSkill1Range = new CheckAttackRange();
        skill1AttackAction = new MiniBossSkill1Attack();
        checkSkill2Probability = new CheckProbability();
        checkSkill2Range = new CheckAttackRange();
        skill2AttackAction = new MiniBossSkill2Attack();
        checkBaseAttackRange = new CheckAttackRange();
        baseAttackAction = new MiniBossBaseAttack();
        followPlayer = new FollowPlayer();

        root = new Selector();
        deadSequence= new Sequence();
        returnSequence = new Sequence();
        attackSelector = new Selector();
        skill1Sequence = new Sequence();
        skill2Sequence = new Sequence();
        baseAttackSequence = new Sequence();
    }

    void Start()
    {

        deadSequence.AddNode(checkDead);
        deadSequence.AddNode(deadAction);

        returnSequence.AddNode(detectPlayer);
        returnSequence.AddNode(returnAction);

        skill1Sequence.AddNode(checkSkill1Probability);
        skill1Sequence.AddNode(checkSkill1Range);
        skill1Sequence.AddNode(skill1AttackAction);
        skill2Sequence.AddNode(checkSkill2Probability);
        skill2Sequence.AddNode(checkSkill2Range);
        skill2Sequence.AddNode(skill2AttackAction);
        baseAttackSequence.AddNode(checkBaseAttackRange);
        baseAttackSequence.AddNode(baseAttackAction);
        attackSelector.AddNode(skill1Sequence);
        attackSelector.AddNode(skill2Sequence);
        attackSelector.AddNode(baseAttackSequence);

        root.AddNode(deadSequence);
        root.AddNode(returnSequence);
        root.AddNode(lookPlayer);
        root.AddNode(attackSelector);
        root.AddNode(followPlayer);

        bt = new BehaviorTreeRunner(root);
    }

    void Update()
    {
        if (isDie)
            return;

        bt.Operator();
    }
}