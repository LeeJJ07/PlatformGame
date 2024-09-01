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
    List<GameObject> pullingObjList = new List<GameObject>();
    void Start()
    {
        if(objectList.Count!=0)
        {
            for(int i=0; i<objectList.Count; i++)
            {
                pullingList.Add(new ObjectPulling(CreateObject, objectList[i], objectList[i].tag));
            }
        }
    }


    //PullingDirector 리스트에 있는 모든 객체들을 비활성화 시키는 함수
    public void DeActivateAllObjects()
    {
        
        foreach (GameObject obj in objectList)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 태그와 같은 객체를 비활성화 함
    /// return : void
    /// </summary>
    /// <param name="tag">비활성화 시킬 객체의 태그입력</param>
    public void DeActivateObjectsWithTag(string tag)
    {
        
        foreach(ObjectPulling pulling in pullingList)
        {
            pullingObjList = pulling.GetObjectList();
            foreach(GameObject obj in pullingObjList)
            {
                if (obj.CompareTag(tag) && obj.activeSelf == true)
                    obj.SetActive(false);

            }
        }
    }
    
    /// <summary>
    /// 객체의 기본태그와 ITag의 detailTag를 비교하여 
    /// 객체를 지정한 위치로 객체 활성화 혹은 생성함
    /// </summary>
    /// <param name="tag">객체의 기본태그</param>
    /// <param name="detailTag">ITag의 태그 이름</param>
    /// <param name="position">생성할 위치</param>
    /// <returns></returns>
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

    /// <summary>
    /// 객체의 기본 태그를 확인하여 객체를 지정한 위치로 활성화 혹은 생성함
    /// </summary>
    /// <param name="tag">객체의 기본 태그</param>
    /// <param name="position">생성할 위치</param>
    /// <returns></returns>
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

    //ObjectPulling.cs에서 사용하기위한 함수
    private GameObject CreateObject(GameObject obj)
    {
        return Instantiate(obj);
    }
}
