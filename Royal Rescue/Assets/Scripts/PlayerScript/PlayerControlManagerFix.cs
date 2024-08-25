﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerControlManagerFix : MonoBehaviour
{
    public int playerHP = 500;
    public float hAxis;
    public float vAxis;
    public float dash = 5f;
    public int skillCount = 5;
    public float dashCoolDown;
    public GameObject weapons;
    public GameObject fireBallPrefabs;
    public Transform fireBallSpawnPoint;
    [SerializeField] private int jumpPossible = 2;
    [SerializeField] private float lastGroundTime;
    [SerializeField] private float jumpPressTime;
    private float attackDelay;
    public float moveSpeed;
    public float JumpPower;
    bool isJumpDown;
    bool isJump;
    bool isDashbool;
    [SerializeField] private int jumpCnt = 0;
    [SerializeField] public bool isDirRight = true;
    [SerializeField] private bool isFloor = false;
    [SerializeField] private bool isDoubleJump = false;
    [SerializeField] public bool isAttackButton = false;
    [SerializeField] private bool isAttackPossible = false;
    [SerializeField] private bool isDashPossible = false;
    [SerializeField] private bool isFbPossible = false;
    [SerializeField] private bool isDie = false;

    private bool ground = false;
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
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
        anim = GetComponentInChildren<Animator>();


        // 종진: 확인 되셨으면 주석 지워주세요
        isAddicted = false;
        fieldView.weight = 0f;
        fieldView.profile.TryGetSettings(out vignette);
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

            setPostProcessCenter(); //포스트 프로세싱 중심 설정(종진) 확인 하셨으면 주석 지워 주세요
        }
        
        playerDie();
        
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
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpPower * -Physics.gravity.y) * 1.3f, ForceMode.Impulse);
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
        if(isAttackButton && isAttackPossible && isFloor && moveDir == Vector3.zero)
        {
            weapons.GetComponent<WeaponControl>().isAttackWeapon = true;
            equipWeapon.WeaponUse();
            anim.SetTrigger("AttackTr");
            attackDelay = 0;
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
    

    void ThrowBall()
    {
        if(skillCount > 0 && !isFbPossible)
        {
            isFbPossible = true;
            GameObject fBall = Instantiate(fireBallPrefabs, fireBallSpawnPoint.position, fireBallSpawnPoint.rotation);
            fBall.GetComponent<FireBallControl>().ballDir = isDirRight ? Vector3.right : Vector3.left;
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
    void playerDie()
    {
        if(playerHP <= 0)
        {
            moveDir = Vector3.zero;
            moveVec = Vector3.zero;
            isDie = true;
            anim.SetTrigger("DieTr");
            anim.SetBool("isDiePlayer", isDie ? true : false);
            //this.gameObject.SetActive(false);
        }
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

    }
    IEnumerator CheckFireBall()
    {
        Debug.Log("남은 횟수 : " + skillCount);
        yield return new WaitForSeconds(1f);//1초 후
        Debug.Log("스킬 키 입력 가능");
        isFbPossible = false;
    }


    /****************************************************/
    // 포스트 프로세싱 부분 (확인하시구 주석만 지워주세요)
    void setPostProcessCenter() {
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
    }
    /****************************************************/
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
        h = Input.GetAxis("Horizontal");        // ������
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
        //90�� ����, -90�� ������


        hand.transform.eulerAngles = new Vector3(0, 0, 50f);
        yield return new WaitForSeconds(1f);
        hand.transform.eulerAngles = new Vector3(0, 0, -8f);
    }
}
 */