using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    [SerializeField] protected NormalMonsterData data;
    protected Animator animator;
    protected Monster monster;

    protected float curDirX;
    protected float curRotY;
    [SerializeField] protected GameObject hitPoint;
    [SerializeField] protected float AttackSoundsync;
    protected bool isRunningCorouting = false;

    public virtual void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();
        
        Debug.Log("AttackState");
        animator.SetBool("isAttack", true);
        hitPoint.SetActive(true);
        
    }
    public virtual void UpdateState()
    {
        curDirX = monster.getDirectionPlayerX();
        Debug.Log("AttackState Running");
        monster.transform.rotation = Quaternion.Euler(0, 180f - 90f * curDirX, 0);
        if (!isRunningCorouting)
        {
            isRunningCorouting = true;
            StartCoroutine("StartSoundEffect");
        }
    }
    public virtual void ExitState()
    {
        hitPoint.SetActive(false);
        animator.SetBool("isAttack", false);

        curRotY = 195f - curDirX * 75f;
        monster.transform.rotation = Quaternion.Euler(0, curRotY, 0);
    }

    IEnumerator StartSoundEffect()
    {
        float span = 0;
        float aniDuration= 0;
        bool isStartSound = false;

        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                aniDuration = animator.GetCurrentAnimatorStateInfo(0).length;
                break;
            }
            yield return null;
        }
        while (true)
        {
            span += Time.deltaTime;
            if (span > aniDuration)
            {
                isRunningCorouting = false;
                isStartSound = false;
                span = 0;
                yield break;
            }
            else if(span>AttackSoundsync && !isStartSound)
            {
                SoundManager.Instance.PlaySound(data.AttackSound);
                isStartSound = true;
            }
            yield return null;
        }
    }
    
}
