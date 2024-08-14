using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;

    [SerializeField] GameObject exclamation;

    float flowTime = 0f;
    float detectMontionTime = 1f;

    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("Chase", true);

        monster.setSpeed(5f);
        flowTime = 0f;
        if (monster.isDetect)
        {
            exclamation.transform.rotation = Quaternion.Euler(0f, monster.getFacingDir() * -50f, 0f);
            exclamation.SetActive(true);
        }
    }
    public void UpdateState()
    {
        if (monster.isDetect)
        {
            if (flowTime < detectMontionTime)
            {
                flowTime += Time.deltaTime;
                return;
            }
            if (exclamation.activeSelf)
                exclamation.SetActive(false);
        }
        if (!monster.CheckGround(monster.transform.position, Vector3.down))
            return;

        if (!LookPlayer())
            monster.FlipX();

        monster.transform.position += new Vector3(monster.getSpeed() * monster.getFacingDir(), 0, 0) * Time.deltaTime;
    }
    public void ExitState()
    {
        animator.SetBool("Chase", false);
        exclamation.SetActive(false);
    }

    bool LookPlayer()
    {
        Vector3 direction = monster.player.transform.position - monster.transform.position;
        if (monster.getFacingDir() > 0 && direction.x < 0)
            return false;
        if (monster.getFacingDir() < 0 && direction.x >= 0)
            return false;
        return true;
    }
}
