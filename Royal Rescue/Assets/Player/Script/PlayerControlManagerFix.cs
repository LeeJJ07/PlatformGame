using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManagerFix : MonoBehaviour
{
    public float hAxis;
    public float vAxis;
    public float dash = 5f;
    public GameObject weapons;
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
    [SerializeField] private bool isDirRight = true;
    [SerializeField] private bool isFloor = false;
    [SerializeField] private bool isDoubleJump = false;
    [SerializeField] private bool isAttackButton = false;
    [SerializeField] private bool isAttackPossible = false;
    
    private bool ground = false;
    public LayerMask layer;

    Rigidbody rb;
    Animator anim;
    WeaponControl equipWeapon;
 
    Vector2 inputDir;
    Vector3 moveDir;
    Vector3 moveVec;
    Vector3 dashPower;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        move();
        Jump();
        Swap();
        Attack();
        if (Input.GetButtonDown("Dash"))
        {
            //rb.AddForce(Vector3.up * Mathf.Sqrt(JumpPower * -Physics.gravity.y), ForceMode.Impulse);
            Debug.Log("´ë½Ã");
            dashPower = (isDirRight ? Vector3.right : Vector3.left) * dash;
            //rb.velocity = dashPower*moveSpeed;
            rb.AddForce(dashPower, ForceMode.VelocityChange);
            //rb.AddForce((dirRight ? Vector3.right : Vector3.left) * dash, ForceMode.Impulse);
            anim.SetTrigger("DashTr");
        }
    }
    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");//
        isJumpDown = Input.GetKeyDown(KeyCode.Space);//Á¡ÇÁ
        isAttackButton = Input.GetButtonDown("Attack");//°ø°Ý
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);//ÁÂ¿ìÀÌµ¿
                                                                    //rb.AddForce(moveDir, ForceMode.VelocityChange);//°³»¡¶óÁü;;
        if (!isDirRight && hAxis > 0.0f)
        {
            changeDir();
        }
        else if (isDirRight && hAxis < 0.0f)
        {
            changeDir();
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
        isAttackPossible = equipWeapon.rate < attackDelay ? true : false;
        if(isAttackButton && isAttackPossible && isFloor && moveDir == Vector3.zero)
        {
            equipWeapon.WeaponUse();
            anim.SetTrigger("AttackTr");
            attackDelay = 0;
        }
    }
    /*void checkGround()
    {
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 3f, Color.red);
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 0.1f))
        {
            //ground = true;
            Debug.Log("ÂøÁö");
            anim.SetTrigger("Land");
        }
        else
        { 
            //ground = false; 
        }
    }*/
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
            Debug.Log("ÇÇ°Ý");
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
}
