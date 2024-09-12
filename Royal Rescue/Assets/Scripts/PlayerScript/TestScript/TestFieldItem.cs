using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestFieldItem : MonoBehaviour
{
    public TestItem item;
    public SpriteRenderer image;
    //public Image image;

    public void SetItem(TestItem _item)
    {
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;
        item.efts = _item.efts;

        image.sprite = item.itemImage;
    }

    public TestItem GetItem() 
    {
        return item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
