using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HitPointController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if(other.gameObject.tag == "Player")
        {
            Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * 5f, ForceMode.Impulse);
            Debug.Log("공격 성공");
        }
    }
}
