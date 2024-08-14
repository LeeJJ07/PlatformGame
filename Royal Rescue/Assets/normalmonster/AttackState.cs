using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;

    private float curDirX;
    [SerializeField] GameObject hitPoint;
    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        curDirX = monster.transform.eulerAngles.y;
        monster.transform.rotation = Quaternion.Euler(0, curDirX + (curDirX - 180f)/2, 0);

        animator.SetBool("isAttack", true);
        hitPoint.SetActive(true);
    }
    public void UpdateState()
    {

    }
    public void ExitState()
    {
        hitPoint.SetActive(false);
        animator.SetBool("isAttack", false);
        monster.transform.rotation = Quaternion.Euler(0, curDirX, 0);
    }
}
