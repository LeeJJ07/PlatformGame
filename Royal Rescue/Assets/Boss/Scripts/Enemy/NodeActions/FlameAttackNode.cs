using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class FlameAttackNode : INode
{
    EnemyAI enemyAI;
    float phaseHpCondition = 0;
    float animationDuration = 100;
    float skillActiveTime = 0;
    bool isActiveAnime = false;

    string name = "";
    public FlameAttackNode(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }
    public FlameAttackNode(EnemyAI enemyAI, float phaseHpCondition, string name)
    {
        this.enemyAI = enemyAI;
        this.phaseHpCondition = phaseHpCondition;
        this.name = name;
    }
    public FlameAttackNode(EnemyAI enemyAI, float phaseHpCondition)
    {
        this.enemyAI = enemyAI;
        this.phaseHpCondition = phaseHpCondition;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!enemyAI || enemyAI.Hp <= phaseHpCondition) 
        {
            Debug.Log("FlameAttack Failure");
            return INode.NodeState.Failure;
        }
        Debug.Log(name);
        skillActiveTime += Time.deltaTime;
        ActiveAnimation();
        if (skillActiveTime >= animationDuration)
        {
            Debug.Log("FlameAttack Success");
            
            isActiveAnime = false;
            skillActiveTime = 0;
            return INode.NodeState.Success;
        }

        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        enemyAI.EnemyAnimation.SetTrigger("FlameAttackTrigger");
        if (enemyAI.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Flame Attack"))
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
