using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerControlManagerFix : MonoBehaviour
{
    private delegate void OnPlayerDeath();
    private OnPlayerDeath onPlayerDeath;

    const int PLAYER_MAX_HP = 500;

    public Material material;
    public float maxAlpha = 1f; // 최대 알파 값
    public float minAlpha = 0f; // 최소 알파 값
    private Color originalColor; // 원래 색상

    public GameObject DamageEffect;
    public int playerMaxHP = PLAYER_MAX_HP;
    public int playerHP;
    public int playerBasicATK = 0;
    public float hAxis;
    public float vAxis;
    public float dash = 5f;
    public int skillCount = 5;
    public float dashCoolDown;
    public GameObject weapons;
    public GameObject fireBallPrefabs;
    public GameObject SwordWindPrefabsR;
    public GameObject SwordWindPrefabsL;
    public GameObject attackIcon;
    public GameObject SkillCharheEft;
    public GameObject SkillPCharheEft;
    public Transform fireBallSpawnPoint;
    [SerializeField] private int jumpPossible = 2;
    private float attackDelay;
    public float moveSpeed;
    public float JumpPower;
    bool isJumpDown;
    [SerializeField] private int jumpCnt = 0;
    [SerializeField] public bool isDirRight = true;
    [SerializeField] private bool isFloor = false;
    [SerializeField] private bool isAttackButton = false;//
    //[SerializeField] private bool isAttackSecond = false;//

    private bool isRunning = false;
    public bool isSwordWindPossible = false;
    public bool isAttackPossible = false;
    public bool isAttackEnhance = false;
    public bool isJumpEnhance = false;
    public bool isDashPossible = false;
    
    [SerializeField] private bool isDie = false;

    private int[] damageRange = {-5,-4,-3,-2,-1,0,1,2,3,4,5 };
    private int basicDamage;//기본(근접 데미지)
    private int slashAttackDamage;//원거리 데미지
    private int bombDamage;//폭탄 데미지

    public float invincibilityDuration = 2.0f;  // 무적 상태 지속 시간
    private bool isInvincible = false;  // 무적 상태 여부
    private Renderer playerRenderer;  // 플레이어 렌더러

    [SerializeField]private float holdTime = 0.0f;//폭탄 차지 시간
    [SerializeField]private float maxHoldTime = 3.0f;//폭탄 최대 차지 가능 시간
    [SerializeField] private float minThrowPower = 5;//폭탄이 받는 최소 방향 파워
    [SerializeField] private float maxThrowPower = 10;//최대 방향 파워


    public LineRenderer lineRenderer;//궤적용
    public int trajectoryResolution = 30; // 궤적의 점 최대수
    private Vector3 ballDirection; // 폭탄의 방향

    private bool isSkillCharging = false;
    public bool isBombStart = false;
    public bool isFbPossible = false;

    public LayerMask layer;

    Rigidbody rb;
    Animator anim;
    WeaponControl equipWeapon;
 
    Vector2 inputDir;
    Vector3 moveDir;
    Vector3 moveVec;
    Vector3 dashPower;

    bool isAddicted;
    [SerializeField] PostProcessVolume fieldView;
    private Vignette vignette;

    private Canvas uiCanvas;
    public GameObject DamageTextPrefab;

    [SerializeField] private int coin = 0; // 코인 갯수
    [SerializeField] private CoinUI coinUI;

    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            coinUI.UpdateCoinText(coin);
        }
    } 

    public Inventory inventory;

    // 엔딩 후 타이틀 화면 복귀 시 참조할 플레이어의 초기 스테이터스 ///
    private float ogMoveSpeed;
    private int ogPlayerBasicATK, ogCoin;

    ///////////////////////////////////////////////////////////////

    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerHP = playerMaxHP;
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
        anim = GetComponentInChildren<Animator>();
        attackIcon.SetActive(true);
        SkillCharheEft.SetActive(false);
        SkillPCharheEft.SetActive(false);
        weapons.GetComponent<BoxCollider>().enabled = false;
        weapons.GetComponent<WeaponControl>().trailEffect.enabled = false;

        isAddicted = false;
        fieldView.weight = 0f;

        if (onPlayerDeath == null)
        {
            onPlayerDeath += playerDie;
            onPlayerDeath += () => StartCoroutine(GameDirector.instance.RespawnScreenTransition());
        }

        basicDamage = weapons.GetComponent<WeaponControl>().damage;
        bombDamage = fireBallPrefabs.GetComponent<FireBallControl>().bombDamage;
        slashAttackDamage = SwordWindPrefabsL.GetComponent<SwordWindControl>().slashDamage;

        inventory = GetComponent<Inventory>();

        uiCanvas = GameObject.Find("InGame Canvas").GetComponent<Canvas>();

        CachePlayerStatus();
        coinUI.UpdateCoinText(coin);
    }
    // Update is called once per frame
    void Update()
    {
        if(!isDie)
        {
            GetInput();
            move();
            Jump();
            Swap();
            Attack();
            if (InventoryKeyDown())
                inventory.Toggle();
            if (Input.GetButtonDown("Dash"))
            {
                CheckDash();
            }

            if (skillCount > 0 && !isFbPossible)
            {
                if (Input.GetButtonDown("FireBallKey") && !isBombStart)
                {
                    SoundManager.Instance.PlaySound("BombCharging", true, SoundType.EFFECT);
                    isBombStart = true;
                    isSkillCharging = true;
                    holdTime = 0;
                }
                if (Input.GetButton("FireBallKey") && isBombStart)
                {
                    SkillCharheEft.SetActive(true);
                    holdTime += Time.deltaTime;
                    holdTime = Mathf.Clamp(holdTime, 0, maxHoldTime);
                    if(holdTime >= 3.0f)
                    {
                        SkillCharheEft.SetActive(false);
                        SkillPCharheEft.SetActive(true);
                    }
                    ballDirection = ((isDirRight ? Vector3.right : Vector3.left) + Vector3.up) * 1.3f; //라인렌더러용 방향 설정
                    ShowTrajectory(fireBallSpawnPoint.position, ballDirection, Mathf.Lerp(minThrowPower, maxThrowPower, holdTime / maxHoldTime));
                }
                if (Input.GetButtonUp("FireBallKey") && isBombStart)
                {
                    if (isSkillCharging)
                    {
                        SoundManager.Instance.StopLoopSound("BombCharging");
                        isSkillCharging = false;
                        SkillCharheEft.SetActive(false);
                        SkillPCharheEft.SetActive(false);
                    }

                    ThrowBall();

                }
            }

            setPostProcessCenter();
        }
    }
    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        isJumpDown = Input.GetKeyDown(KeyCode.Space);
        isAttackButton = Input.GetButtonDown("Attack");
    }
    private void FixedUpdate()
    {
        if(!isDie)
        {
            rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);
                                                              
            if (!isDirRight && hAxis > 0.0f)
            {
                changeDir();
            }
            else if (isDirRight && hAxis < 0.0f)
            {
                changeDir();
            }
            //if(moveVec == Vector3.zero)
                //SoundManager.Instance.StopLoopSound("RunMove");
        }

    }

    //방향 및 이동 관련 함수 START
    void changeDir()
    {
        isDirRight = !isDirRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }

    void move()
    {
        if(!isAttackPossible)
        {
            moveDir = Vector3.zero;
            if (isRunning)
            {
                SoundManager.Instance.StopLoopSound("RunMove");
                isRunning = false;
            }
        }
        else
        {
            moveDir = new Vector3(hAxis, 0, vAxis);
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
            if (moveVec != Vector3.zero && isFloor && !isRunning)
            {
                SoundManager.Instance.PlaySound("RunMove", true, SoundType.EFFECT);
                isRunning = true;
            }
            else if ((moveVec == Vector3.zero || !isFloor) && isRunning)
            {

                    SoundManager.Instance.StopLoopSound("RunMove");
                    isRunning = false;
            }

            

            anim.SetBool("Run", moveVec != Vector3.zero && isFloor);
            anim.SetBool("Idle", moveVec == Vector3.zero && isFloor);

        }
    }
    ////방향 및 이동 관련 함수 END


    //대쉬 START
    void CheckDash()
    {
        if(!isDashPossible)
        {
            SoundManager.Instance.PlaySound("DashMove");
            dashPower = (isDirRight ? Vector3.right : Vector3.left) * dash;
            rb.AddForce(dashPower, ForceMode.VelocityChange);
            anim.SetTrigger("DashTr");
            isDashPossible = true;
            StartCoroutine("DashCoolDown");
        }
        
    }

    //대쉬 END


    //점프, 2단 점프 START
    void Jump()
    {
        if (isJumpDown && jumpCnt > 0)
        {

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpPower * -Physics.gravity.y) * 1.5f, ForceMode.Impulse);
            if (jumpCnt == 2)
            {
                anim.SetTrigger("Jump");
                anim.SetBool("isJump", true);
            }
            else if (jumpCnt == 1 && !isFloor)
            {
                anim.SetTrigger("DoubleJump");
                anim.SetBool("isDoubleJump", true);
            }
            jumpCnt--;

            SoundManager.Instance.PlaySound("Jump");
        }

    }
    //점프, 2단 점프 END
    void Swap()
    {
        if (equipWeapon != null)
            equipWeapon.gameObject.SetActive(false);
        equipWeapon = weapons.GetComponent<WeaponControl>();
        equipWeapon.gameObject.SetActive(true);
    }


    //공격 관련 함수 START
    void Attack()
    {
        if (equipWeapon == null)
            return;
        attackDelay += Time.deltaTime;
        isAttackPossible = equipWeapon.rate < attackDelay ? true : false;
        if(isAttackButton && isAttackPossible )
        {    
            weapons.GetComponent<WeaponControl>().isAttackWeapon = true;
            equipWeapon.WeaponUse();
            if (!isAttackEnhance)
            {
                anim.SetTrigger("AttackTr");
                weapons.GetComponent<BoxCollider>().enabled = true;
            }
            else if (isAttackEnhance)
                SwordWind();
            attackDelay = 0;
            weapons.GetComponent<WeaponControl>().trailEffect.enabled = true;
            StopCoroutine("Swing");
            StartCoroutine("Swing");
            SoundManager.Instance.PlaySound("AttackSound");
        }

    }
    

    public void ThrowBall()
    {
        if(skillCount > 0 && !isFbPossible)
        {
            GameObject fBall;
            isFbPossible = true;
            fBall = Instantiate(fireBallPrefabs, fireBallSpawnPoint.position, fireBallSpawnPoint.rotation);
            fBall.GetComponent<FireBallControl>().throwForce = Mathf.Lerp(minThrowPower, maxThrowPower, holdTime / maxHoldTime);
            fBall.GetComponent<FireBallControl>().ballDir = isDirRight ? Vector3.right : Vector3.left;
            anim.SetTrigger("FireBallTr");
            skillCount -= 1;
            StartCoroutine("CheckFireBall");


            ClearTrajectory(); // 궤적을 지움
        }
        else if (skillCount == 0)
        {
            Debug.Log("횟수를 모두 사용");
            return;
        }
        
    }

    void ShowTrajectory(Vector3 startPosition, Vector3 direction, float force)
    {
        Vector3[] points = new Vector3[trajectoryResolution];
        Vector3 velocity = direction * force; // 초기 속도

        float simulationTimeStep = 0.1f;
        float totalTime = 2.0f;

        // 포물선 궤적 계산
        for (int i = 0; i < trajectoryResolution; i++)
        {
            float time = i * simulationTimeStep;

            if (time > totalTime)
                break;
            // 포물선 공식: 초기 위치 + 속도 * 시간 + 0.5 * 중력 * 시간^2
            points[i] = startPosition + velocity * time + 0.5f * Physics.gravity * time * time;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    // 궤적을 지우는 함수
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }


    public void SwordWind()
    {  
        if (isAttackPossible)
        {
            GameObject fBall;
            isSwordWindPossible = true;
            if (isDirRight)
            {
                fBall = Instantiate(SwordWindPrefabsR, fireBallSpawnPoint.position, fireBallSpawnPoint.rotation);
            }
            else
            {
                fBall = Instantiate(SwordWindPrefabsL, fireBallSpawnPoint.position, fireBallSpawnPoint.rotation);
            }
            fBall.GetComponent<SwordWindControl>().ballDir = isDirRight ? Vector3.right : Vector3.left;
            anim.SetTrigger("FireBallTr");
            StartCoroutine("CheckAttack2");
        }
        else
            return;
        
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.05f);
        weapons.GetComponent<WeaponControl>().isAttackWeapon = false;
        yield return new WaitForSeconds(0.25f);
        weapons.GetComponent<BoxCollider>().enabled = false;
        weapons.GetComponent<WeaponControl>().trailEffect.enabled = false;

    }
    IEnumerator CheckFireBall()
    {
        Debug.Log("남은 횟수 : " + skillCount);
        yield return new WaitForSeconds(3f);
        isFbPossible = false;
        isBombStart = false;

    }
    IEnumerator CheckAttack2()
    {
        yield return new WaitForSeconds(0.5f);
        isSwordWindPossible = false;
    }
    //공격 관련 함수 END



    public void IncreaseCurHp(int amount) 
    {
        playerHP += amount;
        if (amount < 0)
            Debug.Log("현재체력 " + amount + "만큼 감소");
        else
            Debug.Log("현재체력 " + amount + "만큼 증가");

        if (playerHP > playerMaxHP) playerHP = playerMaxHP;
        else if (playerHP < 0) playerHP = 0;
    }
    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        if(amount < 0)
            Debug.Log("스피드가 " + amount + "만큼 감소");
        else
            Debug.Log("스피드가 " + amount + "만큼 증가");

        if (moveSpeed > 12) moveSpeed = 12;           // maxMoveSpeed 12로 제한
        else if (moveSpeed < 5) moveSpeed = 5;      // minMoveSpeed 5로 제한
    }
    public void IncreaseAtk(int amount)
    {
        playerBasicATK += amount;
        if (amount < 0)
            Debug.Log("공격력이 " + amount + "만큼 감소");
        else
            Debug.Log("공격력이 " + amount + "만큼 증가");

        if (playerBasicATK < 0) playerBasicATK = 0;      // minAttack 10으로 제한
    }
    public void IncreaseMaxHp(int amount)
    {
        playerMaxHP += amount;
        if (amount > 0)
            IncreaseCurHp(amount);
            
        if (amount < 0)
            Debug.Log("최대 체력이 " + amount + "만큼 감소");
        else
            Debug.Log("최대 체력이 " + amount + "만큼 증가");

        if (playerMaxHP < 30) playerMaxHP = 30;      // MaxHp 하향 30으로 제한
        playerHP = playerHP > playerMaxHP ? playerMaxHP : playerHP;
    }
    void playerDie()
    {
        isDie = true;

        SetPlayerKinematic(true);
        SetPlayerKinematic(false);
        FixatePlayerRigidBody(true);

        anim.SetTrigger("DieTr");
        anim.SetBool("isDiePlayer", isDie ? true : false);
    }
    
    public void RevivePlayer()
    {
        FixatePlayerRigidBody(false);

        playerHP = playerMaxHP;
        anim.SetBool("isDiePlayer", false);
        anim.SetBool("Idle", true);
        anim.Play(AnimationHash.PLAYER_IDLE);
        isDie = false;
    }

    private void CheckPlayerDeath()
    {
        if (!isDie && playerHP <= 0 && onPlayerDeath != null)
            onPlayerDeath();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 1f, Color.red);
        RaycastHit hit;
        if (collision.gameObject.CompareTag("Floor") && Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            isFloor = true;
            anim.SetBool("isJump", false);
            anim.SetBool("isDoubleJump", false);
            anim.SetBool("isGround", false);
            jumpCnt = jumpPossible;
        }
        else if(!collision.gameObject.CompareTag("Floor") && !isFloor &&Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            jumpCnt = 0;
        }
        if (collision.gameObject.CompareTag("Enemy") && !isAttackButton)
        {
            EnemyControler enemy = collision.gameObject.GetComponent<EnemyControler>();
            if(!isInvincible)
            {
                playerHP -= enemy.enemyAtk;
                Debug.Log("맞음");
            }
            
        }
    }

    //플레이어 피격시 외부에서 데미지를 받아올 수 있게 -> 이부분 수정 필요
    public int GetBasicDamage()
    {
        return basicDamage + playerBasicATK + damageRange[Random.Range(0, damageRange.Length)];
    }
    public int GetSlashAttackDamage()
    {
        return slashAttackDamage + playerBasicATK + damageRange[Random.Range(0, damageRange.Length)]; ;
    }
    public int GetBombDamage()
    {
        return bombDamage + playerBasicATK + damageRange[Random.Range(0, damageRange.Length)]; ;
    }
    public void HurtPlayer(int damage)
    {
        if(isInvincible == false)
        {
            SoundManager.Instance.PlaySound("BeDamage");
            playerHP -= damage;

            Vector3 nVec = new Vector3(0, 1.5f, 0);
            var screenPos = Camera.main.WorldToScreenPoint(transform.position + nVec);
            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.GetComponent<RectTransform>(), screenPos, uiCanvas.worldCamera, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

            GameObject damageUI = Instantiate(DamageTextPrefab) as GameObject;
            damageUI.GetComponent<DamageText>().damage = -damage;
            damageUI.transform.SetParent(uiCanvas.transform, false);
            damageUI.transform.localPosition = localPos;
            damageUI.GetComponent<DamageText>().colorG = 0f;
            damageUI.GetComponent<DamageText>().colorB = 0f;

            StartCoroutine(Invincibility());
        }
        CheckPlayerDeath();
    }
    //
    private void OnCollisionExit(Collision collision)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 3f, Color.red);
        if (collision.gameObject.tag == "Floor" && !Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            isFloor = false;
            anim.SetBool("isGround", true);
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;  // 무적 상태 활성화
        Debug.Log("무적상태 진입");
        // 시각적 피드백(예: 깜빡이기 효과)
        
        for (float i = 0; i <= invincibilityDuration; i += 0.4f)
        {
            Debug.Log("깜빡");
            originalColor = material.color;
            material.color = new Color(255, 130, 100, 255);
            yield return new WaitForSeconds(0.05f);  // 0.2초 동안 대기
            material.color = originalColor;
            yield return new WaitForSeconds(0.05f);
        }

        isInvincible = false;  // 무적 상태 해제
        Debug.Log("무적상태 해제");
    }
    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("대쉬온");
        isDashPossible = false;
    }

    void setPostProcessCenter()
    {
        if (Camera.main == null)
            return;

        if (vignette == null)
            fieldView.profile.TryGetSettings(out vignette);

        if (!Camera.main || !vignette)
            return;
        
        Vector3 playerWorldPosition = transform.position;

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(playerWorldPosition);

        vignette.center.value = viewportPosition;
    }

    private void OnParticleCollision(GameObject other)
    {
        switch (other.tag)
        {
            case "Poison":
                ReduceFieldOfView();
                break;
        }
    }

    void ReduceFieldOfView()
    {
        if (isAddicted) return;
        

        StartCoroutine(ReduceReduceFieldOfViewSlowly());
    }
    IEnumerator ReduceReduceFieldOfViewSlowly()
    {
        isAddicted = true;
        for (int i = 0; i < 100; i++)
        {
            fieldView.weight += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(8f);
        for (int i = 0; i < 100; i++)
        {
            fieldView.weight -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        isAddicted = false;
    }

    public void FixatePlayerRigidBody(bool isFixated)
    {
        if (isFixated)
            rb.constraints |= RigidbodyConstraints.FreezePositionX;
        else
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    public void SetPlayerKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }

    public void SetPlayerVelocity(float x, float y, float z)
    {
        rb.velocity = new Vector3(x, y, z);
    }

    public void SetPlayerEnabled(bool state)
    {
        gameObject.SetActive(state);
    }

    public void AddForceToPlayer(Vector3 force, ForceMode mode)
    {
        rb.AddForce(force, mode);
    }
    public bool InputCoinKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
    public int GetCoin() { return coin; }
    public void EatCoin() { Coin++; }
    public void SetCoin(int needCoin) { Coin -= needCoin; }

    public bool InventoryKeyDown() {
        return Input.GetKeyDown(KeyCode.I);
    }

    //inventory 및 items

    private void CachePlayerStatus()
    {
        ogMoveSpeed = moveSpeed;
        ogPlayerBasicATK = playerBasicATK;
        ogCoin = coin;
    }

    public void ResetPlayerStatus()
    {
        inventory.ResetItemSlots();
        playerHP = playerMaxHP = PLAYER_MAX_HP;
        moveSpeed = ogMoveSpeed;
        playerBasicATK = ogPlayerBasicATK;
        coin = ogCoin;
    }
}

