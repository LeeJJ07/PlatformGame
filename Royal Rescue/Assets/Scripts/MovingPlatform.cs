using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speedX = 1f;
    [SerializeField] float speedY = 1f;
    [SerializeField] float speedZ = 1f;
    [SerializeField] float area = 5f;

    Rigidbody rb;
    Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if ((transform.position - startPos).magnitude >= area)
        {
            speedX *= -1;
        }

        Vector3 speed = new(speedX, speedY, speedZ);
        rb.MovePosition(transform.position + speed * Time.deltaTime);
    }
}
