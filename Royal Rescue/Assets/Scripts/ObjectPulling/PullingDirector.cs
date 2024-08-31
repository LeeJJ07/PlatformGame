using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void DeActivateAllObjects()
    {
        
        foreach (GameObject obj in objectList)
        {
            obj.SetActive(false);
        }
    }
    
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
    GameObject CreateObject(GameObject obj)
    {
        return Instantiate(obj);
    }
}
