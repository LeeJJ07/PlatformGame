using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    protected Animator animator;
    protected Monster monster;

    protected float curDirX;
    protected float curRotY;
    [SerializeField] protected GameObject hitPoint;
    
    public virtual void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("isAttack", true);
        hitPoint.SetActive(true);
    }
    public virtual void UpdateState()
    {
        curDirX = monster.getDirectionPlayer();

        monster.transform.rotation = Quaternion.Euler(0, 180f - 90f * curDirX, 0);
    }
    public virtual void ExitState()
    {
        hitPoint.SetActive(false);
        animator.SetBool("isAttack", false);

        curRotY = 195f - curDirX * 75f;
        monster.transform.rotation = Quaternion.Euler(0, curRotY, 0);
    }
}
