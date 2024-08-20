public class CheckHp : INode
{
    EnemyAI enemyAI;
    float hpSection1;
    float hpSectino2;
    /// <summary>
    /// Hp구간 설정하여 다음 노드를 실행시키는 조건노드
    /// </summary>
    /// <param name="enemyAI">Enemy의 정보를 가져올 변수</param>
    /// <param name="hpSection1">Hp조건의 최대값</param>
    /// <param name="hpSectino2">Hp조건의 최소값</param>
    public CheckHp(EnemyAI enemyAI, float hpSection1,float hpSectino2)
    {
        this.enemyAI = enemyAI;
        this.hpSection1 = hpSection1;
        this.hpSectino2 = hpSectino2;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (enemyAI.Hp <= hpSection1 && enemyAI.Hp >= hpSectino2) 
            return INode.NodeState.Success;
        else
            return INode.NodeState.Failure;
    }
}
