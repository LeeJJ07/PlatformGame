using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAttackState : AttackState
{
    enum Skill { ATTACK, POISON }
    Skill skill;

    [SerializeField] ParticleSystem poison;

    public override void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();
        
        skill = Skill.ATTACK;
    }
    public override void UpdateState()
    {
        float monsterX = monster.transform.position.x;
        float playerX = monster.player.transform.position.x;
        curDirX = (monsterX - playerX) / Mathf.Abs(monsterX - playerX);
        monster.transform.rotation = Quaternion.Euler(0, 180f + 90f * curDirX, 0);

        Debug.Log(monster.isAttack);

        if (monster.isAttack)
            return;
        monster.isAttack = true;

        if (Random.Range(0, 10) < 8)
            skill = Skill.ATTACK;
        else
            skill = Skill.POISON;

        StartCoroutine(Attack());
    }
    public override void ExitState()
    {
        curRotY = 195f + curDirX * 75f;
        monster.transform.rotation = Quaternion.Euler(0, curRotY, 0);

        animator.SetBool("isAttack", false);
        animator.SetBool("isAttack1", false);

    }
    IEnumerator Attack()
    {
        SetAttack();
        if (skill == Skill.POISON)
        {
            Debug.Log("poison is playing.");
            poison.Play();
        }
        float curAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(curAnimationTime);
        ExitAttack();
    }

    void SetAttack()
    {
        switch (skill)
        {
            case Skill.ATTACK:
                hitPoint.SetActive(true);
                //animator.SetBool("isAttack", true);
                animator.Play("Attack", -1, 0f);
                break;
            case Skill.POISON:
                //animator.SetBool("isAttack1", true);
                animator.Play("Attack1", -1, 0f);
                break;
        }
    }
    void ExitAttack()
    {
        switch (skill)
        {
            case Skill.ATTACK:
                animator.SetBool("isAttack", false);
                hitPoint.SetActive(false);
                break;
            case Skill.POISON:
                animator.SetBool("isAttack1", false);
                break;
        }
        monster.isAttack = false;
    }
}
