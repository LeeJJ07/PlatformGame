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

    Ray ray;
    RaycastHit hit;
    float skillSpan = 0;

    bool isActiveAnime = false;
    
    public RushAttackNode(RushAttackScriptableObject rushAttackInfo, SpawnFunction func ,Animator aniController, Transform transform,Transform target)
    {
        this.rushAttackInfo = rushAttackInfo;
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
        spawnRocks = func;
    }
    public void AddNode(INode node) { }


    //�̽�: �߸��� ����ĳ��Ʈ ��ġ�� ���� ���� ����ϴ� ��ó�� ����
    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        Debug.Log("RushAttack Running");
        skillSpan += Time.deltaTime;

        //��ų ����
        if (skillSpan >= rushAttackInfo.RushAttackDuration) 
        {
            aniController.SetBool("isRushAttack", false);
            isActiveAnime = false;
            skillSpan = 0;
            return INode.NodeState.Success;
        }


        ray = new Ray((transform.position + new Vector3(7, 3, 0)) * transform.forward.x, (transform.position + new Vector3(10, 3, 0)) * transform.forward.x);
        transform.position += transform.forward * rushAttackInfo.rushSpeed * Time.deltaTime;
        Collider[] colliders =  Physics.OverlapSphere(transform.position, 5, rushAttackInfo.playerLayer); 
        Physics.Raycast(ray,out hit, 5, rushAttackInfo.WallLayer);
        

        //�巡�� ȸ��(���� ����� ��)
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
        //�÷��̾� �˹�
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
                    //collider.GetComponent<PlayerControlManagerFix>();
                    Rigidbody rigid = collider.GetComponent<Rigidbody>();
                    Debug.Log($"rigid: {rigid == null}");
                    if (rigid != null)
                    {
                        rigid.AddForce(dir.normalized/2.35f, ForceMode.Impulse);
                    }
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
            isActiveAnime = true;
        }
    }
}
