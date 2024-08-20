using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPhase2Node : INode
{
    EnemyAI enemyAI;
    float time = 0;
    float animationDuration = 100;
    bool isActiveAnime = false;
    public EntryPhase2Node(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI; 
    }

    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        Debug.Log("entryPhase2 Running");
        time += Time.deltaTime;
        ActiveAnimation();
        Collider[] colliders = Physics.OverlapSphere(enemyAI.transform.position, 15,LayerMask.GetMask("Player"));
        if(colliders!=null)
        {
            
            foreach(Collider collider in colliders)
            {
                Vector3 dir = collider.transform.position + enemyAI.transform.position;
                collider.GetComponent<Rigidbody>().AddForce(dir.normalized, ForceMode.Impulse);
            }
        }
        if (time > animationDuration) 
        {
            Debug.Log("entryPhase2 Success");
            time = 0;
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
