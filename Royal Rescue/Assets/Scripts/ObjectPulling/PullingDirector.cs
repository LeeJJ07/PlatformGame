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


    //PullingDirector ����Ʈ�� �ִ� ��� ��ü���� ��Ȱ��ȭ ��Ű�� �Լ�
    public void DeActivateAllObjects()
    {
        
        foreach (GameObject obj in objectList)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// �±׿� ���� ��ü�� ��Ȱ��ȭ ��
    /// return : void
    /// </summary>
    /// <param name="tag">��Ȱ��ȭ ��ų ��ü�� �±��Է�</param>
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
    /// ��ü�� �⺻�±׿� ITag�� detailTag�� ���Ͽ� 
    /// ��ü�� ������ ��ġ�� ��ü Ȱ��ȭ Ȥ�� ������
    /// </summary>
    /// <param name="tag">��ü�� �⺻�±�</param>
    /// <param name="detailTag">ITag�� �±� �̸�</param>
    /// <param name="position">������ ��ġ</param>
    /// <returns></returns>
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

    /// <summary>
    /// ��ü�� �⺻ �±׸� Ȯ���Ͽ� ��ü�� ������ ��ġ�� Ȱ��ȭ Ȥ�� ������
    /// </summary>
    /// <param name="tag">��ü�� �⺻ �±�</param>
    /// <param name="position">������ ��ġ</param>
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

    //ObjectPulling.cs���� ����ϱ����� �Լ�
    private GameObject CreateObject(GameObject obj)
    {
        return Instantiate(obj);
    }
}
