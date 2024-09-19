using UnityEngine;

public class RushAttackNode : INode
{
    //낙하물 생성 함수
    public delegate void SpawnFunction(GameObject[] objs,int spawnCount); 
    public delegate void PlaySound(string name, bool isLoop);

    SpawnFunction spawnRocks;
    PlaySound playSound;
    RushAttackScriptableObject rushAttackInfo;
    Animator aniController;

    Transform transform;
    Transform target;
    Collider[] bossColliders;

    float skillSpan = 0;
    float soundSpan = 0;
    int rotDir = -1;
    bool isActiveAnime = false;
    
    public RushAttackNode(RushAttackScriptableObject rushAttackInfo, PlaySound playSound, Collider[] bossColliders ,SpawnFunction spawnRock ,Animator aniController, Transform transform,Transform target)
    {
        this.rushAttackInfo = rushAttackInfo;
        this.playSound = playSound;
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
        this.spawnRocks = spawnRock;
        this.bossColliders = bossColliders;
    }
    public void AddNode(INode node) { }


    //이슈: 잘못된 레이캐스트 위치로 인해 벽을 통과하는 것처럼 보임
    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        Debug.Log("RushAttack Running");
        skillSpan += Time.deltaTime;
        soundSpan += Time.deltaTime;

        transform.position += transform.forward * rushAttackInfo.rushSpeed * Time.deltaTime;
        Collider[] colliders =  Physics.OverlapSphere(transform.position, rushAttackInfo.hitRange, rushAttackInfo.DetectLayers); 
        //스킬 종료
        if (skillSpan >= rushAttackInfo.RushAttackDuration) 
        {
            aniController.SetBool("isRushAttack", false);
            foreach (Collider bossCollider in bossColliders)
                bossCollider.enabled = true;
            isActiveAnime = false;
            skillSpan = 0;
            soundSpan = 0;
            return INode.NodeState.Success;
        }

        if(soundSpan>rushAttackInfo.playSoundTime)
        {
            soundSpan = 0;
            playSound("Boss_Rush", false);
        }
        if(colliders!= null)
        {
            foreach (Collider collider in colliders)
            {
                
                if (collider.tag.Equals(rushAttackInfo.playerTag)) //플레이어 데미지 전달
                {
                    collider.GetComponent<PlayerControlManagerFix>().HurtPlayer(rushAttackInfo.damage);
                    Debug.Log("PlayerHit RushAttack!");
                }
                else if (collider.tag.Equals(rushAttackInfo.wallTag)) //드래곤 회전(벽에 닿았을 시)
                {
                    
                    Debug.Log("WallHit RushAttack!");
                    rotDir *= -1;
                    Quaternion curRot = transform.rotation;
                    curRot.y += 180;
                    transform.Rotate(new Vector3(0, -curRot.y, 0));

                    spawnRocks(rushAttackInfo.objs, 5);
                }
            }
        }
        
        

        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetBool("isRushAttack",true);
        if(aniController.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            foreach(Collider bossCollider in bossColliders)
                bossCollider.enabled = false;
            isActiveAnime = true;
        }
    }
}
