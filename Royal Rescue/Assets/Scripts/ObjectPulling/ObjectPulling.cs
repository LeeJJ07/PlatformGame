using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ObjectPulling
{
    public delegate GameObject InstantiateObj(GameObject obj);
    InstantiateObj instantiate;
    List<GameObject> objList;
    GameObject obj;
    
    public ObjectPulling(InstantiateObj instantiate,GameObject obj,string tag)
    {
        objList = new List<GameObject>();
        this.instantiate = instantiate;
        FindObjsWithITag(tag,obj.GetComponent<ITag>().GetTag());
        //FindObjs(tag);
        this.obj = obj;
    }
    void FindObjs(string tag)
    {
        GameObject[] gms = GameObject.FindGameObjectsWithTag(tag);
        if (gms.Length != 0)
        {
            foreach (GameObject gm in gms)
            {
                objList.Add(gm);
            }
        }
    }
    void FindObjsWithITag(string tag, string detailTag)
    {
        GameObject[] gms = GameObject.FindGameObjectsWithTag(tag);
        if (gms.Length != 0)
        {
            foreach (GameObject gm in gms)
            {
                if(gm.GetComponent<ITag>().CompareToTag(detailTag))
                {
                    objList.Add(gm);
                }
            }
        }
    }


    public GameObject GetObject()
    {
        if(objList.Count!=0)
        {
            foreach(GameObject obj in objList)
            {
                if(!obj.activeSelf)
                {
                    return obj;
                }
            }
        }
        GameObject gm = instantiate(obj);
        objList.Add(gm);
        return gm;
    }
    public List<GameObject> GetObjectList()
    {
        return objList;
    }
    public int ActiveObjCount()
    {
        int count = 0;
        foreach(GameObject gm in objList)
        {
            if (gm.activeSelf)
                count++;
        }
        return count;
    }
}
