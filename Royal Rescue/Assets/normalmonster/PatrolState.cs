using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class PatrolState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;

    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("Patrol", true);
        animator.SetBool("Chase", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Die", false);
        animator.SetBool("IsLive", true);

        monster.setSpeed(3f);
    }
    public void UpdateState()
    {
        monster.transform.position += new Vector3(monster.getSpeed() * monster.getFacingDir(), 0, 0) * Time.deltaTime;
        if (!monster.CheckGround(monster.transform.position, Vector3.down)
            || monster.CheckWall(monster.transform.position))
        {
            monster.FlipX();
        }
    }
    public void ExitState()
    {
        animator.SetBool("Patrol", false);
    }
}
