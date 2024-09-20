using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    Coroutine coMove = null;
    IEnumerator CoMove(PlayerControlManagerFix player)
    {
        float prePosX = transform.position.x;
        while(true)
        {
            float posX = prePosX - transform.position.x;
            player.transform.position = player.transform.position + (Vector3.left * posX);
            prePosX = transform.position.x;

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<PlayerControlManagerFix>(out PlayerControlManagerFix player))
        {
            if(coMove != null) StopCoroutine(coMove);
            coMove = StartCoroutine(CoMove(player));
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(coMove);
            //coMove = null;
        }
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
