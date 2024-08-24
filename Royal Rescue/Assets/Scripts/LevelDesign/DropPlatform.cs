using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 startPos;
    [SerializeField] float dropTime = 0.5f;
    [SerializeField] float resetTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Drop();
        }
    }

    void Drop()
    {
        float curTime = Time.deltaTime;
        if (curTime > dropTime)
        {
            rb.useGravity = true;

        }
        else if (curTime < resetTime)
        {
            rb.useGravity = false;
            transform.position = startPos;
        }

    }
}
