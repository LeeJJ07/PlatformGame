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

    public Material material;
    public float maxAlpha = 1f; // 최대 알파 값
    public float minAlpha = 0f; // 최소 알파 값
    private Color originalColor; // 원래 색상

    public GameObject DamageEffect;
    public int playerMaxHP = 500;
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
    public GameObject Inventory;
    public GameObject attackIcon;
    public Transform fireBallSpawnPoint;
    [SerializeField] private int jumpPossible = 2;
    private float attackDelay;
    public float moveSpeed;
    public float JumpPower;
    bool isJumpDown;
    bool isDashbool;
    [SerializeField] private int jumpCnt = 0;
    [SerializeField] public bool isDirRight = true;
    [SerializeField] private bool isFloor = false;
    [SerializeField] public bool isAttackButton = false;
    [SerializeField] public bool isAttackSecond = false;
    [SerializeField] public bool isSwordWindPossible = false;
    public bool isAttackPossible = false;
    public bool isAttackEnhance = false;
    public bool isJumpEnhance = false;
    public bool isDashPossible = false;
    public bool isFbPossible = false;
    [SerializeField] private bool isDie = false;

    private int basicDamage;
    private int slashAttackDamage = 10;
    private int bombDamage;

    public float invincibilityDuration = 2.0f;  // 무적 상태 지속 시간
    private bool isInvincible = false;  // 무적 상태 여부
    private Renderer playerRenderer;  // 플레이어 렌더러

    [SerializeField]private float holdTime = 0.0f;
    [SerializeField]private float maxHoldTime = 3.0f;
    [SerializeField] private float minThrowPower = 5;
    [SerializeField] private float maxThrowPower = 10;


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

    [SerializeField] private int coin = 0; // 코인 갯수
    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerHP = playerMaxHP;
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
        anim = GetComponentInChildren<Animator>();
        attackIcon.SetActive(true);
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
            if (Input.GetButtonDown("Dash"))
            {
                CheckDash();
            }
            if (Input.GetButtonDown("FireBallKey"))
            {
                holdTime = 0;
            }
            if (Input.GetButton("FireBallKey"))
            {
                holdTime += Time.deltaTime;
                holdTime = Mathf.Clamp(holdTime, 0, maxHoldTime);

            }
            if (Input.GetButtonUp("FireBallKey"))
            {
                ThrowBall();
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
        }

    }
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
        }
        else
        {
            moveDir = new Vector3(hAxis, 0, vAxis);
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;

            anim.SetBool("Run", moveVec != Vector3.zero && isFloor);
            anim.SetBool("Idle", moveVec == Vector3.zero && isFloor);

        }
        
    }
    void CheckDash()
    {
        if(!isDashPossible)
        {
            dashPower = (isDirRight ? Vector3.right : Vector3.left) * dash;
            rb.AddForce(dashPower, ForceMode.VelocityChange);
            anim.SetTrigger("DashTr");
            isDashPossible = true;
            StartCoroutine("DashCoolDown");
        }
        
    }
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
    void Swap()
    {
        if (equipWeapon != null)
            equipWeapon.gameObject.SetActive(false);
        equipWeapon = weapons.GetComponent<WeaponControl>();
        equipWeapon.gameObject.SetActive(true);
    }


    //공격 관련 함수
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
            }
            else if (isAttackEnhance)
                SwordWind();
            attackDelay = 0;
            weapons.GetComponent<BoxCollider>().enabled = true ;
            weapons.GetComponent<WeaponControl>().trailEffect.enabled = true;
            StopCoroutine("Swing");
            StartCoroutine("Swing");
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
        }
        else if (skillCount == 0)
        {
            Debug.Log("횟수를 모두 사용");
            return;
        }
        
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
    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        if(amount < 0)
            Debug.Log("스피드가 " + amount + "만큼 감소");
        else
            Debug.Log("스피드가 " + amount + "만큼 증가");
    }
    public void IncreaseAtk(int amount)
    {
        playerBasicATK += amount;
        if (amount < 0)
            Debug.Log("공격력이 " + amount + "만큼 감소");
        else
            Debug.Log("공격력이 " + amount + "만큼 증가");
    }
    public void IncreaseMaxHp(int amount)
    {
        playerMaxHP += amount;
        if (amount < 0)
            Debug.Log("최대 체력이 " + amount + "만큼 감소");
        else
            Debug.Log("최대 체력이 " + amount + "만큼 증가");
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

    //ENFORCE, JEWEL, HEAL, POWER, HEALTH, ATTACKSPEED
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ENFORCE":
                //?
                break;
            case "JEWEL":
                break;
            case "HEAL":
                break;
            case "POWER":
                break;
            case "HEALTH":
                break;
            case "ATTACKSPEED":
                break;
        }
    }

    
    public int GetBasicDamage()
    {
        return basicDamage + playerBasicATK;
    }
    public int GetSlashAttackDamage()
    {
        return slashAttackDamage + playerBasicATK;
    }
    public int GetBombDamage()
    {
        return bombDamage + playerBasicATK;
    }
    //
    public void HurtPlayer(int damage)
    {
        if(isInvincible == false)
        {
            playerHP -= damage;
            Debug.Log("맞음");
            StartCoroutine(Invincibility());
        }
        CheckPlayerDeath();
    }
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
        yield return new WaitForSeconds(5f);
        isFbPossible = false;
    }
    IEnumerator CheckAttack2()
    {
        yield return new WaitForSeconds(0.5f);
        isSwordWindPossible = false;
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
        isAddicted = true;

        StartCoroutine(ReduceReduceFieldOfViewSlowly());
    }
    IEnumerator ReduceReduceFieldOfViewSlowly()
    {
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

    public void AddForceToPlayer(Vector3 force, ForceMode mode)
    {
        rb.AddForce(force, mode);
    }
    public bool InputCoinKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
    public int GetCoin() { return coin; }
    public void SetCoin(int needCoin) { coin -= needCoin; }
   


    //inventory 및 items : 종진
    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None:CursorLockMode.Locked;
    }
}

