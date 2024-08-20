public class CheckHp : INode
{
    EnemyAI enemyAI;
    float hpSection1;
    float hpSectino2;
    /// <summary>
    /// Hp���� �����Ͽ� ���� ��带 �����Ű�� ���ǳ��
    /// </summary>
    /// <param name="enemyAI">Enemy�� ������ ������ ����</param>
    /// <param name="hpSection1">Hp������ �ִ밪</param>
    /// <param name="hpSectino2">Hp������ �ּҰ�</param>
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
