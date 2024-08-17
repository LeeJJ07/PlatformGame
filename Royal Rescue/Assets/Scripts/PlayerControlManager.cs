using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    public float moveSpeed = 1;//�ӵ�
    public float moveDir;//x�� ����
    public float jumpDir;//y�� ����
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
        /*if (Input.GetButtonDown("Jump") && (isFloor || !isDoubleJump))//�÷��̾ ������ ������ �ٴڿ� �ְų� ���� �������� �ƴҶ�
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode.Impulse);//
            
            if (!isDoubleJump && !isFloor)//������������ �ƴϰ� �ٴ��� �ƴϸ�
                isDoubleJump = true;//�������� ����
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
        if(Input.GetButtonDown("Jump"))//������ ���Ȱ�
        {
            jumpCnt += 1;
            Jump();
        }
    }
    private void Jump()
    {
        if(isFloor || !isDoubleJump)//�ٴ��� ture�ų� ���������� false��
        {
            rb.velocity = transform.up * jumpPower;
            if (!isDoubleJump && !isFloor)//������������ �ƴϰ� �ٴ��� �ƴϸ�
                isDoubleJump = true;//�������� ����
        }
        anim.SetTrigger("Jump");
      
        if (isDoubleJump)
        {
            anim.SetTrigger("DoubleJump");
        }

    }
    /*void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");//�¿�
        float v = Input.GetAxis("Vertical");//��,��
        Vector3 dir = new Vector3(h, 0, v);
        transform.position += dir * moveSpeed * Time.deltaTime;
        //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }*/
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);//�¿��̵�

        if(isFloor)//�÷��̾ �ٴڿ� �ִٸ�
        {
            isDoubleJump = false;//�������� �ٽ� ����
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