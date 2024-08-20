using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvent : MonoBehaviour
{
    
    [SerializeField] Transform flameSpawnPoint;
    [SerializeField] Transform target;
    public void ActiveSpawnFlame(GameObject flamePrefab)
    {
        GameObject flameGM = Instantiate(flamePrefab, flameSpawnPoint.position, transform.rotation);
        flameGM.GetComponent<FlameBehavior>().SetTarget(target);
        flameGM.GetComponent<FlameBehavior>().ActiveSkill();
    }
}
