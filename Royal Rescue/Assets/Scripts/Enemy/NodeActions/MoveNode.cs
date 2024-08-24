using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class MoveNode : INode
{
    Transform transform;
    Transform target;
    Animator aniController;
    float moveSpeed = 0;
    float animationDuration = 100;
    public MoveNode(Transform transform, Transform target, Animator aniController,float moveSpeed)
    {
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
        this.moveSpeed = moveSpeed;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        Debug.Log("MoveState Running");
        ActiveAnimation();
        //float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.Target.position);
        float distance = XDistance(transform.position.x, target.position.x);
        

        Vector3 dir= target.position - transform.position;
        transform.position += new Vector3(dir.normalized.x,0,0) * moveSpeed * Time.deltaTime;
        if(dir.normalized.x<0)
            transform.rotation= Quaternion.Euler(0,-90,0);
        else
            transform.rotation = Quaternion.Euler(0, 90, 0);

        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        aniController.SetBool("isWalk",true);
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            animationDuration = aniController.GetCurrentAnimatorStateInfo(0).length;
        }
    }
    float XDistance(float x1, float x2)
    {
        return Mathf.Abs(x1-x2);
    }
}
