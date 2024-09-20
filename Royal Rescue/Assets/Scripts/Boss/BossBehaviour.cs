using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
//�����...

public class BossBehaviour : MonoBehaviour,ITag
{
    [Header("보스 활성화")]
    [SerializeField] bool isActivate = false;

    [Header("보스 필수 컴포넌트들")]
    [SerializeField] string bossName;
    [SerializeField] string detailTag;
    [SerializeField] float hp = 100;
    [SerializeField] float moveSpeed = 0;
    [SerializeField] GameObject warningPrefab;
    [SerializeField] GameObject shockWave;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform[] wallTransforms;
    [SerializeField] Transform[] spawnRange;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform flamePosition;
    [SerializeField] Collider[] bossColliders;
    [SerializeField] Animator aniController;

    [Header("보스 인트로 후 딜레이")]
    [SerializeField] float endEntrystateNdelay;

    [Header("페이즈1 HP조건")]
    [SerializeField] float Phase1HpCondition;

    [Header("페이즈1 스킬 정보들")]
    [SerializeField] FlameAttackScriptableObject Phase1flameAttackInfo;
    [SerializeField] ScreamAttackScriptableObject Phase1screamAttackInfo;

    [Header("페이즈2")]
    [SerializeField] float Phase2HpCondition;

    [Header("페이즈2 스킬 정보들")]
    [SerializeField] FlameAttackScriptableObject Phase2flameAttackInfo;
    [SerializeField] BreathAttackScriptableObject Phase2breathAttackInfo;
    [SerializeField] ScreamAttackScriptableObject Phase2screamAttackInfo;
    [SerializeField] BasicAttackScriptableObject Phase2basicAttackInfo;

    [Header("페이즈3")]
    [SerializeField] GameObject flamePrefabsObject;
    [SerializeField] GameObject angryLight;
    [SerializeField] float Phase3HpCondition;
    
    [Header("페이즈3 스킬 정보들")]
    [SerializeField] FlameAttackScriptableObject Phase3flameAttackInfo;
    [SerializeField] ScreamAttackScriptableObject Phase3screamAttackInfo;
    [SerializeField] BasicAttackScriptableObject Phase3basicAttackInfo;
    [SerializeField] BreathAttackScriptableObject Phase3breathAttackInfo;
    [SerializeField] RushAttackScriptableObject Phase3RushAttackInfo;
    
    
    //���ݹ���
    [Header("스킬 공격가능 거리")]
    [SerializeField] float basicAttackDistance;
    [SerializeField] float screamAttackDistance;
    [SerializeField] float flameAttackDistance;
    [SerializeField] float breathAttackDistance;


    bool isDie = false;
    bool isHit = true;
    bool isActiveGetHitAni = false;
    bool isDelayTime = false;
    PullingDirector pullingDirector;
    SoundManager soundManager;
    BehaviorTreeRunner Bt;
    PlayerControlManagerFix playerControl;
    BossHpBarUI hpbarUi;

    #region 보스 행동트리 노드들
    ////////////////////
    INode IntroNode;
    INode DieNode;
    INode DieHpConditionDecorator;
    INode moveNode;
    INode targetinFlameAttackRange;
    INode targetinScreamAttackRange;
    INode targetinBasicAttackRange;
    INode targetinBreathAttackRange;
    /// ///////////////////////
    
    INode phase1CheckSpawnCount;
    INode phase1HpConditionDecorator;
    INode phase1FlameAttackNode;
    INode phase1ScreamAttackNode;
    INode checkIncomingPhase1;
    INode phase1EntryLandNode;
    INode phase1EntryScreamNode;
    INode phase1EntryafterDelay;
    INode phase1FlameAttackDelay;
    INode phase1ScreamAttackDelay;

    INode phase2CheckSpawnCount;
    INode phase2HpConditionDecorator;
    INode phase2FlameAttackNode;
    INode phase2ScreamAttackNode;
    INode phase2BasicAttackNode;
    INode phase2BreathAttackNode;
    INode phase2EntryNode;
    INode checkIncomingPhase2;
    INode phase2BasicAttackDelay;
    INode phase2FlameAttackDelay;
    INode phase2ScreamAttackDelay;
    INode phase2BreathAttackDelay;

    INode phase3CheckSpawnCount;
    INode phase3HpConditionDecorator;
    INode phase3FlameAttackNode;
    INode phase3ScreamAttackNode;
    INode phase3BasicAttackNode;
    INode phase3RushAttackNode;
    INode phase3BreathAttackNode;
    INode phase3EntryNode;
    INode checkIncomingPhase3;
    INode phase3BasicAttackDelay;
    INode phase3FlameAttackDelay;
    INode phase3ScreamAttackDelay;
    INode phase3BreathAttackDelay;
    INode phase3RushAttackDelay;
    INode phase3WarningRushAttackNode;
    #endregion

    #region Sequence, Selector, Parallel, RamdomSelector 
    Selector root;
    Sequence dieSequence;

    Parallel phase1;
    Sequence entryPhase1Sequence;
    Sequence entryPhase1ActionSequence;
    Sequence phase1FlameAttackSequence;
    Sequence phase1ScreamAttackSequence;
    Selector phase1ActionSelector;
    RandomSelector phase1AttackRandomSelector;

    Parallel phase2;//����ü��>30%    Sequence EntryPhase2Sequence;
    Parallel phase2basicAttackmoveParallel;
    Parallel phase2screamAttackmoveParallel;
    Parallel phase2breathAttackmoveParallel;
    Parallel phase2flameAttackmoveParallel;
    Sequence entryPhase2Sequence;
    RandomSelector phase2AttackRandomSelector;
    Selector phase2ActionSelector;
    Selector phase2BasicAttackSelector;
    Selector phase2BreathAttackSelector;
    Selector phase2FlameAttackSelector;
    Selector phase2ScreamAttackSelector;
    Sequence phase2BasicAttackSequence;
    Sequence phase2BreathAttackSequence;
    Sequence phase2FlameAttackSequence;
    Sequence phase2ScreamAttackSequence;


    Parallel phase3;//����ü��<=30%
    Parallel phase3basicAttackmoveParallel;
    Parallel phase3screamAttackmoveParallel;
    Parallel phase3flameAttackmoveParallel;
    Parallel phase3breathAttackmoveParallel;
    Selector phase3ActionSelector;
    Sequence entryPhase3Sequence;
    RandomSelector phase3AttackRandomSelector;
    Selector phase3BasicAttackSelector;
    Selector phase3FlameAttackSelector;
    Selector phase3ScreamAttackSelector;
    Selector phase3BreathAttackSelector;
    Sequence phase3BasicAttackSequence;
    Sequence phase3FlameAttackSequence;
    Sequence phase3ScreamAttackSequence;
    Sequence phase3RushAttackSequence;
    Sequence phase3BreathAttackSequence;
    #endregion


    private void Awake()
    {
        pullingDirector = GameObject.FindWithTag("Director").GetComponent<PullingDirector>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        playerTransform = GameObject.FindWithTag("Player").transform;
       
        root = new Selector();
        //조건 노드들
        
        DieHpConditionDecorator = new CheckHp(GetHp, 0,-100);
        phase1HpConditionDecorator = new CheckHp(GetHp, Phase1HpCondition, Phase2HpCondition);
        phase2HpConditionDecorator = new CheckHp(GetHp, Phase2HpCondition, Phase3HpCondition);
        phase3HpConditionDecorator = new CheckHp(GetHp, Phase3HpCondition, 0);
        checkIncomingPhase1 = new CheckIncomingPhase();
        checkIncomingPhase2 = new CheckIncomingPhase();
        checkIncomingPhase3 = new CheckIncomingPhase();

        targetinFlameAttackRange = new ChecktoTargetDistance(transform, playerTransform, flameAttackDistance);
        targetinScreamAttackRange = new ChecktoTargetDistance(transform, playerTransform, screamAttackDistance);
        targetinBasicAttackRange = new ChecktoTargetDistance(transform, playerTransform, basicAttackDistance);
        targetinBreathAttackRange = new ChecktoTargetDistance(transform, playerTransform, breathAttackDistance);

        phase1CheckSpawnCount = new CheckSpawnMonsterCount(GetSpawnMonsterCount, Phase1screamAttackInfo.maxSpawnCount, Phase1screamAttackInfo.objSpawnCount);
        phase2CheckSpawnCount = new CheckSpawnMonsterCount(GetSpawnMonsterCount, Phase2screamAttackInfo.maxSpawnCount, Phase2screamAttackInfo.objSpawnCount);
        phase3CheckSpawnCount = new CheckSpawnMonsterCount(GetSpawnMonsterCount, Phase3screamAttackInfo.maxSpawnCount, Phase3screamAttackInfo.objSpawnCount);

        phase1FlameAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase1flameAttackInfo.subSequenceDelay,aniController);
        phase1ScreamAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase1screamAttackInfo.subSequenceDelay,aniController);
        phase1EntryafterDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, endEntrystateNdelay, aniController);

        phase2BasicAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase2basicAttackInfo.subSequenceDelay,aniController);
        phase2FlameAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase2flameAttackInfo.subSequenceDelay,aniController);
        phase2ScreamAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase2screamAttackInfo.subSequenceDelay,aniController);
        phase2BreathAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase2breathAttackInfo.subSequenceDelay, aniController);

        phase3BasicAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase3basicAttackInfo.subSequenceDelay,aniController);
        phase3FlameAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase3flameAttackInfo.subSequenceDelay,aniController);
        phase3ScreamAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase3screamAttackInfo.subSequenceDelay,aniController);
        phase3BreathAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase3breathAttackInfo.subSequenceDelay, aniController);
        phase3RushAttackDelay = new NodeDelay(GetIsActiveGetHitAni, SetisDelayTime, Phase3RushAttackInfo.subSequenceDelay,aniController);

        //스킬 노드들
        moveNode = new MoveNode(transform, playerTransform, aniController, moveSpeed);
        DieNode = new DieNode(DeActivateSpawnObjs, SoundEffect, BossDie, transform, playerTransform, aniController);
        phase1FlameAttackNode = new FlameAttackNode(Phase1flameAttackInfo, SpawnObjectWithITag, flamePosition, aniController,transform,playerTransform, SoundEffect);
        phase1ScreamAttackNode = new ScreamAttackNode(Phase1screamAttackInfo, RandomSpawnObjectsWithITag, SpawnObjectWithITag, aniController, flamePosition);
        phase1EntryLandNode = new EntryPhase1LandNode(transform, playerTransform, aniController);
        phase1EntryScreamNode = new EntryPhase1ScreamNode(transform, playerTransform, aniController,shockWave,flamePosition, SpawnObjectWithITag, ActivateHpUi);

        phase2EntryNode = new EntryPhase2Node(transform, playerTransform, flamePosition,shockWave, aniController, SpawnObjectWithITag,DeActivateParticles, SetisDelayTime);
        phase2FlameAttackNode = new FlameAttackNode(Phase2flameAttackInfo, SpawnObjectWithITag, flamePosition, aniController, transform, playerTransform, SoundEffect);
        phase2ScreamAttackNode = new ScreamAttackNode(Phase2screamAttackInfo, RandomSpawnObjectsWithITag, SpawnObjectWithITag, aniController, flamePosition);
        phase2BasicAttackNode = new BasicAttackNode(Phase2basicAttackInfo, aniController,flamePosition, transform, playerTransform);
        phase2BreathAttackNode = new BreathAttackNode(SpawnObjectWithITag, Phase2breathAttackInfo, aniController, flamePosition, transform, playerTransform,SoundEffect);

        phase3EntryNode = new EntryPhase3Node(angryLight, transform, playerTransform,flamePosition, shockWave, flamePrefabsObject, aniController,SpawnObjectWithITag, DeActivateParticles, SetisDelayTime);
        phase3FlameAttackNode = new FlameAttackNode(Phase3flameAttackInfo, SpawnObjectWithITag, flamePosition, aniController,transform,playerTransform, SoundEffect);
        phase3ScreamAttackNode = new ScreamAttackNode(Phase3screamAttackInfo, RandomSpawnObjectsWithITag, SpawnObjectWithITag, aniController, flamePosition);
        phase3BasicAttackNode = new BasicAttackNode(Phase3basicAttackInfo, aniController, flamePosition, transform, playerTransform);
        phase3RushAttackNode = new RushAttackNode(Phase3RushAttackInfo, SoundEffect, bossColliders,RandomSpawnObjectsWithITag, aniController, transform, playerTransform);
        phase3WarningRushAttackNode = new WarningRushAttack(SpawnObjects, angryLight, warningPrefab,transform, Phase3RushAttackInfo.warningDelay);
        phase3BreathAttackNode = new BreathAttackNode(SpawnObjectWithITag, Phase3breathAttackInfo, aniController, flamePosition, transform, playerTransform, SoundEffect);


        //Sequence, Selector, Parallel...
        dieSequence = new Sequence();

        phase1 = new Parallel();
        phase1ActionSelector = new Selector();
        phase1AttackRandomSelector = new RandomSelector();
        entryPhase1ActionSequence = new Sequence();
        entryPhase1Sequence = new Sequence();
        phase1FlameAttackSequence = new Sequence();
        phase1ScreamAttackSequence = new Sequence();

        phase2 = new Parallel();
        phase2ActionSelector = new Selector();
        phase2FlameAttackSelector = new Selector();
        phase2BasicAttackSelector = new Selector();
        phase2BreathAttackSelector = new Selector();
        phase2basicAttackmoveParallel = new Parallel ();
        phase2screamAttackmoveParallel = new Parallel();
        phase2breathAttackmoveParallel = new Parallel();
        phase2flameAttackmoveParallel = new Parallel();
        phase2ScreamAttackSelector = new Selector();
        phase2BasicAttackSequence = new Sequence();
        phase2BreathAttackSequence = new Sequence();
        phase2ScreamAttackSequence = new Sequence();
        phase2FlameAttackSequence = new Sequence();

        phase2AttackRandomSelector = new RandomSelector();
        
        entryPhase2Sequence= new Sequence();


        phase3 = new Parallel();
        phase3ActionSelector = new Selector();
        entryPhase3Sequence = new Sequence();
        phase3AttackRandomSelector = new RandomSelector();
        phase3BasicAttackSelector = new Selector();
        phase3FlameAttackSelector = new Selector();
        phase3ScreamAttackSelector = new Selector();
        phase3BreathAttackSelector = new Selector();
        phase3basicAttackmoveParallel = new Parallel();
        phase3screamAttackmoveParallel = new Parallel();
        phase3flameAttackmoveParallel = new Parallel();
        phase3breathAttackmoveParallel = new Parallel();
        phase3BasicAttackSequence = new Sequence();
        phase3FlameAttackSequence = new Sequence();
        phase3ScreamAttackSequence = new Sequence();
        phase3RushAttackSequence = new Sequence();
        phase3BreathAttackSequence = new Sequence();
        
    }

    void Start()
    {
        
        //Die노드 트리구성
        dieSequence.AddNode(DieHpConditionDecorator);
        dieSequence.AddNode(DieNode);


        //페이즈1 트리구성
        phase1FlameAttackSequence.AddNode(phase1FlameAttackNode);
        phase1FlameAttackSequence.AddNode(phase1FlameAttackDelay);
        phase1ScreamAttackSequence.AddNode(phase1CheckSpawnCount);
        phase1ScreamAttackSequence.AddNode(phase1ScreamAttackNode);
        phase1ScreamAttackSequence.AddNode(phase1ScreamAttackDelay);

        entryPhase1ActionSequence.AddNode(phase1EntryLandNode);
        entryPhase1ActionSequence.AddNode(phase1EntryScreamNode);
        entryPhase1ActionSequence.AddNode(phase1EntryafterDelay);

        entryPhase1Sequence.AddNode(checkIncomingPhase1);
        entryPhase1Sequence.AddNode(entryPhase1ActionSequence);
        phase1AttackRandomSelector.AddNode(phase1FlameAttackSequence);
        phase1AttackRandomSelector.AddNode(phase1ScreamAttackSequence);
        phase1ActionSelector.AddNode(entryPhase1Sequence);
        phase1ActionSelector.AddNode(phase1AttackRandomSelector);
        phase1.AddNode(phase1HpConditionDecorator);
        phase1.AddNode(phase1ActionSelector);


        //페이즈2 트리구성
        phase2ScreamAttackSequence.AddNode(phase2CheckSpawnCount);
        phase2ScreamAttackSequence.AddNode(phase2ScreamAttackNode);
        phase2ScreamAttackSequence.AddNode(phase2ScreamAttackDelay);
        phase2BreathAttackSequence.AddNode(phase2BreathAttackNode);
        phase2BreathAttackSequence.AddNode(phase2BreathAttackDelay);
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

        phase2breathAttackmoveParallel.AddNode(targetinBreathAttackRange);
        phase2breathAttackmoveParallel.AddNode(moveNode);
        phase2BreathAttackSelector.AddNode(phase2breathAttackmoveParallel);
        phase2BreathAttackSelector.AddNode(phase2BreathAttackSequence);

        phase2AttackRandomSelector.AddNode(phase2BasicAttackSelector);
        phase2AttackRandomSelector.AddNode(phase2ScreamAttackSelector);
        phase2AttackRandomSelector.AddNode(phase2FlameAttackSelector);
        phase2AttackRandomSelector.AddNode(phase2BreathAttackSelector);
        entryPhase2Sequence.AddNode(checkIncomingPhase2);
        entryPhase2Sequence.AddNode(phase2EntryNode);
        phase2ActionSelector.AddNode(entryPhase2Sequence);
        phase2ActionSelector.AddNode(phase2AttackRandomSelector);
        phase2.AddNode(phase2HpConditionDecorator);
        phase2.AddNode(phase2ActionSelector);

        //페이즈3 트리구성
        phase3BasicAttackSequence.AddNode(phase3BasicAttackNode);
        phase3BasicAttackSequence.AddNode(phase3BasicAttackDelay);

        phase3ScreamAttackSequence.AddNode(phase3CheckSpawnCount);
        phase3ScreamAttackSequence.AddNode(phase3ScreamAttackNode);
        phase3ScreamAttackSequence.AddNode(phase3ScreamAttackDelay);

        phase3FlameAttackSequence.AddNode(phase3FlameAttackNode);
        phase3FlameAttackSequence.AddNode(phase3FlameAttackDelay);

        phase3RushAttackSequence.AddNode(phase3WarningRushAttackNode);
        phase3RushAttackSequence.AddNode(phase3RushAttackNode);
        phase3RushAttackSequence.AddNode(phase3RushAttackDelay);

        phase3BreathAttackSequence.AddNode(phase3BreathAttackNode);
        phase3BreathAttackSequence.AddNode(phase3BreathAttackDelay);


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

        phase3breathAttackmoveParallel.AddNode(targetinBreathAttackRange);
        phase3breathAttackmoveParallel.AddNode(moveNode);
        phase3BreathAttackSelector.AddNode(phase3breathAttackmoveParallel);
        phase3BreathAttackSelector.AddNode(phase3BreathAttackSequence);

        entryPhase3Sequence.AddNode(checkIncomingPhase3);
        entryPhase3Sequence.AddNode(phase3EntryNode);

        phase3AttackRandomSelector.AddNode(phase3BasicAttackSelector);
        phase3AttackRandomSelector.AddNode(phase3FlameAttackSelector);
        phase3AttackRandomSelector.AddNode(phase3ScreamAttackSelector);
        phase3AttackRandomSelector.AddNode(phase3BreathAttackSelector);
        phase3AttackRandomSelector.AddNode(phase3RushAttackSequence);

        phase3ActionSelector.AddNode(entryPhase3Sequence);
        phase3ActionSelector.AddNode(phase3AttackRandomSelector);
        phase3.AddNode(phase3HpConditionDecorator);
        phase3.AddNode(phase3ActionSelector);

        //보스 트리구성
        root.AddNode(dieSequence);
        root.AddNode(phase1);
        root.AddNode(phase2);
        root.AddNode(phase3);

        angryLight.SetActive(false);
        flamePrefabsObject.SetActive(false);
        playerControl = GameDirector.instance.PlayerControl;
        Bt = new BehaviorTreeRunner(root);
    }
    void Update()
    {
        if (!isActivate) return;
        Bt.Operator();
    }

    private void ActivateHpUi()
    {
        GameObject[] uiObjs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject obj in uiObjs)
        {
            if (obj.GetComponent<ITag>().CompareToTag("BossHpUI"))
                hpbarUi = obj.GetComponent<BossHpBarUI>();
        }

        float[] hpcolorChangeNum = { Phase1HpCondition, Phase2HpCondition, Phase3HpCondition };
        if (hpbarUi)
        {
            hpbarUi.Init((int)hp, hpcolorChangeNum, bossName);
            hpbarUi.ActivateUI();
        }
    }
    private void SoundEffect(string name, bool isLoop)
    {
        soundManager.PlaySound(name, isLoop);
    }

    private bool GetIsActiveGetHitAni()
    {
        return isActiveGetHitAni;
    }
    
    public void SetisDelayTime(bool value)
    {
        isDelayTime = value;
    }

    //보스 활성화 함수
    public void ActivateBoss()
    {
        isActivate = true;
    }


    private float GetHp()
    {
        return hp;
    }

    private void BossDie()
    {
        isActivate = false;
        gameObject.SetActive(false);
        if (hpbarUi)
            hpbarUi.DeActivateUI();
    }

    //스폰한 모든 몬스터 비활성화
    private void DeActivateSpawnObjs()
    {
        pullingDirector.DeActivateSpawnObjects();
        isDie = true;
    }

    //스폰한 모든 파티클들 비활성화
    private void DeActivateParticles()
    {
        pullingDirector.DeActivateObjectsWithTag("Particle");
    }

    private int GetSpawnMonsterCount()
    {
        int count = pullingDirector.GetSpawnCountWithTag("Monster");
        Debug.Log($"Monster count: {count}");
        return count;
    }

    //ITag를 사용한 오브젝트 스폰
    private void RandomSpawnObjectsWithITag(GameObject[] objs, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float randomPosiX = Random.Range(spawnRange[0].position.x, spawnRange[1].position.x);

            Vector3 posi = new Vector3(randomPosiX, spawnRange[0].position.y, spawnRange[0].position.z);
            int randomIndex = Random.Range(0, objs.Length - 1);
            pullingDirector.SpawnObjectwithITag(objs[randomIndex].tag, objs[randomIndex].GetComponent<ITag>(), posi);
        }
        
    }

    //ITag를 사용하여 지정한 위치에 오브젝트 스폰
    private GameObject SpawnObjectWithITag(GameObject obj, Vector3 posi)
    {
         return pullingDirector.SpawnObjectwithITag(obj.tag, obj.GetComponent<ITag>(), posi);
    }

    //지정한 숫자만큼 오브젝트 스폰
    private GameObject[] SpawnObjects(GameObject obj, Vector3 posi ,int count)
    {
        GameObject[] spawnObjs = new GameObject[count];
        for(int i=0; i<count; i++)
        {
            spawnObjs[i] = pullingDirector.SpawnObject(obj.tag, posi);
        }
        return spawnObjs;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Phase3RushAttackInfo.hitRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(flamePosition.position, new Vector3(5, 5, 1));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDie) return;
        if (other.GetComponent<Collider>().CompareTag("Weapon")
           || other.GetComponent<Collider>().CompareTag("Bomb")
           || other.GetComponent<Collider>().CompareTag("SlashAttack"))
        {
            StartCoroutine(OnDamage(other.gameObject.tag));
            StartCoroutine(hitAniCoroutine());
        }
    }
    
   
    private void OnTriggerStay(Collider other)
    {
        if (isDie) return;
        if (other.CompareTag("Player"))
        {
            Vector3 dir = other.transform.position - transform.position;
            other.GetComponent<Rigidbody>().AddForce(Vector3.right * dir.normalized.x * 50, ForceMode.Impulse);
        }
    }

    //피격 애니메이션 재생 코루틴
    IEnumerator hitAniCoroutine()
    {
        if (!isDelayTime) yield break;
        if(isActiveGetHitAni) yield break;
        isActiveGetHitAni = true;
        float aniDuration = 0;
        while(true)
        {
            aniController.SetTrigger("GetHitTrigger");
            if (aniController.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
            {
                aniDuration = aniController.GetCurrentAnimatorStateInfo(0).length;
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(aniDuration);
        
        isActiveGetHitAni = false;
    }

    //피격 데미지 받는 코루틴
    IEnumerator OnDamage(string tag)
    {
        if (!isHit) yield break;

        GameObject hitEffectObj = SpawnObjectWithITag(hitEffect, transform.position);
        if (hitEffectObj)
        {
            hitEffectObj.GetComponent<ParticleSystem>().Play();
            hitEffectObj.GetComponent<HitEffect>().StartCoroutine("Start");

        }

        isHit = false;
        bossColliders[1].enabled = false;
        switch (tag)
        {
            case "Weapon":
                hp -= playerControl.GetBasicDamage();
                //Debug.Log("일반 공격 받음");
                break;
            case "Bomb":
                hp -= playerControl.GetBombDamage();
                //Debug.Log("폭탄공격 받음");
                break;
            case "SlashAttack":
                hp -= playerControl.GetSlashAttackDamage();
                //Debug.Log("검기 공격 받음");
                break;
        }
        if (hpbarUi)
            hpbarUi.ChangeHpValue((int)hp);
        yield return new WaitForSeconds(0.5f);
        bossColliders[1].enabled = true;
        
        
        isHit=true;
    }

    public string GetTag()
    {
        return detailTag;
    }

    public bool CompareToTag(string detailTag)
    {
        return this.detailTag==detailTag;
    }

    public INode GetBossDeathNode()
    {
        return DieNode;
    }
}

