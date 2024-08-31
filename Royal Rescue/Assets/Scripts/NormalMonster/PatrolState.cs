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

        animator.SetBool("isPatrol", true);
    }
    public void UpdateState()
    {
        monster.transform.position += new Vector3(monster.getWalkSpeed() * monster.getFacingDir(), 0f, 0f) * Time.deltaTime;
        if (!monster.CheckGround(monster.transform.position, Vector3.down, monster.getToGroundDistance())
            || monster.CheckWall(monster.transform.position, new Vector3(monster.facingDir, 0f, 0f)))
        {
            monster.FlipX();
        }
    }
    public void ExitState()
    {
        animator.SetBool("isPatrol", false);
    }
}
