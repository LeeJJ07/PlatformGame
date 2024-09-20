using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float attackSpeed = 1.5f;
    float afterShootTime = 0f;
    public override void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<RangedMonster>();

        animator.SetBool("isAttack", true);
        afterShootTime = 0f;
        AttackSoundsync = attackSpeed;
    }
    public override void UpdateState()
    {
        curDirX = monster.getDirectionPlayerX();
        monster.transform.rotation = Quaternion.Euler(0, 180f - 90f * curDirX, 0);

        afterShootTime += Time.deltaTime;
        if (afterShootTime > attackSpeed)
        {
            afterShootTime = 0;
            SoundManager.Instance.PlaySound(data.AttackSound);
            Instantiate(projectile, monster.transform.position + new Vector3(0f, 1.3f, 0f), monster.transform.rotation);
        }
    }
    public override void ExitState()
    {
        animator.SetBool("isAttack", false);

        curRotY = 195f - curDirX * 75f;
        monster.transform.rotation = Quaternion.Euler(0, curRotY, 0);
    }
}
