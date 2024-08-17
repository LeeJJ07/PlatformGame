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
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");        // °¡·ÎÃà
        transform.position += new Vector3(h, 0, 0) * speed * Time.deltaTime;
        if (h > 1e-3 || h < -1e-3)
            transform.rotation = Quaternion.Euler(0, h < 0 ? 0 : 180, 0);
        Jump();
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
}