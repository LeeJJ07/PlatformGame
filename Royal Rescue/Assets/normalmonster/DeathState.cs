using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;

    private float span = 0f;
    private float deactivateTime = 2.8f;
    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("isDie", true);
    }
    public void UpdateState()
    {
        span += Time.deltaTime;
        if (span > deactivateTime){
            animator.SetBool("isLive", false);
            ExitState();
        }
    }
    public void ExitState()
    {
        monster.gameObject.SetActive(false);
    }
}
