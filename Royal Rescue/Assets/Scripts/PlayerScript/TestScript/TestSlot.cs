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
    // ���Կ��� ���� �ϴ� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateItemUI();
        // ���콺�� �����϶����� �κ��丮 ����â UI�� ������Ʈ ���ش�
    }

    public void OnMouseDown()
    {
        if (CurrentitemData.Type == ItemType.Potion)
        {
            Debug.Log("���� �Դ´�");
            //TestInven.Instance.Remove(CurrentitemData);
            // ������ �Һ������, ������ 0�̵Ǹ� �������
        }
        Time.timeScale = 1.0f;
    }
    public void UpdateItemUI()
    {
        //Inventory.instance.InventoryDescriptionUI.Refresh(CurrentitemData);

    }

}
