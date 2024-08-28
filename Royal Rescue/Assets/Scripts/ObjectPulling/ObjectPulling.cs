using System.Collections.Generic;
using UnityEngine;

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
        FindObjs(tag);
        this.obj = obj;
    }
    void FindObjs(string tag)
    {
        GameObject test = GameObject.FindWithTag(tag);
        GameObject[] gms = GameObject.FindGameObjectsWithTag(tag);
        if (gms.Length != 0)
        {
            foreach (GameObject gm in gms)
            {
                objList.Add(gm);
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
}
