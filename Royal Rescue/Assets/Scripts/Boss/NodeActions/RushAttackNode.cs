using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RushAttackNode : INode
{
    public delegate void SpawnFunction(GameObject[] objs,int spawnCount);
    SpawnFunction spawnRocks;

    RushAttackScriptableObject rushAttackInfo;
    Animator aniController;

    Transform transform;
    Transform target;
    Collider[] bossColliders;

    Ray ray;
    RaycastHit hit;
    float skillSpan = 0;

    bool isActiveAnime = false;
    
    public RushAttackNode(RushAttackScriptableObject rushAttackInfo,Collider[] bossColliders ,SpawnFunction func ,Animator aniController, Transform transform,Transform target)
    {
        this.rushAttackInfo = rushAttackInfo;
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
        spawnRocks = func;
        this.bossColliders = bossColliders;
    }
    public void AddNode(INode node) { }


    //이슈: 잘못된 레이캐스트 위치로 인해 벽을 통과하는 것처럼 보임
    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        Debug.Log("RushAttack Running");
        skillSpan += Time.deltaTime;

        //스킬 종료
        if (skillSpan >= rushAttackInfo.RushAttackDuration) 
        {
            aniController.SetBool("isRushAttack", false);
            foreach (Collider bossCollider in bossColliders)
                bossCollider.enabled = true;
            isActiveAnime = false;
            skillSpan = 0;
            return INode.NodeState.Success;
        }


        ray = new Ray((transform.position + new Vector3(7, 3, 0)) * transform.forward.x, (transform.position + new Vector3(10, 3, 0)) * transform.forward.x);
        transform.position += transform.forward * rushAttackInfo.rushSpeed * Time.deltaTime;
        Collider[] colliders =  Physics.OverlapSphere(transform.position, 5, rushAttackInfo.playerLayer); 
        Physics.Raycast(ray,out hit, 5, rushAttackInfo.WallLayer);
        

        //드래곤 회전(벽에 닿았을 시)
        if(hit.collider != null)
        {
            if (hit.collider.tag.Equals(rushAttackInfo.wallTag))
            {
                Quaternion curRot = transform.rotation;
                curRot.y += 180;
                transform.Rotate(new Vector3(0, -curRot.y, 0));

                spawnRocks(rushAttackInfo.objs, 5);
                ray = new Ray((transform.position + new Vector3(7, 3, 0)) * transform.forward.x, (transform.position + new Vector3(10, 3, 0)) * transform.forward.x);
            }
        }
        //플레이어 데미지
        if(colliders!= null)
        {
            foreach(Collider collider in colliders)
            {
                if(collider.tag.Equals(rushAttackInfo.playerTag))
                {
                    Vector3 dir = collider.transform.position + transform.position;
                    Mathf.Clamp(dir.x, 0, 1);
                    Mathf.Clamp(dir.y, 0, 1);
                    dir.z = 0;

                    //collider.GetComponent<PlayerControlManagerFix>().hitdamage();
                    
                }
            }
        }
        

        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        Debug.Log("rushattackANimation");
        aniController.SetBool("isRushAttack",true);
        if(aniController.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            foreach(Collider bossCollider in bossColliders)
                bossCollider.enabled = false;
            isActiveAnime = true;
        }
    }
}
