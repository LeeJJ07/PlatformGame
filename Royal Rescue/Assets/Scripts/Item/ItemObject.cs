using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemDatas item;

    private Canvas uiCanvas;
    public GameObject getItemTextPrefab;
    public void OnInteract()
    {
        GameDirector.instance.PlayerControl.inventory.AddItem(item);

        uiCanvas = GameObject.Find("InGame Canvas").GetComponent<Canvas>();
        GameObject getItemUI = Instantiate(getItemTextPrefab, uiCanvas.transform) as GameObject;
        getItemUI.GetComponent<ItemText>().itemText = item.displayName + " È¹µæ!";

        Destroy(gameObject);
    }
    public void OnInteractCoin() {
        GameDirector.instance.PlayerControl.EatCoin();
        Destroy(gameObject);
    }
}