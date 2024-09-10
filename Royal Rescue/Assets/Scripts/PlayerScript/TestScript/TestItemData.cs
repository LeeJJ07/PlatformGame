using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion,
    Weaphon,
    Coin,
    Etc
}
public class TestItemData : ScriptableObject
{
    public GameObject Prefab;
    public Vector3 Position;
    public int ID;
    public ItemType Type;
    public string Name;
    public string Description;
    public int Value;
    public Sprite Icon;
    public Sprite BigImage;
}
