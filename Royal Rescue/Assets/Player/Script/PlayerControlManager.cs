using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    public float moveSpeed = 1;//속도
    public float moveDir;//x축 방향
    public float jumpDir;//y축 방향
    public float jumpPower = 10.0f;
    float v = 0.0f;
    float h = 0.0f;
    private int jumpCnt = 0;
    private bool dirRight = true;
    public bool isFloor = false;
    private bool isDoubleJump = false;
    private Rigidbody rb;


    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //PlayerMove();
        moveDir = Input.GetAxis("Horizontal");
        if(moveDir != 0 && !Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("run");
        }
        else if (moveDir == 0 && !Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Idle");
        }
        /*if (Input.GetButtonDown("Jump") && (isFloor || !isDoubleJump))//플레이어가 점프를 눌렀고 바닥에 있거나 더블 점프중이 아닐때
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode.Impulse);//
            
            if (!isDoubleJump && !isFloor)//더블점프중이 아니고 바닥이 아니면
                isDoubleJump = true;//더블점프 가능
        }
        if(Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
        }*/
        tryJump();
        
        /*
        else if(Input.GetButtonDown("Jump") && isDoubleJump)
        {
            anim.SetTrigger("DoubleJump");
        }*/

        
    }
    private void tryJump()
    {
        if(Input.GetButtonDown("Jump"))//점프가 눌렸고
        {
            jumpCnt += 1;
            Jump();
        }
    }
    private void Jump()
    {
        if(isFloor || !isDoubleJump)//바닥이 ture거나 더블점프가 false면
        {
            rb.velocity = transform.up * jumpPower;
            if (!isDoubleJump && !isFloor)//더블점프중이 아니고 바닥이 아니면
                isDoubleJump = true;//더블점프 가능
        }
        anim.SetTrigger("Jump");
      
        if (isDoubleJump)
        {
            anim.SetTrigger("DoubleJump");
        }

    }
    /*void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");//좌우
        float v = Input.GetAxis("Vertical");//앞,뒤
        Vector3 dir = new Vector3(h, 0, v);
        transform.position += dir * moveSpeed * Time.deltaTime;
        //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }*/
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);//좌우이동

        if(isFloor)//플레이어가 바닥에 있다면
        {
            isDoubleJump = false;//더블점프 다시 꺼둠
            jumpCnt = 0;
        }
        if(!dirRight && moveDir > 0.0f)
        {
            changeDir();
        }
        else if (dirRight && moveDir < 0.0f)
        {
            changeDir();
        }

    }
    void changeDir()
    {
        dirRight = !dirRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }*/
}