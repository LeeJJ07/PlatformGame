using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManagerFix : MonoBehaviour
{
    public float hAxis;
    public float vAxis;

    public float moveSpeed;
    public float JumpPower;
    private bool dirRight = true;
    bool JDown;
    bool isJump;
    public bool isFloor = false;

    int jumpCnt;
    public int jumpPossible;

    private Rigidbody rb;
    [SerializeField] Animator anim;

    Vector3 moveDir;
    Vector3 moveVec;
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
        if(JDown && jumpCnt > 0)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpPower * -Physics.gravity.y), ForceMode.Impulse);
            if (jumpCnt == 2 )
            {
                anim.SetTrigger("Jump");
                anim.SetBool("isJump", true);
            }
            else if (jumpCnt == 1)
            {
                anim.SetTrigger("DoubleJump");
                anim.SetBool("isJJump", true);
            }
            jumpCnt--;
            
        }
        
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
