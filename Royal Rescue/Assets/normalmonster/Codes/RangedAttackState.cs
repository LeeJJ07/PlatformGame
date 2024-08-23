using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float attackSpeed = 0.5f;
    float afterShootTime = 0f;
    new void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<RangedMonster>();

        animator.SetBool("isAttack", true);
        afterShootTime = 0f;
    }
    new void UpdateState()
    {
        curDirX = monster.getDirectionPlayerX();
        monster.transform.rotation = Quaternion.Euler(0, 180f - 90f * curDirX, 0);

        afterShootTime += Time.deltaTime;
        if(afterShootTime > attackSpeed)
        {
            afterShootTime = 0;
            Instantiate(projectile, monster.transform);
        }
    }
    new void ExitState()
    {
        animator.SetBool("isAttack", false);

        curRotY = 195f - curDirX * 75f;
        monster.transform.rotation = Quaternion.Euler(0, curRotY, 0);
    }
}
