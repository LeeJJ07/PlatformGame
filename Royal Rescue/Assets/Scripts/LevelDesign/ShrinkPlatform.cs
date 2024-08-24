
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPlatform : MonoBehaviour
{
    [SerializeField] float speedX = 0.1f;
    [SerializeField] float speedY = 0.1f;
    [SerializeField] float speedZ = 0.1f;
    [SerializeField] float minimunSize = 0.1f;
    [SerializeField] float maximunSize = 1f;

    private Rigidbody rb;
    private Vector3 scaleChange;
    void Start()
    {
        scaleChange = new Vector3(speedX, speedY, speedZ);

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale += scaleChange;
        rb.MovePosition(transform.position + scaleChange/2);

        if (transform.localScale.x < minimunSize || transform.localScale.x > maximunSize)
        {
            scaleChange = -scaleChange;
        }
        else if (transform.localScale.y < minimunSize || transform.localScale.y > maximunSize)
        {
            scaleChange = -scaleChange;
        }
        else if (transform.localScale.z < minimunSize || transform.localScale.z > maximunSize)
        {
            scaleChange = -scaleChange;
        }
    }
}
