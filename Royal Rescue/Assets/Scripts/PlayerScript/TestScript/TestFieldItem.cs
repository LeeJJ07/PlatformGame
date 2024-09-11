using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFieldItem : MonoBehaviour
{
    public TestItem item;
    public SpriteRenderer image;

    public void SetItem(TestItem _item)
    {
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;
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
