using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class TestSlot : MonoBehaviour, IPointerUpHandler
{
    public int slotNum;
    public TestItem item;
    public Image itemIcon;

    public void UpdateSlotUI()
    {
        if (item == null) 
            return;
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (item == null) 
            return;
        foreach (TestItemEffect effect in item.efts)
        {
            if (effect is TestSpeedPotion speedPotionEffect)
            {
                speedPotionEffect.SetPlayer(GameObject.FindWithTag("Player"));
            }
            else if (effect is TestAtkPotion AtkPotionEffect)
            {
                AtkPotionEffect.SetPlayer(GameObject.FindWithTag("Player"));
            }
        }
        bool isUse = item.Use();
        if(isUse)
        {
            InventorySingleton.Instance.RemoveItem(slotNum);
        }
    }
}
