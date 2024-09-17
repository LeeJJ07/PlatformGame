using System.Collections;
using UnityEngine;

public class PatrolState : MonoBehaviour, IState
{
    private Animator animator;
    private Monster monster;
    [SerializeField] protected NormalMonsterData data;

    public void EnterState()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!monster) monster = GetComponent<Monster>();

        animator.SetBool("isPatrol", true);
        StartCoroutine(StartSoundEffect());
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
    IEnumerator StartSoundEffect()
    {
        while (true) 
        {
            if(Physics.Raycast(transform.position, Vector3.down, 2f, LayerMask.GetMask("Ground")))
            {
                SoundManager.Instance.StopLoopSound(data.PatrolSound);
                SoundManager.Instance.StopLoopSound(data.ChaseSound);
                SoundManager.Instance.PlaySound(data.PatrolSound, true);
                yield break;
            }
            yield return null;
        }
    }
}
