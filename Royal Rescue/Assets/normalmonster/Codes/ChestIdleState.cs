using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestIdleState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;

    [SerializeField] GameObject exclamation;
    [SerializeField] ParticleSystem poisonParticle;
    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("isPatrol", false);
        animator.SetBool("isChase", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isDie", false);
        animator.SetBool("isLive", false);

        monster.GetComponent<Collider>().enabled = false;
    }
    public void UpdateState()
    {
    }
    public void ExitState()
    {
        StartCoroutine(ActiveExclamation());
        monster.transform.rotation = Quaternion.Euler(0f, 180f - monster.getFacingDir() * 60f, 0f);
        monster.GetComponent<Collider>().enabled = true;
        animator.SetBool("isLive", true);
        poisonParticle.Play();
    }
    IEnumerator ActiveExclamation()
    {
        exclamation.transform.rotation = Quaternion.Euler(0f, monster.getFacingDir() * -50f, 0f);
        exclamation.SetActive(true);
        yield return new WaitForSeconds(1f);
        exclamation.SetActive(false);
    }
}