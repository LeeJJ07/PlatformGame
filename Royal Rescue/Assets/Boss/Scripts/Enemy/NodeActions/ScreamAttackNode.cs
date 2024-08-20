using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreamAttackNode : INode
{
    EnemyAI enemyAI;
    float phaseHpCondition = 0;
    float animationDuration = 100;
    float time = 0;
    bool isActiveAnime = false;
    int randomSpawnIndex = 0;
    int spawnCount = 0;

    string name = "";
    public ScreamAttackNode(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }
    public ScreamAttackNode(EnemyAI enemyAI,float phaseHpCondition)
    {
        this.enemyAI = enemyAI;
        this.phaseHpCondition = phaseHpCondition;
    }
    public ScreamAttackNode(EnemyAI enemyAI, float phaseHpCondition,string name)
    {
        this.enemyAI = enemyAI;
        this.phaseHpCondition = phaseHpCondition;
        this.name = name;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (enemyAI.Hp <= phaseHpCondition) 
            return INode.NodeState.Failure;
        Debug.Log(name);
        
        ActiveAnimation();
        objectSpawn();
        time += Time.deltaTime;
        if (time >= animationDuration)
        {
            Debug.Log("Scream Success");

            time = 0;
            spawnCount = 0;
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void objectSpawn()
    {
        if (spawnCount >= enemyAI.EnemySpawnCount) return;
        randomSpawnIndex = Random.Range(0, enemyAI.ObjectPrefabs.Count);
        enemyAI.SpawnEnemy(randomSpawnIndex);
        spawnCount++;
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
