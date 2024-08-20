using System.Collections.Generic;
using UnityEngine;
//살려줘...
//TODO: 스킬패턴 랜덤으로 하지말고 확률조작 할 것...
public class EnemyAI : MonoBehaviour
{
    [Header("Common Component")]
    [SerializeField] List<GameObject> objectPrefabs = new List<GameObject>();
    [SerializeField] GameObject flamePrefab;
    [SerializeField] GameObject dangerZonePrefab;
    [SerializeField] Transform[] spawnRange;
    [SerializeField] Transform target;
    [SerializeField] Transform flamePosition;
    [SerializeField] List<Transform> moveList = new List<Transform>();
    [SerializeField] Animator enemyAnimation;               //패턴들을 저장할 리스트    
    public int AttackIndex = 0;                         //여러패턴중 하나를 고르기위한 인덱스 변수
    

    //공격범위
    [SerializeField] float basicAttackRange;
    [SerializeField] float screamAttackRange;
    [SerializeField] float flameAttackRange;

    //공격상태로 변할 타겟과의 거리
    [SerializeField] float attackToTargetDistance;
    [SerializeField] float hp = 100;

    [Header("Phase1")]
    [SerializeField] float Phase1attackTime = 1.2f;
    [SerializeField] float Phase1HpCondition;
    [SerializeField] List<int> Phase1attackPattern1;
    [SerializeField] List<int> Phase1attackPattern2;
    [SerializeField] int enemySpawnCount = 0;

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
    INode IntroNode;
    INode DieNode;
    INode DieHpConditionDecorator;
    INode ChecktoTargetDistance;
    INode MoveNode;
    INode phase1HpConditionDecorator;
    INode phase2HpConditionDecorator;
    INode phase3HpConditionDecorator;
    /// ///////////////////////

    INode phase1FlameAttackNode;
    INode phase1ScreamAttackNode;

    INode phase2FlameAttackNode;
    INode phase2ScreamAttackNode;
    INode phase2BasicAttackNode;
    INode phase2EntryNode;
    INode checkIncomingPhase2;
    
    INode phase3FlameAttackNode;
    INode phase3ScreamAttackNode;
    INode phase3BasicAttackNode;

    Selector root;

    Sequence dieSequence;

    Parallel phase1;
    Sequence phase1MoveSequence;
    Selector phase1ActionSelector;
    RandomSequence phase1AttackArrSequence;

    Parallel Phase2;//보스체력>30%
    
    Sequence EntryPhase2Sequence;
    Selector Phase2actionSelector;
    Sequence Phase2MoveSequence;
    RandomSequence Phase2AttackSequence;

    Parallel Phase3;//보스체력<=30%
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
    public float Phase1attackTime1 { get => Phase1attackTime; }
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
    public List<GameObject> ObjectPrefabs { get => objectPrefabs;}
    public int EnemySpawnCount { get => enemySpawnCount; }
    public GameObject FlamePrefab { get => flamePrefab; }

    #endregion


    private void Awake()
    {
        root = new Selector();
        
        //조건 노드들
        ChecktoTargetDistance = new ChecktoTargetDistance(this, AttackToTargetDistance);
        DieHpConditionDecorator = new CheckHp(this, 0,-100);
        checkIncomingPhase2 = new CheckIncomingPhase2();

        //행동 노드들
        MoveNode = new MoveNode(this);
        DieNode = new DieNode(this);
        phase1FlameAttackNode = new FlameAttackNode(this, Phase2HpCondition,"phase1FlameAttack");
        phase1ScreamAttackNode = new ScreamAttackNode(this, Phase2HpCondition,"phase1ScreamAttack");

        phase2EntryNode = new EntryPhase2Node(this);
        phase2FlameAttackNode = new FlameAttackNode (this, Phase3HpCondition,"phase2FlameAttack");
        phase2ScreamAttackNode = new ScreamAttackNode (this, Phase3HpCondition,"phase2ScreamAttack");
        phase2BasicAttackNode = new BasicAttackNode(this, Phase3HpCondition,"phase2BasicAttack");

        phase3FlameAttackNode = new FlameAttackNode(this, 0);
        phase3ScreamAttackNode = new ScreamAttackNode(this, 0);
        phase3BasicAttackNode = new BasicAttackNode (this, 0);
        



        //시퀀스, 셀렉터 노드들
        dieSequence = new Sequence();

        phase1 = new Parallel();
        phase1HpConditionDecorator = new CheckHp(this,100,Phase1HpCondition1);
        phase1ActionSelector = new Selector();
        phase1MoveSequence = new Sequence();
        phase1AttackArrSequence = new RandomSequence("phase1 RandomSequence");

        Phase2 = new Parallel();
        Phase2actionSelector = new Selector("phase2ActionSelector");
        phase2HpConditionDecorator = new CheckHp(this, Phase1HpCondition1,Phase2HpCondition1);
        Phase2AttackSequence = new RandomSequence("phase2 RandomSequence");
        Phase2MoveSequence = new Sequence();
        EntryPhase2Sequence= new Sequence();


        Phase3 = new Parallel();
        phase3HpConditionDecorator = new CheckHp(this, Phase3HpCondition1,0);
    }

    void Start()
    {
        //죽는 상태 트리
        dieSequence.AddNode(DieHpConditionDecorator);
        dieSequence.AddNode(DieNode);
        

        //페이지1 트리
        phase1AttackArrSequence.AddNode(phase1FlameAttackNode);
        phase1AttackArrSequence.AddNode(phase1ScreamAttackNode);
        phase1.AddNode(phase1HpConditionDecorator);
        phase1.AddNode(phase1AttackArrSequence);
        

        //페이지2 트리
        Phase2AttackSequence.AddNode(phase2BasicAttackNode);
        Phase2AttackSequence.AddNode(phase2FlameAttackNode);
        Phase2AttackSequence.AddNode(phase2ScreamAttackNode);
        Phase2MoveSequence.AddNode(ChecktoTargetDistance);
        Phase2MoveSequence.AddNode(MoveNode);
        EntryPhase2Sequence.AddNode(checkIncomingPhase2);
        EntryPhase2Sequence.AddNode(phase2EntryNode);
        Phase2actionSelector.AddNode(EntryPhase2Sequence);
        Phase2actionSelector.AddNode(Phase2MoveSequence);
        Phase2actionSelector.AddNode(Phase2AttackSequence);
        Phase2.AddNode(phase2HpConditionDecorator);
        Phase2.AddNode(Phase2actionSelector);



        root.AddNode(dieSequence);
        root.AddNode(phase1);
        root.AddNode(Phase2);

        Bt = new BehaviorTreeRunner(root);
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
    public float LookTarget(Transform transform, Vector3 target)
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
    public void HitDamage(int damage)
    {
        hp -= damage;
    }
    public void SpawnEnemy(int index)
    {
        float xPosition = Random.Range(spawnRange[0].position.x, spawnRange[1].position.x);
        Vector3 spawnPoint = new Vector3(xPosition, spawnRange[0].position.y, 0);
        GameObject GMenemy =  Instantiate(ObjectPrefabs[index], spawnPoint, transform.rotation);
        Ray ray = new Ray(spawnPoint, Vector3.down);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity,LayerMask.GetMask("Ground"));
        if(hit.collider!=null)
        {
            GameObject dangerZoneGM = Instantiate(dangerZonePrefab);
            dangerZoneGM.transform.position = hit.point;
        }
    }

    public void SpawnFlame()
    {
        Debug.Log("spawnFlame");
        GameObject flameGM = Instantiate(flamePrefab, FlamePosition.position,transform.rotation);
        flameGM.GetComponent<FlameBehavior>().SetTarget(Target);
        flameGM.GetComponent<FlameBehavior>().ActiveSkill();
    }
}

