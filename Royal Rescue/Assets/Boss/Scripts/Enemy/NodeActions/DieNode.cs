using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieNode : INode
{
    EnemyAI enemyAI;
    float animationDuration = 100;
    float time = 0;
    bool isActiveAnime = false;
    
    public DieNode(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        Debug.Log("test");
        if(!enemyAI)
        {
            Debug.Log("Die Failure");
            return INode.NodeState.Failure;
        }
        ActiveAnimation();
        time += Time.deltaTime;
        Debug.Log("Die Running");
        if (time>animationDuration)
        {
            Debug.Log("Die Success");
            animationDuration = 0;
            isActiveAnime = false;
            enemyAI.DeActivateObj();
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        enemyAI.EnemyAnimation.SetTrigger("DieTrigger");
        if (enemyAI.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Die"))
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
