   
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ItemSlot
{
    public ItemDatas item;
    public int quantity;
}
public class Inventory : MonoBehaviour
{

    public ItemSlotUI[] uidSlot;
    public ItemSlot[] slots;

    public GameObject inventoryWindow;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject useButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerControlManagerFix controller;

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    public static Inventory instance;
    private void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerControlManagerFix>();

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uidSlot.Length];

        for (int i = 0; i < slots.Length; i++) {
            slots[i] = new ItemSlot();
            uidSlot[i].index = i;
            uidSlot[i].Clear();
        }
        ClearSelectItemWindow();
    }
    public void Toggle() {
        if (inventoryWindow.activeInHierarchy) {
            inventoryWindow.SetActive(false);
            onCloseInventory?.Invoke();
            controller.ToggleCursor(false);
        } else {
            inventoryWindow.SetActive(true);
            onOpenInventory?.Invoke();
            controller.ToggleCursor(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemDatas item)
    {
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }
        ThrowItem(item);
    }
    private void ThrowItem(ItemDatas item) //구현해야함
    {
        
    }
    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                uidSlot[i].Set(slots[i]);
            else
                uidSlot[i].Clear();
        }
    }

    ItemSlot GetItemStack(ItemDatas item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount)
                return slots[i];
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        for(int i = 0;i < uidSlot.Length; i++) {
            uidSlot[i].SetOutline(false);
        }
        uidSlot[index].SetOutline(true);

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        dropButton.SetActive(true);
    }

    private void ClearSelectItemWindow()
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;

        useButton.SetActive(false);
        dropButton.SetActive(false);
    }
    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.HEAL:
                        //conditions.Heal(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.POWER:
                        break;
                    case ConsumableType.HEALTH:
                        break;
                    case ConsumableType.MOVESPEED:
                        break;
                }
            }
        }
        RemoveSelectedItem();
    }
    public void OnEquipButton()
    {

    }

    void UnEquip(int index)
    {

    }

    public void OnUnEquipButton()
    {

    }
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        selectedItem.quantity--;
        if (selectedItem.quantity <= 0)
        {
            selectedItem.item = null;
            ClearSelectItemWindow();
        }
        UpdateUI();
    }

    public void RemoveItem(ItemDatas item)
    {

    }
    public bool HasItems(ItemDatas item, int quantity)
    {
        return false;
    }
}
