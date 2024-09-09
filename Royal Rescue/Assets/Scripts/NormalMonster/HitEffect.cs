using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        OnDie();
    }
    void OnDie()
    {
        Destroy(gameObject);
    }
}
