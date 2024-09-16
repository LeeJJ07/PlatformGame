using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemDatas item;
    public void OnInteract()
    {
        GameDirector.instance.PlayerControl.inventory.AddItem(item);
        Destroy(gameObject);
    }
    public void OnInteractCoin() {
        GameDirector.instance.PlayerControl.EatCoin();
        Destroy(gameObject);
    }
}