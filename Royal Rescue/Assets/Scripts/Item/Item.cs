using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Rigidbody rigid;
    void Start()
    {
        rigid= GetComponent<Rigidbody>();

        rigid.AddForce(new Vector3(0, 1000f, 0));
    }
}
