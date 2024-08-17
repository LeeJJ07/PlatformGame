using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAttackNode : INode
{
    bool isActiveAnime = false;
    float SkillDuration = 0;
    float time = 0;
    EnemyAI enemyAI;
    Ray ray;
    RaycastHit hit;
    public RushAttackNode(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        time += Time.deltaTime;
        if (time >= SkillDuration) 
        {
            enemyAI.EnemyAnimation.SetBool("isRun", false);
            time = 0;
            isActiveAnime = false;
            return INode.NodeState.Success;
        }

        ray = new Ray(enemyAI.transform.position, enemyAI.transform.position + Vector3.left * 5);
        Physics.Raycast(ray, out hit);
        if (hit.collider.tag.Equals("wall"))
        {
            Vector3 rot=new Vector3(0,enemyAI.transform.rotation.y*-1,0);
            enemyAI.transform.rotation = Quaternion.Euler(rot);
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        enemyAI.EnemyAnimation.SetBool("isWalk", true);
        isActiveAnime = true;
    }
}
