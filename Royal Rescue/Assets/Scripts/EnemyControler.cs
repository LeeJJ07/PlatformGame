using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("weapon"))
        {
            Debug.Log("몬스터피격");
        }
    }
}
