using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HitPointController : MonoBehaviour
{
    [SerializeField] Monster monster;
    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if(other.gameObject.tag == "Player")
        {
            Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * 5f, ForceMode.Impulse);


            int dmg = monster.getDamage() + Random.Range(0, 6);
            GameDirector.instance.PlayerControl.HurtPlayer(dmg);
        }
    }
}
