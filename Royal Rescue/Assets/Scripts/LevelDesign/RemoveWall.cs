using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWall : MonoBehaviour
{
    [SerializeField] private GameObject gem;

    // Update is called once per frame
    void Update()
    {
        if (gem.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
    }
}
