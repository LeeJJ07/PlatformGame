using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetailTag : MonoBehaviour, ITag
{
    [Header("Detail Tag"), SerializeField] 
    string detailTag;
    public string GetTag()
    {
        return detailTag;
    }
    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }
}
