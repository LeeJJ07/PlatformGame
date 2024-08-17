using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ScreamAttackNode : INode
{
    EnemyAI enemyAI;
    float animationDuration = 0;
    float time = 0;
    bool isActiveAnime = false;
    public ScreamAttackNode(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if(!enemyAI)
        {
            Debug.Log("Scream Failure");
            return INode.NodeState.Failure;
        }
        Debug.Log("Scream Running");
        ActiveAnimation();
        time += Time.deltaTime;
        if (time >= animationDuration)
        {
            time = 0;
            Debug.Log("Scream Success");
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        enemyAI.EnemyAnimation.SetTrigger("ScreamTrigger");
        if (enemyAI.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
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
