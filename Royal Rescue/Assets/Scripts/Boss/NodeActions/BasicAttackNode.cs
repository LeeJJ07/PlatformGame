using UnityEngine;

public class BasicAttackNode : INode
{
    BasicAttackScriptableObject basicAttackInfo;
    Animator aniController;
    Transform transform;
    Transform target;
    Transform attackPosi;
    float animationDuration = 100;
    float attackTime = 0;
    float time = 0;
    bool isActiveAnime = false;
    bool isAttackPlayer = false;

    public BasicAttackNode(BasicAttackScriptableObject basicAttackInfo, Animator aniController,Transform attackPosi ,Transform transform, Transform target)
    {
        this.basicAttackInfo = basicAttackInfo;
        this.aniController = aniController;
        this.attackPosi = attackPosi;
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
        if(time > attackTime&& !isAttackPlayer)
        {
            Collider[] hitCollider = Physics.OverlapBox(attackPosi.position, new Vector3(5, 5, 1), Quaternion.identity,LayerMask.GetMask("Player"));
            if(hitCollider!=null)
            {
                foreach(Collider hit in hitCollider)
                {
                    if(hit.CompareTag("Player"))
                        hit.GetComponent<PlayerControlManagerFix>().HurtPlayer(basicAttackInfo.damage);
                }
                
                isAttackPlayer = true;
            }
        }
        if(time>=animationDuration)
        {
            time = 0;
            Debug.Log("BasicAttack Success");
            isActiveAnime = false;
            isAttackPlayer = false;
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
            dir.z = 0;
            dir.y = 0;
            attackTime = animationDuration / 2f;
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);


            isActiveAnime = true;
        }
    }
}
