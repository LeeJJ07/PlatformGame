using System.Collections.Generic;
using UnityEngine;
//살려줘...

public class BossAI : MonoBehaviour
{
    
    [Header("Common Component")]
    [SerializeField] List<GameObject> objectPrefabs = new List<GameObject>();
    [SerializeField] GameObject warningPrefab;
    [SerializeField] GameObject flamePrefab;
    [SerializeField] Transform[] spawnRange;
    [SerializeField] Transform target;
    [SerializeField] Transform flamePosition;
    [SerializeField] Animator aniController;
    [SerializeField] float hp = 100;
    [SerializeField] float moveSpeed = 0;

    [Header("Phase1")]
    [SerializeField] float Phase1HpCondition;

    [Header("PHASE1 Skill Info")]
    [SerializeField] FlameAttackScriptableObject Phase1flameAttackInfo;
    [SerializeField] ScreamAttackScriptableObject Phase1screamAttackInfo;

    [Header("Phase2")]
    [SerializeField] float Phase2HpCondition;

    [Header("PHASE2 Skill Info")]
    [SerializeField] FlameAttackScriptableObject Phase2flameAttackInfo;
    [SerializeField] ScreamAttackScriptableObject Phase2screamAttackInfo;
    [SerializeField] BasicAttackScriptableObject Phase2basicAttackInfo;

    [Header("Phase3")]
    [SerializeField] GameObject angryLight;
    [SerializeField] float Phase3HpCondition;
    
    [Header("PHASE3 Skill Info")]
    [SerializeField] FlameAttackScriptableObject Phase3flameAttackInfo;
    [SerializeField] ScreamAttackScriptableObject Phase3screamAttackInfo;
    [SerializeField] BasicAttackScriptableObject Phase3basicAttackInfo;
    [SerializeField] RushAttackScriptableObject Phase3RushAttackInfo;
    //공격범위
    [SerializeField] float basicAttackDistance;
    [SerializeField] float screamAttackDistance;
    [SerializeField] float flameAttackDistance;

    PullingDirector pullingDirector;

    
    bool isDie = false;

    BehaviorTreeRunner Bt;

    #region 노드 변수들(행동노드, 조건노드)
    ////////////////////
    INode IntroNode;
    INode DieNode;
    INode DieHpConditionDecorator;
    INode moveNode;
    INode targetinFlameAttackRange;
    INode targetinScreamAttackRange;
    INode targetinBasicAttackRange;
    /// ///////////////////////

    INode phase1HpConditionDecorator;
    INode phase1FlameAttackNode;
    INode phase1ScreamAttackNode;
    INode checkIncomingPhase1;
    INode phase1EntryLandNode;
    INode phase1EntryScreamNode;
    INode phase1FlameAttackDelay;
    INode phase1ScreamAttackDelay;
    
    INode phase2HpConditionDecorator;
    INode phase2FlameAttackNode;
    INode phase2ScreamAttackNode;
    INode phase2BasicAttackNode;
    INode phase2EntryNode;
    INode checkIncomingPhase2;
    INode phase2BasicAttackDelay;
    INode phase2FlameAttackDelay;
    INode phase2ScreamAttackDelay;

    INode phase3HpConditionDecorator;
    INode phase3FlameAttackNode;
    INode phase3ScreamAttackNode;
    INode phase3BasicAttackNode;
    INode phase3RushAttackNode;
    INode phase3EntryNode;
    INode checkIncomingPhase3;
    INode phase3BasicAttackDelay;
    INode phase3FlameAttackDelay;
    INode phase3ScreamAttackDelay;
    INode phase3RushAttackDelay;
    INode phase3WarningRushAttack;
    #endregion

    #region Sequence, Selector, Parallel, RamdomSelector 변수들
    Selector root;
    Sequence dieSequence;

    Parallel phase1;
    Sequence entryPhase1Sequence;
    Sequence entryPhase1ActionSequence;
    Sequence phase1FlameAttackSequence;
    Sequence phase1ScreamAttackSequence;
    Selector phase1ActionSelector;
    RandomSelector phase1AttackRandomSelector;

    Parallel phase2;//보스체력>30%    Sequence EntryPhase2Sequence;
    Parallel phase2basicAttackmoveParallel;
    Parallel phase2screamAttackmoveParallel;
    Parallel phase2flameAttackmoveParallel;
    Sequence entryPhase2Sequence;
    RandomSelector phase2AttackRandomSelector;
    Selector phase2ActionSelector;
    Selector phase2BasicAttackSelector;
    Selector phase2FlameAttackSelector;
    Selector phase2ScreamAttackSelector;
    Sequence phase2BasicAttackSequence;
    Sequence phase2FlameAttackSequence;
    Sequence phase2ScreamAttackSequence;


    Parallel phase3;//보스체력<=30%
    Parallel phase3basicAttackmoveParallel;
    Parallel phase3screamAttackmoveParallel;
    Parallel phase3flameAttackmoveParallel;
    Selector phase3ActionSelector;
    Sequence entryPhase3Sequence;
    RandomSelector phase3AttackRandomSelector;
    Selector phase3BasicAttackSelector;
    Selector phase3FlameAttackSelector;
    Selector phase3ScreamAttackSelector;
    Sequence phase3BasicAttackSequence;
    Sequence phase3FlameAttackSequence;
    Sequence phase3ScreamAttackSequence;
    Sequence phase3RushAttackSequence;
    #endregion




    private void Awake()
    {
        root = new Selector();
        //조건 노드들
        
        DieHpConditionDecorator = new CheckHp(GetHp, 0,-100);
        phase1HpConditionDecorator = new CheckHp(GetHp, Phase1HpCondition1, Phase2HpCondition1);
        phase2HpConditionDecorator = new CheckHp(GetHp, Phase2HpCondition1, Phase3HpCondition1);
        phase3HpConditionDecorator = new CheckHp(GetHp, Phase3HpCondition1, 0);
        checkIncomingPhase1 = new CheckIncomingPhase();
        checkIncomingPhase2 = new CheckIncomingPhase();
        checkIncomingPhase3 = new CheckIncomingPhase();

        targetinFlameAttackRange = new ChecktoTargetDistance(transform, target, flameAttackDistance);
        targetinScreamAttackRange = new ChecktoTargetDistance(transform, target, screamAttackDistance);
        targetinBasicAttackRange = new ChecktoTargetDistance(transform, target, basicAttackDistance);
        phase1FlameAttackDelay = new NodeDelay(Phase1flameAttackInfo.subSequenceDelay,aniController);
        phase1ScreamAttackDelay = new NodeDelay(Phase1screamAttackInfo.subSequenceDelay,aniController);

        phase2BasicAttackDelay = new NodeDelay(Phase2basicAttackInfo.subSequenceDelay,aniController);
        phase2FlameAttackDelay = new NodeDelay(Phase2flameAttackInfo.subSequenceDelay,aniController);
        phase2ScreamAttackDelay = new NodeDelay(Phase2screamAttackInfo.subSequenceDelay,aniController);

        phase3BasicAttackDelay = new NodeDelay(Phase3basicAttackInfo.subSequenceDelay,aniController);
        phase3FlameAttackDelay = new NodeDelay(Phase3flameAttackInfo.subSequenceDelay,aniController);
        phase3ScreamAttackDelay = new NodeDelay(Phase3screamAttackInfo.subSequenceDelay,aniController);
        phase3RushAttackDelay = new NodeDelay(Phase3RushAttackInfo.subSequenceDelay,aniController);

        //행동 노드들
        moveNode = new MoveNode(transform, target, aniController, moveSpeed);
        DieNode = new DieNode(DeActivateObj, transform, target, aniController);
        phase1FlameAttackNode = new FlameAttackNode(Phase1flameAttackInfo,aniController,transform,target);
        phase1ScreamAttackNode = new ScreamAttackNode(Phase1screamAttackInfo,aniController, RandomSpawnObjects, transform,target);
        phase1EntryLandNode = new EntryPhase1LandNode(transform, target, aniController);
        phase1EntryScreamNode = new EntryPhase1ScreamNode(transform, target, aniController);

        phase2EntryNode = new EntryPhase2Node(transform, target, aniController);
        phase2FlameAttackNode = new FlameAttackNode(Phase2flameAttackInfo, aniController, transform, target);
        phase2ScreamAttackNode = new ScreamAttackNode(Phase2screamAttackInfo, aniController, RandomSpawnObjects, transform, target);
        phase2BasicAttackNode = new BasicAttackNode(Phase2basicAttackInfo, aniController, transform, target);
      

        phase3EntryNode = new EntryPhase3Node(angryLight, transform, target, aniController);
        phase3FlameAttackNode = new FlameAttackNode(Phase3flameAttackInfo,aniController,transform,target);
        phase3ScreamAttackNode = new ScreamAttackNode(Phase3screamAttackInfo, aniController, RandomSpawnObjects, transform, target);
        phase3BasicAttackNode = new BasicAttackNode(Phase3basicAttackInfo, aniController, transform, target);
        phase3RushAttackNode = new RushAttackNode(Phase3RushAttackInfo, RandomSpawnObjects, aniController, transform, target);
        phase3WarningRushAttack = new WarningRushAttack(SpawnObject, angryLight, warningPrefab, transform.position, Phase3RushAttackInfo.warningDelay);


        //시퀀스, 셀렉터 노드들
        dieSequence = new Sequence();

        phase1 = new Parallel();
        phase1ActionSelector = new Selector();
        phase1AttackRandomSelector = new RandomSelector("phase1 RandomSequence");
        entryPhase1ActionSequence = new Sequence();
        entryPhase1Sequence = new Sequence();
        phase1FlameAttackSequence = new Sequence();
        phase1ScreamAttackSequence = new Sequence();

        phase2 = new Parallel();
        phase2ActionSelector = new Selector("phase2ActionSelector");
        phase2FlameAttackSelector = new Selector();
        phase2BasicAttackSelector = new Selector();
        phase2basicAttackmoveParallel = new Parallel ();
        phase2screamAttackmoveParallel = new Parallel();
        phase2flameAttackmoveParallel = new Parallel();
        phase2ScreamAttackSelector = new Selector();
        phase2BasicAttackSequence = new Sequence();
        phase2ScreamAttackSequence = new Sequence();
        phase2FlameAttackSequence = new Sequence();

        phase2AttackRandomSelector = new RandomSelector("phase2 RandomSequence");
        
        entryPhase2Sequence= new Sequence();


        phase3 = new Parallel();
        phase3ActionSelector = new Selector();
        entryPhase3Sequence = new Sequence();
        phase3AttackRandomSelector = new RandomSelector();
        phase3BasicAttackSelector = new Selector();
        phase3FlameAttackSelector = new Selector();
        phase3ScreamAttackSelector = new Selector();
        phase3basicAttackmoveParallel = new Parallel();
        phase3screamAttackmoveParallel = new Parallel();
        phase3flameAttackmoveParallel = new Parallel();
        phase3BasicAttackSequence = new Sequence();
        phase3FlameAttackSequence = new Sequence();
        phase3ScreamAttackSequence = new Sequence();
        phase3RushAttackSequence = new Sequence();
        pullingDirector = GameObject.FindWithTag("Director").GetComponent<PullingDirector>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        //죽는 상태 트리
        dieSequence.AddNode(DieHpConditionDecorator);
        dieSequence.AddNode(DieNode);


        //페이지1 트리
        phase1FlameAttackSequence.AddNode(phase1FlameAttackNode);
        phase1FlameAttackSequence.AddNode(phase1FlameAttackDelay);
        phase1ScreamAttackSequence.AddNode(phase1ScreamAttackNode);
        phase1ScreamAttackSequence.AddNode(phase1ScreamAttackDelay);

        entryPhase1ActionSequence.AddNode(phase1EntryLandNode);
        entryPhase1ActionSequence.AddNode(phase1EntryScreamNode);
        entryPhase1Sequence.AddNode(checkIncomingPhase1);
        entryPhase1Sequence.AddNode(entryPhase1ActionSequence);
        phase1AttackRandomSelector.AddNode(phase1FlameAttackSequence);
        phase1AttackRandomSelector.AddNode(phase1ScreamAttackSequence);
        phase1ActionSelector.AddNode(entryPhase1Sequence);
        phase1ActionSelector.AddNode(phase1AttackRandomSelector);
        phase1.AddNode(phase1HpConditionDecorator);
        phase1.AddNode(phase1ActionSelector);


        //페이지2 트리
        phase2ScreamAttackSequence.AddNode(phase2ScreamAttackNode);
        phase2ScreamAttackSequence.AddNode(phase2ScreamAttackDelay);
        phase2BasicAttackSequence.AddNode(phase2BasicAttackNode);
        phase2BasicAttackSequence.AddNode(phase2BasicAttackDelay);
        phase2FlameAttackSequence.AddNode(phase2FlameAttackNode);
        phase2FlameAttackSequence.AddNode(phase2FlameAttackDelay);

        phase2flameAttackmoveParallel.AddNode(targetinFlameAttackRange);
        phase2flameAttackmoveParallel.AddNode(moveNode);
        phase2FlameAttackSelector.AddNode(phase2flameAttackmoveParallel);
        phase2FlameAttackSelector.AddNode(phase2FlameAttackSequence);

        phase2screamAttackmoveParallel.AddNode(targetinScreamAttackRange);
        phase2screamAttackmoveParallel.AddNode(moveNode);
        phase2ScreamAttackSelector.AddNode(phase2screamAttackmoveParallel);
        phase2ScreamAttackSelector.AddNode(phase2ScreamAttackSequence);

        phase2basicAttackmoveParallel.AddNode(targetinBasicAttackRange);
        phase2basicAttackmoveParallel.AddNode(moveNode);
        phase2BasicAttackSelector.AddNode(phase2basicAttackmoveParallel);
        phase2BasicAttackSelector.AddNode(phase2BasicAttackSequence);

        phase2AttackRandomSelector.AddNode(phase2BasicAttackSelector);
        phase2AttackRandomSelector.AddNode(phase2ScreamAttackSelector);
        phase2AttackRandomSelector.AddNode(phase2FlameAttackSelector);
        entryPhase2Sequence.AddNode(checkIncomingPhase2);
        entryPhase2Sequence.AddNode(phase2EntryNode);
        phase2ActionSelector.AddNode(entryPhase2Sequence);
        phase2ActionSelector.AddNode(phase2AttackRandomSelector);
        phase2.AddNode(phase2HpConditionDecorator);
        phase2.AddNode(phase2ActionSelector);

        //페이지3 트리
        phase3BasicAttackSequence.AddNode(phase3BasicAttackNode);
        phase3BasicAttackSequence.AddNode(phase3BasicAttackDelay);
        phase3ScreamAttackSequence.AddNode(phase3ScreamAttackNode);
        phase3ScreamAttackSequence.AddNode(phase3ScreamAttackDelay);
        phase3FlameAttackSequence.AddNode(phase3FlameAttackNode);
        phase3FlameAttackSequence.AddNode(phase3FlameAttackDelay);
        phase3RushAttackSequence.AddNode(phase3WarningRushAttack);
        phase3RushAttackSequence.AddNode(phase3RushAttackNode);
        phase3RushAttackSequence.AddNode(phase3RushAttackDelay);


        phase3basicAttackmoveParallel.AddNode(targetinBasicAttackRange);
        phase3basicAttackmoveParallel.AddNode(moveNode);
        phase3BasicAttackSelector.AddNode(phase3basicAttackmoveParallel);
        phase3BasicAttackSelector.AddNode(phase3BasicAttackSequence);
        
        phase3flameAttackmoveParallel.AddNode(targetinFlameAttackRange);
        phase3flameAttackmoveParallel.AddNode(moveNode);
        phase3FlameAttackSelector.AddNode(phase3flameAttackmoveParallel);
        phase3FlameAttackSelector.AddNode(phase3FlameAttackSequence);

        phase3screamAttackmoveParallel.AddNode(targetinScreamAttackRange);
        phase3screamAttackmoveParallel.AddNode(moveNode);
        phase3ScreamAttackSelector.AddNode(phase3screamAttackmoveParallel);
        phase3ScreamAttackSelector.AddNode(phase3ScreamAttackSequence);
        
        entryPhase3Sequence.AddNode(checkIncomingPhase3);
        entryPhase3Sequence.AddNode(phase3EntryNode);
        //phase3AttackRandomSelector.AddNode(phase3BasicAttackSelector);
        //phase3AttackRandomSelector.AddNode(phase3FlameAttackSelector);
        //phase3AttackRandomSelector.AddNode(phase3ScreamAttackSelector);
        phase3AttackRandomSelector.AddNode(phase3RushAttackSequence);

        phase3ActionSelector.AddNode(entryPhase3Sequence);
        phase3ActionSelector.AddNode(phase3AttackRandomSelector);
        phase3.AddNode(phase3HpConditionDecorator);
        phase3.AddNode(phase3ActionSelector);


        root.AddNode(dieSequence);
        root.AddNode(phase1);
        root.AddNode(phase2);
        root.AddNode(phase3);

        angryLight.SetActive(false);
        Bt = new BehaviorTreeRunner(root);
    }
    void Update()
    {
        if (isDie) return;
        Bt.Operator();
    }

    public float GetHp()
    {
        return hp;
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
    

    //정해진 범위 안에 랜덤한 적스폰
    public void RandomSpawnObjects(GameObject[] objs,int spawnCount)
    {
        for(int i=0; i<spawnCount; i++)
        {
            float randomPosiX = Random.Range(spawnRange[0].position.x, spawnRange[1].position.x);

            Vector3 pos = new Vector3(randomPosiX, spawnRange[0].position.y, 0);
            int randomIndex = Random.Range(0, objs.Length);
            pullingDirector.SpawnObject(objs[randomIndex].tag, pos);
        }
    }

    //지정한 위치로 적 종류별로 스폰
    public void SpawnObjects(GameObject[] objs,Vector3 posi)
    {
        foreach(GameObject obj in objs)
        {
            pullingDirector.SpawnObject(obj.tag, posi);
        }
    }

    //지정한 위치와 갯수만큼 적 스폰
    public GameObject[] SpawnObject(GameObject obj, Vector3 posi ,int count)
    {
        GameObject[] spawnObjs = new GameObject[count];
        for(int i=0; i<count; i++)
        {
            spawnObjs[i] = pullingDirector.SpawnObject(obj.tag, posi);
        }
        return spawnObjs;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5);

    }
    #region 변수 캡슐화(region)
    public Transform FlamePosition { get => flamePosition; }
    public Animator EnemyAnimation { get => aniController; }
    public float BasicAttackRange { get => basicAttackDistance; }
    public float ScreamSkillRange { get => screamAttackDistance; }
    public float FlameSkillRange { get => flameAttackDistance; }
    public float Phase1HpCondition1 { get => Phase1HpCondition; }
    public float Phase2HpCondition1 { get => Phase2HpCondition; }
    public float Phase3HpCondition1 { get => Phase3HpCondition; }
    public List<GameObject> ObjectPrefabs { get => objectPrefabs; }
    public GameObject FlamePrefab { get => flamePrefab; }
    public GameObject AngryLight { get => angryLight; }

    #endregion
}

