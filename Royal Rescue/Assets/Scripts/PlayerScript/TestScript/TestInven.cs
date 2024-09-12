using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using static UnityEditor.Progress;

public class TestInven : MonoBehaviour
{
    InventorySingleton inven;
    public GameObject inventoryPanel;
    bool aciveInventory = false;

    public TestSlot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inven = InventorySingleton.Instance;
        slots = slotHolder.GetComponentsInChildren<TestSlot>();
        inven.onSlotCntChange += SlotChange;
        inven.onChangeItem += ReDrawSlotUI;
        inventoryPanel.SetActive(aciveInventory);
    }

    private void SlotChange(int val)
    {
        for(int i = 0; i < slots.Length ; i++)
        {
            slots[i].slotNum = i;
            if (i < inven.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            aciveInventory = !aciveInventory;
            inventoryPanel.SetActive(aciveInventory);
        }
    }

    public void AddSlot()
    {
        inven.SlotCnt++;
    }

    public void ReDrawSlotUI()
    {
        for(int i = 0; i < slots.Length ; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count ; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].slotNum = i;
            slots[i].UpdateSlotUI();

        }
    }
}
