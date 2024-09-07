using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureOpen : MonoBehaviour
{
    public float destroyTime = 2f;
    void Start()
    {
        Invoke("DestroyParticle", destroyTime);
    }

    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
