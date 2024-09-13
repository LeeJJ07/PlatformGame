using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemDatas item;
    public void OnInteract()
    {
        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
}