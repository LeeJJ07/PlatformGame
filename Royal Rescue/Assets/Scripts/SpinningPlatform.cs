using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpinningPlatform : MonoBehaviour
{
    [SerializeField] float speedX = 1f;
    [SerializeField] float speedY = 1f;
    [SerializeField] float speedZ = 1f;
    [SerializeField] bool isLoop = false;
    [SerializeField] float loopPoint = 30f;
    private Rigidbody rb;
    Vector3 EulerAngleVelocity;

    public float delta = 1.5f;  // Amount to move left and right from the start point
    public float speed = 2.0f;
    public float direction = 1;
    private Quaternion startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EulerAngleVelocity = new Vector3(speedX, speedY, speedZ);
        
        startPos = transform.rotation;
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(EulerAngleVelocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        if (isLoop)
        {
            Quaternion a = startPos;

            a.z += direction * (delta * Mathf.Sin(Time.time * speed));
            transform.rotation = a;
        }
    }
    public void ChangeDirection()
    {
        speedX *= -1;
        speedY *= -1;
        speedZ *= -1;
        EulerAngleVelocity = new Vector3(speedX, speedY, speedZ);
    }

}
