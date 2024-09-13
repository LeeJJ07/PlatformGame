using UnityEngine;

public class DieNode : INode
{
    public delegate void DeActiveObj();
    public delegate void BossDie();
    DeActiveObj deActiveSpawnObj;
    BossDie bossDie;
    Transform transform;
    Transform target;
    Animator aniController;

    float animationDuration = 100f;
    float keepDieStateTime = 5f;
    float time = 0;
    bool isActiveAnime = false;
    public bool IsActiveAnime => isActiveAnime;
    
    public DieNode(DeActiveObj deActiveObj, BossDie bossDie, Transform transform, Transform target, Animator aniController)
    {
        this.deActiveSpawnObj = deActiveObj;
        this.bossDie = bossDie;
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
    }
    public void AddNode(INode node) {}

    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        time += Time.deltaTime;
        Debug.Log("Die Running");
        if (time>animationDuration)
        {
            Debug.Log("Die Success");
            deActiveSpawnObj();
            animationDuration = 0;
            isActiveAnime = false;
            bossDie();
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("DieTrigger");
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            deActiveSpawnObj();
            animationDuration = aniController.GetCurrentAnimatorStateInfo(0).length + keepDieStateTime;
            Vector3 dir = target.position - transform.position;
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);


            isActiveAnime = true;
        }
    }
}
