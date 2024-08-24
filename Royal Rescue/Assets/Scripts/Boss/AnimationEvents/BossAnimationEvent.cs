using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvent : MonoBehaviour
{
    
    [SerializeField] Transform flameSpawnPoint;
    [SerializeField] Transform target;
    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    public void ActiveSpawnFlame(GameObject flamePrefab)
    {
        GameObject flameGM = Instantiate(flamePrefab, flameSpawnPoint.position, transform.rotation);
        flameGM.GetComponent<FlameBehavior>().SetTarget(target);
        flameGM.GetComponent<FlameBehavior>().ActiveSkill();
    }
}
