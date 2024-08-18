using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;

    private float curDirX;
    private float curRotY;
    [SerializeField] GameObject hitPoint;
    
    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("isAttack", true);
        hitPoint.SetActive(true);
    }
    public void UpdateState()
    {
        float monsterX = monster.transform.position.x;
        float playerX = monster.player.transform.position.x;
        curDirX = (monsterX - playerX) /Mathf.Abs(monsterX - playerX);

        monster.transform.rotation = Quaternion.Euler(0, 180f + 90f * curDirX, 0);
    }
    public void ExitState()
    {
        hitPoint.SetActive(false);
        animator.SetBool("isAttack", false);

        curRotY = 195f + curDirX * 75f;
        monster.transform.rotation = Quaternion.Euler(0, curRotY, 0);
    }
}
