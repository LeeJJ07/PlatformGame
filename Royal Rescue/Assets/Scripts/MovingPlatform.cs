using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speedX = 1f;
    [SerializeField] float speedY = 1f;
    [SerializeField] float speedZ = 1f;
    [SerializeField] float area = 5f;
    [SerializeField] SpinningPlatform spinningPlatform;
    Rigidbody rb;
    Vector3 startPos;
    bool bspin = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        bspin = spinningPlatform != null;
    }

    void FixedUpdate()
    {
        if ((transform.position - startPos).magnitude >= area)
        {
            speedX *= -1;
            speedY *= -1;
            speedZ *= -1;

            if (bspin)
            {
                spinningPlatform.ChangeDirection();
            }
        }

        Vector3 speed = new(speedX, speedY, speedZ);
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime);
    }
}
