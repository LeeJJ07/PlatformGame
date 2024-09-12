using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*public class ItemSlot
{
    public itemData item;
    public int amount;
}*/


public class Inventory : MonoBehaviour
{
    //public ItemSlotUI[] uidSlot;
    
}
//public static bool inventoryActivated = false;


// ÇÊ¿äÇÑ ÄÄÆ÷³ÍÆ®
//[SerializeField]
//private GameObject go_InventoryBase;
//private GameObject go_SlotsParent;

// ½½·Ôµé.
//private Slot[] slots;


// Use this for initialization
//void Start()
//{
//    slots = go_SlotsParent.GetComponentsInChildren<Slot>();
//}

// Update is called once per frame
//void Update()
//{
//    TryOpenInventory();
//}

//private void TryOpenInventory()
//{
//    if (Input.GetButtonDown("InventoryKey"))
//    {
//        inventoryActivated = !inventoryActivated;

//        if (inventoryActivated)
//            OpenInventory();
//        else
//            CloseInventory();
//    }
//}

//private void OpenInventory()
//{
//    go_InventoryBase.SetActive(true);
//}

//private void CloseInventory()
//{
//    go_InventoryBase.SetActive(false);
//}

//public void AcquireItem(Item _item, int _count = 1)
//{
//    if (Item.ItemType.Equipment != _item.itemType)
//    {
//        for (int i = 0; i < slots.Length; i++)
//        {
//            if (slots[i].item != null)
//            {
//                if (slots[i].item.itemName == _item.itemName)
//                {
//                    slots[i].SetSlotCount(_count);
//                    return;
//                }
//            }
//        }
//    }

//    for (int i = 0; i < slots.Length; i++)
//    {
//        if (slots[i].item == null)
//        {
//            slots[i].AddItem(_item, _count);
//            return;
//        }
//    }
//}