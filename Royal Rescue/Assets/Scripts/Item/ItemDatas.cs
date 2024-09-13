using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    HEAL, 
    POWER, 
    HEALTH, 
    MOVESPEED
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemDatas : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string dscription;
    public ItemType type;
    public Sprite icon;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
}
