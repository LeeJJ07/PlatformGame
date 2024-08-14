using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if(other.gameObject.tag == "Player")
        {
            Debug.Log("공격 성공");
        }
    }
}
