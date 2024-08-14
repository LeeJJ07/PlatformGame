using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;
    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("Attack", true);
    }
    public void UpdateState()
    {

    }
    public void ExitState()
    {
        animator.SetBool("Attack", false);
    }
}
