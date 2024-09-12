using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equip,
    Potion,
    Etc
}


[System.Serializable]
public class TestItem 
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public List<TestItemEffect> efts;
    public bool Use()
    {
        bool isUsed = false;
        foreach(TestItemEffect eft in efts)
        {
            isUsed = eft.ExecuteRole();
        }
        return isUsed;
    }
}
