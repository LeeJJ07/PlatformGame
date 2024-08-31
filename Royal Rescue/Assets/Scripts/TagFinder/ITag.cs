using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITag
{
    public string GetTag();
    public bool CompareToTag(string detailTag);
}
