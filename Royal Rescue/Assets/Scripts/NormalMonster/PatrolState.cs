using System.Collections;
using UnityEditor;
using UnityEngine;

public class PatrolState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;
    [SerializeField] protected NormalMonsterData data;

    bool isActiveSound = false;

    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("isPatrol", true);

        if (!isActiveSound)
        {
            isActiveSound = true;
            StartCoroutine(StartSoundEffect());
        }
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
        isActiveSound = false;
    }
    IEnumerator StartSoundEffect()
    {
        float soundDelay = 0;
        while(true)
        {
            Debug.Log($"TransitionDelay");

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Patrol"))
            {
                soundDelay = animator.GetCurrentAnimatorStateInfo(0).length;
                break;
            }
            yield return null;
        }
        while (true) 
        {
            Debug.Log("PlaySound!");

            if (Physics.Raycast(transform.position, Vector3.down, 2f, LayerMask.GetMask("Ground")))
            {
                if(!isActiveSound) yield break;
                SoundManager.Instance.PlaySound(data.PatrolSound);
                yield return new WaitForSeconds(soundDelay);
            }
            yield return null;
        }
    }
}
