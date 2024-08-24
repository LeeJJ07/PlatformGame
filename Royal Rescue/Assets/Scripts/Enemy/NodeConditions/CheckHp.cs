using System.Diagnostics;

public class CheckHp : INode
{
    public delegate float Hp();
    Hp hp;
    EnemyAI enemyAI;
    float hpSection1;
    float hpSectino2;
    /// <summary>
    /// Hp구간 설정하여 다음 노드를 실행시키는 조건노드
    /// </summary>
    /// <param name="enemyAI">Enemy의 정보를 가져올 변수</param>
    /// <param name="hpSection1">Hp조건의 최대값</param>
    /// <param name="hpSectino2">Hp조건의 최소값</param>
    public CheckHp(Hp hp, float hpSection1,float hpSectino2)
    {
        this.hp = hp;
        this.hpSection1 = hpSection1;
        this.hpSectino2 = hpSectino2;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (hp() <= hpSection1 && hp() > hpSectino2)
            return INode.NodeState.Success;
        else
            return INode.NodeState.Failure;
    }
}
