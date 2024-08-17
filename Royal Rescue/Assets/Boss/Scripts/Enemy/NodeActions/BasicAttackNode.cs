using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackNode : INode
{
    EnemyAI enemyAI;
    float animationDuration = 100;
    float time = 0;
    bool isActiveAnime = false;
    public BasicAttackNode(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!enemyAI)
        {
            Debug.Log("BasicAttack Failure");
            return INode.NodeState.Failure;
        }
        Debug.Log("BasicAttack Running");
        ActiveAnimation();
        time += Time.deltaTime;
        if(time>=animationDuration)
        {
            time = 0;
            Debug.Log("BasicAttack Success");
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        enemyAI.EnemyAnimation.SetTrigger("BasicAttackTrigger");
        if (enemyAI.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack"))
        {
            animationDuration = enemyAI.EnemyAnimation.GetCurrentAnimatorStateInfo(0).length;
            Vector3 dir = enemyAI.Target.position - enemyAI.transform.position;
            if (dir.normalized.x < 0)
                enemyAI.transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                enemyAI.transform.rotation = Quaternion.Euler(0, 90, 0);


            isActiveAnime = true;
        }
    }
}
