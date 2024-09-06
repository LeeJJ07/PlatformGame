using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������Ʈ���� ����Ʈ�� �����
/// ����Ʈ�� �ִ� ������Ʈ�鸸 Ǯ���� �ϵ�����
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

    //������ ��� ��ü ��Ȱ��ȭ
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
    
    //ITag�� �±� ���� ��Ȱ��ȭ
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
    
    //��ü�� ITag�� �����ҷ���ITag�� ������ ������ ��ġ�� ��ü ����
    public GameObject SpawnObjectwithITag(string tag, ITag detailTag, Vector3 position)
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].CompareTag(tag))
            {
                if (objectList[i].GetComponent<ITag>().CompareToTag(detailTag.GetTag()))
                {
                    //pullingList�� objectList�� �ε����� ��� ������ �ٸ�
                    GameObject obj = pullingList[i].GetObject();
                    obj.SetActive(true);
                    obj.transform.position = position;
                    return obj;
                }
            }
        }
        return null;
    }

    //������ ��ġ�� ��ü ����
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

    //��ü���� �Լ� ��������Ʈ ����
    GameObject CreateObject(GameObject obj)
    {
        return Instantiate(obj);
    }
}
