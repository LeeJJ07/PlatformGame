using UnityEngine;
using static ScreamAttackNode;

public class WarningRushAttack : INode
{
    /*
        필요 컴포넌트 및 변수
        1. Angry Light : GameObject
        2. DangerZone오브젝트 : GameObject
        3. 경고 지속시간 : float
     */
    public delegate GameObject[] SpawnZonePrefab(GameObject obj,Vector3 posi, int count);
    Vector3 warningPrefabOrignScale;
    SpawnZonePrefab spawnFunc;
    GameObject[] spawnObjs;
    GameObject angryLight;
    GameObject WarningPrefab;
    Transform[] wallTransforms;
    Vector3 warningZoneCenter;
    Transform transform;
    float duration;
    float span;
    bool isSpawnZonePrefab;
    Ray forwardRay;
    Ray backRay;
    RaycastHit forwardHit;
    RaycastHit backHit;

    public WarningRushAttack(SpawnZonePrefab spawnFunc, GameObject angryLight, GameObject WarningPrefab, Transform transform,float duration)
    {
        spawnObjs = new GameObject[1];
        this.spawnFunc = spawnFunc;
        this.angryLight = angryLight;
        this.WarningPrefab = WarningPrefab;
        this.duration = duration;
        this.transform = transform;
        warningPrefabOrignScale = WarningPrefab.transform.localScale;
        span = 0;
    }
    public void AddNode(INode node) {}

    public INode.NodeState Evaluate()
    {
        spawnDangerZone();
        span += Time.deltaTime;
        
        Debug.DrawRay(forwardRay.origin, forwardRay.direction,Color.blue);
        Debug.DrawRay(backRay.origin, backRay.direction,Color.green);
        if(span>=duration)
        {
            span = 0;
            isSpawnZonePrefab = false;
            foreach (GameObject obj in spawnObjs)
            {
                obj.transform.localScale = warningPrefabOrignScale;
                obj.SetActive(false);
            }
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void spawnDangerZone()
    {
        if (isSpawnZonePrefab) return;
        forwardRay = new Ray(transform.position, Vector3.left);
        backRay = new Ray(transform.position, Vector3.right);
        Physics.Raycast(forwardRay, out forwardHit, 100, LayerMask.GetMask("Wall"));
        Physics.Raycast(backRay, out backHit, 100, LayerMask.GetMask("Wall"));
        float size = 0;
        if (forwardHit.collider != null && backHit.collider != null) 
        {
            float center = ((forwardHit.point.x+backHit.point.x)/2);
            size = Mathf.Abs(backHit.point.x- forwardHit.point.x)/2;
            warningZoneCenter = new Vector3(center, transform.position.y, 0);
        }
        spawnObjs = spawnFunc(WarningPrefab, warningZoneCenter, 1);
        foreach(GameObject obj in spawnObjs)
        {
            obj.transform.localScale = new Vector3(size, 1, 5);
        }
        isSpawnZonePrefab = true;
    }
}
