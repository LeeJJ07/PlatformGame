using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float forceGravity = 50f;

    float speed = 10f;
    float jumpPower = 20f;
    public float damage = 20f;

    public float h;

    private Rigidbody rigid;
    private Collider coll;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject sword;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        sword.GetComponent<Collider>().enabled = false;
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");        // 가로축
        this.transform.position += new Vector3(h, 0, 0) * speed * Time.deltaTime;
        if (h > 1e-3 || h < -1e-3)
            this.transform.rotation = Quaternion.Euler(0, h < 0 ? 0 : 180, 0);
        Jump();
        Attack();
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y <= 1.1)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    private void FixedUpdate()
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
        //90이 왼쪽, -90이 오른쪽
        sword.GetComponent<Collider>().enabled = true;
        hand.transform.localEulerAngles = new Vector3(0, 0, 50f);
        yield return new WaitForSeconds(0.2f);
        hand.transform.localEulerAngles = new Vector3(0, 0, -8f);
        sword.GetComponent<Collider>().enabled = false;
    }
}