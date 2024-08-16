using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpinningPlatform : MonoBehaviour
{
    [SerializeField] float angleX = 1f;
    [SerializeField] float angleY = 1f;
    [SerializeField] float angleZ = 1f;
    [SerializeField] bool isLoop = true;
    [SerializeField] float loopSpeed = 2.0f;

    private Rigidbody rb;
    Vector3 EulerAngleVelocity;
    private Quaternion startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EulerAngleVelocity = new Vector3(angleX, angleY, angleZ);
        
        startPos = transform.rotation;
    }

    void FixedUpdate()
    {
        if (isLoop)
        {
            Quaternion a = startPos;
            Quaternion deltaRotation = Quaternion.Euler(EulerAngleVelocity * Mathf.Sin(Time.time * loopSpeed));
            rb.MoveRotation(a * deltaRotation);
        }
        else
        {
            Quaternion deltaRotation = Quaternion.Euler(EulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
    public void ChangeDirection()
    {
        angleX *= -1;
        angleY *= -1;
        angleZ *= -1;
        EulerAngleVelocity = new Vector3(angleX, angleY, angleZ);
    }

}
