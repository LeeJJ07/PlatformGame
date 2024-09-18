using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;

    [SerializeField] protected NormalMonsterData data;
    [SerializeField] GameObject exclamation;

    float flowTime = 0f;
    float detectMontionTime = 1f;

    bool isActiveSound = false;

    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("isChase", true);

        flowTime = 0f;
        if (monster.isDetect)
        {
            exclamation.transform.rotation = Quaternion.Euler(0f, monster.getFacingDir() * -50f, 0f);
            exclamation.SetActive(true);
        }
        if (!monster.LookPlayer())
            monster.FlipX();
        if(!isActiveSound)
        {
            isActiveSound = true;
            StartCoroutine("StartSoundEffect");
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
        if (monster.CheckWall(monster.transform.position, new Vector3(monster.facingDir, 0f, 0f)))
            return;
        if (!monster.CheckGround(monster.transform.position, Vector3.down))
            return;

        if (!monster.LookPlayer())
            monster.FlipX();

        monster.transform.position += new Vector3(monster.getRunSpeed() * monster.getFacingDir(), 0, 0) * Time.deltaTime;
    }
    public void ExitState()
    {
        animator.SetBool("isChase", false);
        exclamation.SetActive(false);
        StopCoroutine("StartSoundEffect");
        isActiveSound = false;
    }

    IEnumerator StartSoundEffect()
    {
        SoundManager.Instance.StopLoopSound(data.PatrolSound);
        SoundManager.Instance.StopLoopSound(data.ChaseSound);
        float soundDelay = 0;
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
            {
                soundDelay = animator.GetCurrentAnimatorStateInfo(0).length;
                break;
            }
            yield return null;
        }
        while (true)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 2f, LayerMask.GetMask("Ground")))
            {
                SoundManager.Instance.PlaySound(data.PatrolSound);
                yield return new WaitForSeconds(soundDelay);
            }
            yield return null;
        }
    }

}
