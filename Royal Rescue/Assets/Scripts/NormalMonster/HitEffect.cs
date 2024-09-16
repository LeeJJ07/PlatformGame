using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] bool isDestroyafterEnd = true;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        OnDie();
    }
    void OnDie()
    {
        Debug.Log($"isDestroyafterEnd: {isDestroyafterEnd}");
        if(isDestroyafterEnd)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
