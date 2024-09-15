using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ResourceType 
{
    RUBY,
    DIAMOND,
    JADE,
    ETC
}
public enum ConsumableType
{
    HEAL,
    POWER,
    HEALTH,
    MOVESPEED
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemDatas : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    [TextArea]
    public string description;
    public ItemType type;
    public Sprite icon;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Resource")]
    public ResourceType resource;
}