using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
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
    Vector3 center;
    float duration;
    float span;
    bool isSpawnZonePrefab;
    public WarningRushAttack(SpawnZonePrefab spawnFunc, GameObject angryLight, GameObject WarningPrefab, Vector3 center ,float duration)
    {
        spawnObjs = new GameObject[1];
        this.spawnFunc = spawnFunc;
        this.angryLight = angryLight;
        this.WarningPrefab = WarningPrefab;
        this.duration = duration;
        this.center = center;
        warningPrefabOrignScale = WarningPrefab.transform.localScale;
        span = 0;
    }
    public void AddNode(INode node) {}

    public INode.NodeState Evaluate()
    {
        spawnDangerZone();
        span += Time.deltaTime;
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
        spawnObjs = spawnFunc(WarningPrefab, center, 1);
        foreach(GameObject obj in spawnObjs)
        {
            obj.transform.localScale = new Vector3(20, 1, 5);
        }
        isSpawnZonePrefab = true;
    }
}
