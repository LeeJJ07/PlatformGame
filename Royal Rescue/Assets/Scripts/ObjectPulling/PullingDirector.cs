using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 오브젝트들을 리스트로 만들어
/// 리스트에 있는 오브젝트들만 풀링을 하도록함
/// </summary>
public class PullingDirector : MonoBehaviour
{
    [SerializeField]List<GameObject> objectList = new List<GameObject>();
    List<ObjectPulling> pullingList = new List<ObjectPulling>();
    void Start()
    {
        pullingList = new List<ObjectPulling> ();
        if (objectList.Count!=0)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                pullingList.Add(new ObjectPulling(CreateObject, objectList[i], objectList[i].tag));
            }
        }
    }

    //스폰한 모든 객체 비활성화
    public void DeActivateSpawnObjects()
    {
        List<GameObject> pullingObjList = new List<GameObject>();
        int i = 0;
        foreach (ObjectPulling pulling in pullingList)
        {
            pullingObjList = pulling.GetObjectList();
            i++;
            foreach(GameObject obj in pullingObjList)
            {
                obj.SetActive(false);
            }
        }
    }
    
    //ITag의 태그 비교후 비활성화
    public void DeActivateObjectsWithTag(string tag)
    {
        List<GameObject> pullingObjList = new List<GameObject>();

        foreach (ObjectPulling pulling in pullingList)
        {
            pullingObjList = pulling.GetObjectList();
            foreach(GameObject obj in pullingObjList)
            {
                if (obj.CompareTag(tag) && obj.activeSelf == true)
                    obj.SetActive(false);

            }
        }
    }
    
    //객체의 ITag와 스폰할려는ITag가 같으면 지정한 위치로 객체 스폰
    public GameObject SpawnObjectwithITag(string tag, ITag detailTag, Vector3 position)
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].CompareTag(tag))
            {
                if (objectList[i].GetComponent<ITag>().CompareToTag(detailTag.GetTag()))
                {
                    //pullingList와 objectList의 인덱스에 담긴 정보가 다름
                    GameObject obj = pullingList[i].GetObject();
                    obj.SetActive(true);
                    obj.transform.position = position;
                    return obj;
                }
            }
        }
        return null;
    }

    //지정한 위치로 객체 스폰
    public GameObject SpawnObject(string tag,Vector3 position)
    {
        for(int i=0; i<objectList.Count; i++)
        {
            if (objectList[i].tag.Equals(tag))
            {
                GameObject obj = pullingList[i].GetObject();
                obj.SetActive(true);
                obj.transform.position = position;
                return obj;
            }
        }
        return null;
    }

    //객체생성 함수 델리게이트 전달
    GameObject CreateObject(GameObject obj)
    {
        return Instantiate(obj);
    }
}
