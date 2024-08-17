using System.Collections.Generic;
using UnityEngine;
//살려줘...
public class EnemyAI : MonoBehaviour
{
    [Header("Common Component")]
    [SerializeField] Transform target;
    [SerializeField] Transform flamePosition;
    [SerializeField] List<Transform> moveList = new List<Transform>();
    [SerializeField] Animator enemyAnimation;
    List<int[]> pattern;                                //패턴들을 저장할 리스트    
    public int AttackIndex = 0;                         //여러패턴중 하나를 고르기위한 인덱스 변수
    

    //공격범위
    [SerializeField] float basicAttackRange;
    [SerializeField] float screamAttackRange;
    [SerializeField] float flameAttackRange;

    //공격상태로 변할 타겟과의 거리
    [SerializeField] float attackToTargetDistance;
    [SerializeField] float hp = 100;

    [Header("Phase1")]
    [SerializeField] float Phase1moveSpeed = 3f;
    [SerializeField] float Phase1attackTime = 1.2f;
    [SerializeField] float Phase1increaseDamage = 0;
    [SerializeField] float Phase1HpCondition;
    /*[SerializeField] int[] Phase1attackPattern1;
    [SerializeField] int[] Phase1attackPattern2;*/
    //List<int[]> phase1Patterns;
    [SerializeField] List<int> Phase1attackPattern1;
    [SerializeField] List<int> Phase1attackPattern2;
    List<List<int>> phase1Patterns;


    [Header("Phase2")]
    [SerializeField] float Phase2moveSpeed = 3f;
    [SerializeField] float Phase2attackTime = 1.2f;
    [SerializeField] float Phase2increaseDamage = 0;
    [SerializeField] float Phase2HpCondition;

    [Header("Phase3")]
    [SerializeField] float Phase3moveSpeed = 3f;
    [SerializeField] float Phase3attackTime = 1.2f;
    [SerializeField] float Phase3increaseDamage = 0;
    [SerializeField] float Phase3HpCondition;

    bool isDie = false;

    BehaviorTreeRunner Bt;

    ////////////////////
    INode ChooseActionIndex;
    INode ChecktoTargetDistance;
    INode MoveNode;
    INode BasicAttackNode;
    INode FlameAttackNode;
    INode ScreamAttackNode;

    Phase1Sequence phase1;//보스체력>70%
    Sequence phase1MoveSequence;
    Phase1ActionSelector phase1ActionSelector;
    Phase1AttackArrSequence phase1AttackArrSequence;


    Sequence Phase2;//보스체력>30%
    Sequence Phase2actionSequcne;
    ArraySequence Phase2AttackSequence;

    Sequence Phase3;//보스체력<=30%
    Sequence Phase3actionSequcne;
    ArraySequence Phase3AttackSequence;


    #region 변수 캡슐화(region)
    public Transform Target { get => target;  }
    public Transform FlamePosition { get => flamePosition; }
    public List<Transform> MoveList { get => moveList; }
    public Animator EnemyAnimation { get => enemyAnimation; }
    public float BasicAttackRange { get => basicAttackRange; }
    public float ScreamSkillRange { get => screamAttackRange; }
    public float FlameSkillRange { get => flameAttackRange; }
    public float Hp { get => hp; }
    public float Phase1MoveSpeed { get => Phase1moveSpeed; }
    public float Phase1attackTime1 { get => Phase1attackTime; }
    public float Phase1increaseDamage1 { get => Phase1increaseDamage; }
    public float Phase2moveSpeed1 { get => Phase2moveSpeed; }
    public float Phase2attackTime1 { get => Phase2attackTime; }
    public float Phase2increaseDamage1 { get => Phase2increaseDamage; }
    public float Phase3moveSpeed1 { get => Phase3moveSpeed; }
    public float Phase3attackTime1 { get => Phase3attackTime; }
    public float Phase3increaseDamage1 { get => Phase3increaseDamage; }
    public float Phase1HpCondition1 { get => Phase1HpCondition; }
    public float Phase2HpCondition1 { get => Phase2HpCondition; }
    public float Phase3HpCondition1 { get => Phase3HpCondition; }
    public float AttackToTargetDistance { get => attackToTargetDistance; }

    #endregion


    private void Awake()
    {
        phase1Patterns = new List<List<int>>();

        ChecktoTargetDistance = new ChecktoTargetDistance(this, AttackToTargetDistance);
        MoveNode = new MoveNode(this);
        BasicAttackNode = new BasicAttackNode(this);
        FlameAttackNode = new FlameAttackNode(this);
        ScreamAttackNode = new ScreamAttackNode(this);
        phase1MoveSequence = new Sequence();


        phase1Patterns.Add(Phase1attackPattern1);
        phase1Patterns.Add(Phase1attackPattern2);
        phase1 = new Phase1Sequence(this);
        phase1ActionSelector = new Phase1ActionSelector();
        phase1AttackArrSequence = new Phase1AttackArrSequence(this, phase1Patterns);
        ChooseActionIndex = new ChoosePatternNode(this,phase1Patterns.Count);

        //Phase2actionSequcne =
        //Phase2AttackSequence =
        //Phase3actionSequcne =
        //Phase3AttackSequence =

        //ChooseActionIndex = 
        //MoveNode = 
        //BasicAttackNode = 
        //FlameAttackNode = 
        //ScreamAttackNode = 

    }

    void Start()
    {
        //페이지1 트리
        phase1AttackArrSequence.AddNode(BasicAttackNode);
        phase1AttackArrSequence.AddNode(FlameAttackNode);
        phase1MoveSequence.AddNode(ChecktoTargetDistance);
        phase1MoveSequence.AddNode(MoveNode);
        phase1ActionSelector.AddNode(phase1MoveSequence);
        phase1ActionSelector.AddNode(phase1AttackArrSequence);
        phase1.AddNode(ChooseActionIndex);
        phase1.AddNode(phase1ActionSelector);


        Bt = new BehaviorTreeRunner(phase1);
    }
    void Update()
    {
        if (isDie) return;
        Bt.Operator(); 
    }
    public void DeActivateObj()
    {
        isDie = true;
        //gameObject.SetActive(false);
    }
    public float lookTarget(Transform transform, Vector3 target)
    {
        Vector3 forward = transform.forward;
        Vector3 dir = target - transform.position;
        float dot = Vector3.Dot(dir, forward);
        float sign = Vector3.Cross(dir, forward).y;

        dot = Mathf.Clamp(dot, 0, 1f);
        float rot = Mathf.Acos(dot) * Mathf.Deg2Rad;
        if (sign > 0)
            return rot;
        return -rot;
    }

}
