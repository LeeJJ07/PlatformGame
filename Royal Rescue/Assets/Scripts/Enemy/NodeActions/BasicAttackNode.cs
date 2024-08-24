using UnityEngine;

public class BasicAttackNode : INode
{
    BasicAttackScriptableObject basicAttackInfo;
    Animator aniController;
    Transform transform;
    Transform target;
    float animationDuration = 100;
    float time = 0;
    bool isActiveAnime = false;

    public BasicAttackNode(BasicAttackScriptableObject basicAttackInfo, Animator aniController, Transform transform, Transform target)
    {
        this.basicAttackInfo = basicAttackInfo;
        this.aniController = aniController;
        this.transform = transform;
        this.target = target;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!basicAttackInfo)
        {
            Debug.Log("BasicAttack Failure");
            return INode.NodeState.Failure;
        }
        ActiveAnimation();
        time += Time.deltaTime;
        if(time>=animationDuration)
        {
            time = 0;
            Debug.Log("BasicAttack Success");
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("BasicAttackTrigger");
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack"))
        {
            animationDuration = aniController.GetCurrentAnimatorStateInfo(0).length;
            Vector3 dir = target.position - transform.position;
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);


            isActiveAnime = true;
        }
    }
}
