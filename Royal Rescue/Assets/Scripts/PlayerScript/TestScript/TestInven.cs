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
        // ���� �� �� �������� ������ �κ��丮 UI ������Ʈ 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !Inventory.activeSelf;
            Inventory.SetActive(isActive); // �κ��丮 UI Ȱ��ȭ/��Ȱ��ȭ ���
                                           // �κ��丮�� Ȱ��ȭ�Ǹ� ���콺 Ŀ���� ǥ���ϰ�, �׷��� ������ ����ϴ�.
            UnityEngine.Cursor.visible = isActive;
            // �κ��丮�� Ȱ��ȭ�Ǹ� ���콺 Ŀ���� ����� �ʰ�, �׷��� ������ ��޴ϴ�.
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
            // ���� �������̸� ī��Ʈ +1
        }

        else
        {
            newItem.Value = 1;
            items.Add(newItem);
            // ���ο� �������̸� �߰�
        }
        onItemChangedCallback?.Invoke(); // ������ ���� �̺�Ʈ �߻�
    }

    public void Remove(TestItemData item)
    {
        TestItemData itemToRemove = items.Find(i => i.Name == item.Name);
        if (itemToRemove != null && itemToRemove.Value > 0)
        {
            itemToRemove.Value -= 1;
            int index = items.IndexOf(itemToRemove);
            ItemInventoryUISlots[index].countitemText.text = itemToRemove.Value.ToString();
            Debug.Log("������ �𿩾���");

            if (itemToRemove.Value == 0)
            {
                Debug.Log("������ 0�� �Ǿ�� ��");
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
            // �� ���� �� �����
        }
        foreach (Transform child in itemContect)
        {
            if (!child.gameObject.activeSelf)
            // �� ���� ���¿���
            {
                for (int i = 0; i < items.Count; i++)
                {
                    // ������ ���� ��ŭ ���� Ȱ��ȭ�ϰ� UI ������Ʈ
                    ItemInventoryUISlots[i].gameObject.SetActive(true);
                    ItemInventoryUISlots[i].itemIconNameText.text = items[i].Name;
                    ItemInventoryUISlots[i].itemNameText.text = items[i].Name;
                    ItemInventoryUISlots[i].itemDescriptionText.text = items[i].Description;
                    ItemInventoryUISlots[i].itemIconImage.sprite = items[i].Icon;
                    ItemInventoryUISlots[i].itemBigImage.sprite = items[i].BigImage;
                    ItemInventoryUISlots[i].countitemText.text = $"{items[i].Value}";
                    ItemInventoryUISlots[i].CurrentitemData = items[i];
                    // ���Կ� Ŀ��Ʈ �������� �־� �� �������� �������� �˰� ���ش�
                }
            }
        }
    }
}
