using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;



public class TestSlot : MonoBehaviour, IPointerEnterHandler
{

    public TextMeshProUGUI itemIconNameText;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Image itemIconImage;
    public Image itemBigImage;
    public TextMeshProUGUI countitemText;
    public TestItemData CurrentitemData;
    // 슬롯에서 들어가야 하는 변수들
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateItemUI();
        // 마우스가 움직일때마다 인벤토리 상태창 UI를 업데이트 해준다
    }

    public void OnMouseDown()
    {
        if (CurrentitemData.Type == ItemType.Potion)
        {
            Debug.Log("포션 먹는다");
            //TestInven.Instance.Remove(CurrentitemData);
            // 포션은 소비아이템, 갯수가 0이되면 사라진다
        }
        Time.timeScale = 1.0f;
    }
    public void UpdateItemUI()
    {
        //Inventory.instance.InventoryDescriptionUI.Refresh(CurrentitemData);

    }

}
