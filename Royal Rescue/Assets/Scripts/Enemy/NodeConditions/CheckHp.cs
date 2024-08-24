using System.Diagnostics;

public class CheckHp : INode
{
    public delegate float Hp();
    Hp hp;
    EnemyAI enemyAI;
    float hpSection1;
    float hpSectino2;
    /// <summary>
    /// Hp���� �����Ͽ� ���� ��带 �����Ű�� ���ǳ��
    /// </summary>
    /// <param name="enemyAI">Enemy�� ������ ������ ����</param>
    /// <param name="hpSection1">Hp������ �ִ밪</param>
    /// <param name="hpSectino2">Hp������ �ּҰ�</param>
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
