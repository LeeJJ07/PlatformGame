using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossBaseAttack : INode
{
    Transform transform;
    Transform playerTransform;
    Animator animator;
    float time = 0f;
    public MiniBossBaseAttack(Transform transform, Transform playerTransform, Animator animator)
    {
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.animator = animator;
    }

    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        float dirX = (playerTransform.position - transform.position).x;
        if (time > 1f)
        {
            time = 0f;
            transform.GetChild(2).gameObject.SetActive(false);
            transform.eulerAngles = new Vector3(0f, 180f - 60 * dirX / Mathf.Abs(dirX), 0f);
            return INode.NodeState.Success;
        }
        if (time == 0f)
        {
            animator.SetTrigger("baseAttack");
            transform.eulerAngles = new Vector3(0f, 180f - 90 * dirX / Mathf.Abs(dirX), 0f);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        time += Time.deltaTime;
        return INode.NodeState.Running;
    }
}
