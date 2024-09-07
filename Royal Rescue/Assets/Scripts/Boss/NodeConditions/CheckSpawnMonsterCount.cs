using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpawnMonsterCount : INode
{
    public delegate int GetMonsterSpawnCount();
    GetMonsterSpawnCount spawnCount;
    int maxCount = 0;
    int skillSpawnCount = 0; //스킬사용에 스폰되는 몬스터 양
    public CheckSpawnMonsterCount(GetMonsterSpawnCount spawnCount, int maxCount, int skillSpawnCount)
    {
        this.spawnCount = spawnCount;
        this.maxCount = maxCount;
        this.skillSpawnCount = skillSpawnCount;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (maxCount >= spawnCount() + skillSpawnCount) 
        {
            return INode.NodeState.Success;
        }
        return INode.NodeState.Failure;
    }
}
