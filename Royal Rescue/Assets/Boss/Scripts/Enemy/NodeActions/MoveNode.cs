using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : INode
{
    EnemyAI enemyAI;
    float animationDuration = 100;
    public MoveNode(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!enemyAI)
        {
            Debug.Log("MoveState Faiure");
            return INode.NodeState.Failure;
        }
        Debug.Log("MoveState Running");
        ActiveAnimation();
        //float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.Target.position);
        float distance = XDistance(enemyAI.transform.position.x, enemyAI.Target.position.x);
        
        if (distance <= enemyAI.AttackToTargetDistance)
        {
            Debug.Log("MoveState Success");
            return INode.NodeState.Success;
        }

        Vector3 dir= enemyAI.Target.position - enemyAI.transform.position;
        enemyAI.transform.position += new Vector3(dir.normalized.x,0,0) * enemyAI.Phase1MoveSpeed * Time.deltaTime;
        if(dir.normalized.x<0)
            enemyAI.transform.rotation= Quaternion.Euler(0,-90,0);
        else
            enemyAI.transform.rotation = Quaternion.Euler(0, 90, 0);

        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        enemyAI.EnemyAnimation.SetBool("isWalk",true);
        if (enemyAI.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            animationDuration = enemyAI.EnemyAnimation.GetCurrentAnimatorStateInfo(0).length;
        }
    }
    float XDistance(float x1, float x2)
    {
        return Mathf.Abs(x1-x2);
    }
}
