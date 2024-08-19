using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManagerFix : MonoBehaviour
{
    public float hAxis;
    public float vAxis;
    public float dash = 5f;

    public float moveSpeed;
    public float JumpPower;
    [SerializeField]private bool dirRight = true;
    bool JDown;
    bool isJump;
    bool dashbool;

    public bool isFloor = false;
    private bool ground = false;
    public LayerMask layer;

    int jumpCnt;
    public int jumpPossible;

    private Rigidbody rb;
    [SerializeField] Animator anim;

    Vector3 moveDir;
    Vector3 moveVec;
    Vector3 dashPower;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        move();
        Jump();
        if(Input.GetButtonDown("Dash"))
        {
            //rb.AddForce(Vector3.up * Mathf.Sqrt(JumpPower * -Physics.gravity.y), ForceMode.Impulse);
            Debug.Log("대시");
            dashPower = (dirRight ? Vector3.right : Vector3.left) * dash;
            //rb.velocity = dashPower*moveSpeed;
            rb.AddForce(dashPower, ForceMode.VelocityChange);
            //rb.AddForce((dirRight ? Vector3.right : Vector3.left) * dash, ForceMode.Impulse);
            anim.SetTrigger("Dash");
        }
        //Trun();
        //Dodge();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);//좌우이동

        if (!dirRight && hAxis > 0.0f)
        {
            changeDir();
        }
        else if (dirRight && hAxis < 0.0f)
        {
            changeDir();
        }

    }
    void changeDir()
    {
        dirRight = !dirRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }
    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");//
        //wDown = Input.GetButton("Walk");//걷기
        JDown = Input.GetKeyDown(KeyCode.Space);//점프
        //JJDown = Input.GetButtonDown("Jump");//더블점프
        //dDown = Input.GetButtonDown("Dodge");//회피
    }
    void move()
    {
        moveDir = new Vector3(hAxis, 0, vAxis);
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        //transform.position += moveVec * moveSpeed * Time.deltaTime;
        anim.SetBool("run", moveVec != Vector3.zero);
        anim.SetBool("Idle", moveVec == Vector3.zero);
    }
    void Jump()
    {
        if(JDown && jumpCnt > 0 )
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpPower * -Physics.gravity.y), ForceMode.Impulse);
            if (jumpCnt == 2 && isFloor)
            {
                anim.SetTrigger("Jump");
                anim.SetBool("isJump", true);
            }
            else if (jumpCnt == 1 && !isFloor)
            {
                anim.SetTrigger("DoubleJump");
                anim.SetBool("isJJump", true);
            }
            jumpCnt--;
        }
        //if(jumpCnt < 1)
          //  checkGround();
            /*if (JDown && !isJump && isFloor)
            {
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                anim.SetBool("isJump", true);
                anim.SetTrigger("Jump");
                isJump = true;
            }
            else if (JJDown  && !isJJDown && isJump)
            {
                rb.AddForce(Vector3.up * DJumpPower, ForceMode.Impulse);
                //anim.SetBool("isJJump", true);
                anim.SetTrigger("DoubleJump");
                isJJDown = true;
            }*/
     }
    void checkGround()
    {
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 3f, Color.red);
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 0.1f))
        {
            //ground = true;
            Debug.Log("착지");
            anim.SetTrigger("Land");
        }
        else
        { 
            //ground = false; 
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isFloor = true;
            //isFloor = true;
            anim.SetBool("isJump", false);
            //isJump = false;
            anim.SetBool("isJJump", false);
            //isJJDown = false;
            jumpCnt = jumpPossible;
            
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isFloor = false;
        }
    }
}
