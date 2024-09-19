using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniBossSkill1Attack : INode
{
    public delegate float Hp();
    Hp hp;

    Transform transform;
    Transform playerTransform;
    Animator animator;
    float runSpeed;
    bool isAttack = false;
    bool delay = false;
    float time = 0f;

    public MiniBossSkill1Attack(Transform transform, Transform playerTransform, Animator animator, float runSpeed, Hp hp)
    {
        this.hp = hp;
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.animator = animator;
        this.runSpeed = runSpeed;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (hp() <= 0)
            return INode.NodeState.Success;

        float dirX = (playerTransform.position - transform.position).x;
        if (!isAttack)
        {
            isAttack = true;
            animator.SetTrigger("run");
        }
        if (delay || (Mathf.Abs(dirX) < 5f))
        {
            if (time == 0f)
            {
                transform.eulerAngles = new Vector3(0f, 180f - 90 * dirX / Mathf.Abs(dirX), 0f);
                animator.SetTrigger("skill1");
                
            }else if (time > 0.6f && !transform.GetChild(3).gameObject.activeSelf) {
                transform.GetChild(3).gameObject.SetActive(true);
            }
            delay = true;
            time += Time.deltaTime;

            if (time > 1f)
            {
                delay = false;
                isAttack = false;
                time = 0f;
                transform.GetChild(3).gameObject.SetActive(false);
                transform.eulerAngles = new Vector3(0f, 180f - 60 * dirX / Mathf.Abs(dirX), 0f);
                return INode.NodeState.Success;
            }
        }
        else if (Mathf.Abs(dirX) >= 5f)
            transform.position += new Vector3(dirX * runSpeed / 5 * Time.deltaTime, 0f, 0f);

        return INode.NodeState.Running;
    }
}
