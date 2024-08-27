using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossAI : MonoBehaviour
{
    [Header("중간보스 능력치")]
    [SerializeField] float hp;
    [SerializeField] float maxHp;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    [Header("설정")]
    [SerializeField] GameObject player;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform startPlayerPosition;
    [SerializeField] Animator animator;

    [Header("스킬 사용 확률")]
    [SerializeField] int skill1Probability = 25;
    [SerializeField] int skill2Probability = 33;

    [Header("스킬 범위")]
    [SerializeField] float skill1Range= 20f;
    [SerializeField] float skill2Range = 10f;
    [SerializeField] float baseAttackRange = 5f;

    [Header("공격 딜레이")]
    [SerializeField] float skill1DelayTime = 0.5f;
    [SerializeField] float skill2DelayTime = 0.5f;
    [SerializeField] float baseAttackDelayTime = 0.5f;

    [SerializeField] float followDelayTime = 0f;

    INode checkDie;
    INode dieAction;
    INode detectPlayer;
    INode returnAction;
    INode lookPlayer;
    INode checkSkill1Probability;
    INode checkSkill1Range;
    INode skill1AttackAction;
    INode skill1AttackActionDelay;
    INode checkSkill2Probability;
    INode checkSkill2Range;
    INode skill2AttackAction;
    INode skill2AttackActionDelay;
    INode checkBaseAttackRange;
    INode baseAttackAction;
    INode baseAttackActionDelay;
    INode followPlayer;
    INode followPlayerDelay;

    Selector root;
    Sequence deadSequence;
    Sequence returnSequence;
    Selector attackSelector;
    Sequence skill1Sequence;
    Sequence skill2Sequence;
    Sequence baseAttackSequence;
    Sequence followPlayerSequence;

    bool isDie = false;
    BehaviorTreeRunner bt;

    private void Awake()
    {
        checkDie = new CheckMiniBossHp(Hp);
        dieAction = new DieAction(Die, animator);
        detectPlayer = new DetectPlayer(player.transform, startPlayerPosition);
        returnAction = new ReturnAction(transform, startPosition, walkSpeed);
        lookPlayer = new LookPlayer(transform, player.transform);
        checkSkill1Probability = new CheckProbability(skill1Probability);
        checkSkill1Range = new CheckAttackRange(transform, player.transform, skill1Range);
        skill1AttackAction = new MiniBossSkill1Attack(transform, player.transform, animator, runSpeed);
        skill1AttackActionDelay = new ActionDelay(animator, skill1DelayTime);
        checkSkill2Probability = new CheckProbability(skill2Probability);
        checkSkill2Range = new CheckAttackRange(transform, player.transform, skill2Range);
        skill2AttackAction = new MiniBossSkill2Attack();
        skill2AttackActionDelay = new ActionDelay(animator, skill2DelayTime);
        checkBaseAttackRange = new CheckAttackRange(transform, player.transform, baseAttackRange);
        baseAttackAction = new MiniBossBaseAttack(animator);
        baseAttackActionDelay = new ActionDelay(animator, baseAttackDelayTime);
        followPlayer = new FollowPlayer(transform, player.transform, animator, walkSpeed, Hp);
        followPlayerDelay = new ActionDelay(animator, followDelayTime);

        root = new Selector();
        deadSequence= new Sequence();
        returnSequence = new Sequence();
        attackSelector = new Selector();
        skill1Sequence = new Sequence();
        skill2Sequence = new Sequence();
        baseAttackSequence = new Sequence();
        followPlayerSequence = new Sequence();
    }

    void Start()
    {
        maxHp = 100f;
        hp = maxHp;

        deadSequence.AddNode(checkDie);
        deadSequence.AddNode(dieAction);

        returnSequence.AddNode(detectPlayer);
        returnSequence.AddNode(returnAction);

        skill1Sequence.AddNode(checkSkill1Probability);
        skill1Sequence.AddNode(checkSkill1Range);
        skill1Sequence.AddNode(skill1AttackAction);
        skill1Sequence.AddNode(skill1AttackActionDelay);

        skill2Sequence.AddNode(checkSkill2Probability);
        skill2Sequence.AddNode(checkSkill2Range);
        skill2Sequence.AddNode(skill2AttackAction);
        skill2Sequence.AddNode(skill2AttackActionDelay);

        baseAttackSequence.AddNode(checkBaseAttackRange);
        baseAttackSequence.AddNode(baseAttackAction);
        baseAttackSequence.AddNode(baseAttackActionDelay);

        attackSelector.AddNode(skill1Sequence);
        attackSelector.AddNode(skill2Sequence);
        attackSelector.AddNode(baseAttackSequence);

        followPlayerSequence.AddNode(followPlayer);
        followPlayerSequence.AddNode(followPlayerDelay);

        root.AddNode(deadSequence);
        root.AddNode(returnSequence);
        root.AddNode(lookPlayer);
        root.AddNode(attackSelector);
        root.AddNode(followPlayerSequence);

        bt = new BehaviorTreeRunner(root);
    }

    void Update()
    {
        if (isDie)
        {
            StartCoroutine(DeActive());
            return;
        }

        bt.Operator();
    }
    public float Hp()
    {
        return hp;
    }
    public void Die()
    {
        isDie = true;
    }
    IEnumerator DeActive()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}