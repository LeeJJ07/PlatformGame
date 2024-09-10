using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class TestInven : MonoBehaviour
{
    public static TestInven Instance;
    public List<TestItemData> items = new List<TestItemData>();
    public GameObject Inventory;
    public Transform itemContect;
    public GameObject InventoryItem;
    public List<TestSlot> ItemInventoryUISlots;
    public delegate void OnItemChanged();
    public static event OnItemChanged onItemChangedCallback;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        ListItem();
        // 시작 할 때 아이템이 있으면 인벤토리 UI 업데이트 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !Inventory.activeSelf;
            Inventory.SetActive(isActive); // 인벤토리 UI 활성화/비활성화 토글
                                           // 인벤토리가 활성화되면 마우스 커서를 표시하고, 그렇지 않으면 숨깁니다.
            UnityEngine.Cursor.visible = isActive;
            // 인벤토리가 활성화되면 마우스 커서를 잠그지 않고, 그렇지 않으면 잠급니다.
            UnityEngine.Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = isActive ? 0 : 1;
        }
    }
    public void Add(TestItemData newItem)
    {
        TestItemData existingItem = items.Find(item => item.Name == newItem.Name);
        if (existingItem != null)
        {
            existingItem.Value += 1;
            // 같은 아이템이면 카운트 +1
        }

        else
        {
            newItem.Value = 1;
            items.Add(newItem);
            // 새로운 아이템이면 추가
        }
        onItemChangedCallback?.Invoke(); // 아이템 변경 이벤트 발생
    }

    public void Remove(TestItemData item)
    {
        TestItemData itemToRemove = items.Find(i => i.Name == item.Name);
        if (itemToRemove != null && itemToRemove.Value > 0)
        {
            itemToRemove.Value -= 1;
            int index = items.IndexOf(itemToRemove);
            ItemInventoryUISlots[index].countitemText.text = itemToRemove.Value.ToString();
            Debug.Log("포션이 깎여야함");

            if (itemToRemove.Value == 0)
            {
                Debug.Log("포션이 0이 되어야 함");
                ItemInventoryUISlots[index].gameObject.SetActive(false);
                items.Remove(itemToRemove);
            }

            onItemChangedCallback?.Invoke();
        }
    }
    public void ListItem()
    {
        foreach (Transform child in itemContect)
        {
            child.gameObject.SetActive(false);
            // 빈 슬롯 다 지우고
        }
        foreach (Transform child in itemContect)
        {
            if (!child.gameObject.activeSelf)
            // 빈 슬롯 상태에서
            {
                for (int i = 0; i < items.Count; i++)
                {
                    // 아이템 먹은 만큼 슬롯 활성화하고 UI 업데이트
                    ItemInventoryUISlots[i].gameObject.SetActive(true);
                    ItemInventoryUISlots[i].itemIconNameText.text = items[i].Name;
                    ItemInventoryUISlots[i].itemNameText.text = items[i].Name;
                    ItemInventoryUISlots[i].itemDescriptionText.text = items[i].Description;
                    ItemInventoryUISlots[i].itemIconImage.sprite = items[i].Icon;
                    ItemInventoryUISlots[i].itemBigImage.sprite = items[i].BigImage;
                    ItemInventoryUISlots[i].countitemText.text = $"{items[i].Value}";
                    ItemInventoryUISlots[i].CurrentitemData = items[i];
                    // 슬롯에 커렌트 아이템을 넣어 이 아이템이 무엇인지 알게 해준다
                }
            }
        }
    }
}
