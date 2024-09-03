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

    //public GameObject[] PlayerHeart = new GameObject[5];
    public GameObject DamageEffect;
    public int playerMaxHP = 500;
    public int playerHP;
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
    public GameObject attackIcon;//스킬 활성화 비활성화 표시용
    public Transform fireBallSpawnPoint;//폭탄 생성 좌표
    [SerializeField] private int jumpPossible = 2;//점프 최대 가능 횟수
    //[SerializeField] private float lastGroundTime;
    //[SerializeField] private float jumpPressTime;
    private float attackDelay;
    public float moveSpeed;
    public float JumpPower;
    bool isJumpDown;
    //bool isJump;
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
    private Vignette vignette; // 비네팅 효과
    void Start()
    {
        playerHP = playerMaxHP;
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
        anim = GetComponentInChildren<Animator>();
        Inventory.SetActive(false);
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
                ThrowBall();
            }
            if (Input.GetButtonDown("InventoryKey"))
            {
                Inventory.SetActive(true);
            }
            if(isAttackEnhance)
            {
                attackIcon.SetActive(false);
            }

            setPostProcessCenter();
        }
    }
    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");//
        isJumpDown = Input.GetKeyDown(KeyCode.Space);//점프
        isAttackButton = Input.GetButtonDown("Attack");//공격
    }
    private void FixedUpdate()
    {
        if(!isDie)
        {
            rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);//좌우이동
                                                                        //rb.AddForce(moveDir, ForceMode.VelocityChange);//개빨라짐;;
            if (!isDirRight && hAxis > 0.0f)
            {
                changeDir();
            }
            else if (isDirRight && hAxis < 0.0f)
            {
                changeDir();
            }
        }
        
        /*if(rb.velocity.y > 0)
        {
            Debug.DrawRay(rb.position, Vector3.down * 3f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rb.position,Vector3.down, 1);
            if (hit.collider != null)
            {
                if(hit.distance < 0.1f)
                {
                    anim.SetBool("isJump", false);
                    anim.SetBool("isDoubleJump", false);
                    anim.SetTrigger("Land");
                }
            }
        }*/
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
            //rb.AddForce(Vector3.up * Mathf.Sqrt(JumpPower * -Physics.gravity.y), ForceMode.Impulse);
            Debug.Log("대쉬");
            dashPower = (isDirRight ? Vector3.right : Vector3.left) * dash;
            //rb.velocity = dashPower*moveSpeed;
            rb.AddForce(dashPower, ForceMode.VelocityChange);
            //rb.AddForce((dirRight ? Vector3.right : Vector3.left) * dash, ForceMode.Impulse);
            anim.SetTrigger("DashTr");
            isDashPossible = true;
            StartCoroutine("DashCoolDown");
        }
        else
        {
            Debug.Log("대쉬 쿨타임임");
        }
        
    }
    void Jump()
    {
        //touch = Input.GetTouch(0);
        if (isJumpDown && jumpCnt > 0)
        {
            //curTabTime = Time.time;
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
            //lastTabTime = Time.time;

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
    void Attack()
    {
        if (equipWeapon == null)
            return;
        attackDelay += Time.deltaTime;
        isAttackPossible = equipWeapon.rate < attackDelay ? true : false;//공격 딜레이 시간이 공격 쿨타임(rate)을 넘었다면 ? 공격 함 : 공격 눌러도 안써짐
        if(isAttackButton && isAttackPossible )
        {    
            weapons.GetComponent<WeaponControl>().isAttackWeapon = true;
            equipWeapon.WeaponUse();
            if (!isAttackEnhance)
                anim.SetTrigger("AttackTr");
            else if (isAttackEnhance)
                SwordWind();
            attackDelay = 0;
            weapons.GetComponent<BoxCollider>().enabled = true ;
            weapons.GetComponent<WeaponControl>().trailEffect.enabled = true;
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
    

    public void ThrowBall()//폭탄 사용 체크
    {
        if(skillCount > 0 && !isFbPossible)
        {
            GameObject fBall;
            isFbPossible = true;
            fBall = Instantiate(fireBallPrefabs, fireBallSpawnPoint.position, fireBallSpawnPoint.rotation);
            fBall.GetComponent<FireBallControl>().ballDir = isDirRight ? Vector3.right : Vector3.left;
            //fBall.GetComponent<FireBallControl>().isFireball = true;
            //Vector3 throwDir = (isDirRight ? Vector3.right : Vector3.left) + Vector3.up * 0.5f; // 약간 위로 던짐 (포물선 효과)
            //fBall.GetComponent<FireBallControl>().Throw(throwDir);
            //fBall.transform.position = fireBallSpawnPoint.transform.position;
            anim.SetTrigger("FireBallTr");
            skillCount -= 1;
            StartCoroutine("CheckFireBall");
        }
        else if (skillCount == 0)
        {
            Debug.Log("파이어볼 횟수 모두 사용");
            return;
        }
        
    }
    public void SwordWind()//원거리공격 방향에 따른 프리팹 및 원거리공격 딜레이와 애니메이션(폭탄과 동일 애니메이션)
    {  
        if (!isSwordWindPossible)
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
            //fBall = Instantiate(fireBallPrefabs, fireBallSpawnPoint.position, fireBallSpawnPoint.rotation);
            fBall.GetComponent<SwordWindControl>().ballDir = isDirRight ? Vector3.right : Vector3.left;
            //fBall.GetComponent<FireBallControl>().isFireball = true;
            //Vector3 throwDir = (isDirRight ? Vector3.right : Vector3.left) + Vector3.up * 0.5f; // 약간 위로 던짐 (포물선 효과)
            //fBall.GetComponent<FireBallControl>().Throw(throwDir);
            //fBall.transform.position = fireBallSpawnPoint.transform.position;
            anim.SetTrigger("FireBallTr");
            StartCoroutine("CheckAttack2");
        }
        else
            return;
        
    }
    void playerDie()
    {
        isDie = true;
        rb.isKinematic = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;

        anim.SetTrigger("DieTr");
        anim.SetBool("isDiePlayer", isDie ? true : false);
        //this.gameObject.SetActive(false);
    }
    
    public void RevivePlayer()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

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
        /*
        if(collision.gameObject.CompareTag("Floor") && Physics.Raycast(transform.position, isDirRight ? Vector3.right : Vector3.left, out hit, 1f) && !isFloor)
        {
            if (isDirRight)
                this.transform.position += new Vector3(-0.1f, 0, 0 );
            else
                this.transform.position += new Vector3(0.1f, 0, 0);
        }*/
        if (collision.gameObject.CompareTag("Enemy") && !isAttackButton)
        {
            EnemyControler enemy = collision.gameObject.GetComponent<EnemyControler>();
            playerHP -= enemy.enemyAtk;
            Debug.Log("피격");
        }
    }

    //플레이어 데미지 관련 외부 접근

    public int GetBasicDamage()
    {
        return weapons.GetComponent<WeaponControl>().damage;
    }
    public int GetSlashAttackDamage()
    {
        return weapons.GetComponent<SwordWindControl>().slashDamage;
    }
    public int GetBombDamage()
    {
        return fireBallPrefabs.GetComponent<FireBallControl>().bombDamage;
    }
    //
    public void HurtPlayer(int damage)
    {
        playerHP -= damage;
        Debug.Log("피격");

        CheckPlayerDeath();
    }
    private void OnCollisionExit(Collision collision)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 3f, Color.red);
        if (collision.gameObject.tag == "Floor" && !Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            isFloor = false;
            anim.SetBool("isGround", true);
        }
    }

    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(1f);//1초 후
        Debug.Log("대쉬 재활성화");
        isDashPossible = false;
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.05f);//0.05초 후
        weapons.GetComponent<WeaponControl>().isAttackWeapon = false;//무기 공격 트리거 상태를 false로 바꿔 공격중이지 않을때는 충돌 트리거 이벤트가 발생하지 않게
        yield return new WaitForSeconds(0.25f);
        weapons.GetComponent<BoxCollider>().enabled = false;
        weapons.GetComponent<WeaponControl>().trailEffect.enabled = false;

    }
    IEnumerator CheckFireBall()
    {
        Debug.Log("남은 횟수 : " + skillCount);
        yield return new WaitForSeconds(5f);//5초 후
        Debug.Log("스킬 키 입력 가능");
        isFbPossible = false;
    }
    IEnumerator CheckAttack2()
    {
        yield return new WaitForSeconds(0.5f);//1초 후
        Debug.Log("스킬 키 입력 가능");
        isSwordWindPossible = false;
    }

    void setPostProcessCenter()
    {
        if (Camera.main == null)
            return;

        if (vignette == null)
        {
            fieldView.profile.TryGetSettings(out vignette);
        }
        Vector3 playerWorldPosition = transform.position;

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(playerWorldPosition);

        vignette.center.value = viewportPosition;
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("충돌");
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

}
/*
 * private Rigidbody rigid;
    private Collider coll;
    [SerializeField] private GameObject hand;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

@@ -22,10 +23,11 @@ public class PlayerController : MonoBehaviour
    void Update()
    {
        h = Input.GetAxis("Horizontal");        // ??????
        transform.position += new Vector3(h, 0, 0) * speed * Time.deltaTime;
        this.transform.position += new Vector3(h, 0, 0) * speed * Time.deltaTime;
        if (h > 1e-3 || h < -1e-3)
            transform.rotation = Quaternion.Euler(0, h < 0 ? 0 : 180, 0);
            this.transform.rotation = Quaternion.Euler(0, h < 0 ? 0 : 180, 0);
        Jump();
        Attack();
    }
    void Jump()
    {

@@ -38,4 +40,21 @@ public class PlayerController : MonoBehaviour
    {
        rigid.AddForce(Vector3.down * forceGravity);
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Atk());
        } 
    }

    IEnumerator Atk()
    {
        //90?? ????, -90?? ??????


        hand.transform.eulerAngles = new Vector3(0, 0, 50f);
        yield return new WaitForSeconds(1f);
        hand.transform.eulerAngles = new Vector3(0, 0, -8f);
    }
}
 */
