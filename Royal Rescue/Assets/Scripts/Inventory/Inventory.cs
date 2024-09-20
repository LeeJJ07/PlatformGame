   
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

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    private Canvas uiCanvas;
    public GameObject useItemTextPrefab;

    [HideInInspector] public int healPotionCount;

    private void Awake()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uidSlot.Length];
        ResetItemSlots();

        healPotionCount = 5;
    }
    public void ResetItemSlots()
    {
        for (int i = 0; i < slots.Length; i++) {
            slots[i] = new ItemSlot();
            uidSlot[i].index = i;
            uidSlot[i].Clear();
        }
        ClearSelectItemWindow();
    }
    public void Toggle() {
        if (slots.Length == 0) return;
        
        if (inventoryWindow.activeInHierarchy) {
            inventoryWindow.SetActive(false);
            onCloseInventory?.Invoke();
            GameDirector.instance.SetCursorVisibility(false);
        } else {
            inventoryWindow.SetActive(true);
            onOpenInventory?.Invoke();
            GameDirector.instance.SetCursorVisibility(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemDatas item)
    {
        if (item.displayName == "힐 포션")
        {
            healPotionCount++;
            return;
        }

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
        if (item.type == ItemType.Resource) {
            switch (item.resource) {
                case ResourceType.RUBY:
                    AltarControl.redGem++;
                    break;
                case ResourceType.DIAMOND:
                    AltarControl.whiteGem++;
                    break;
                case ResourceType.JADE:
                    AltarControl.greenGem++;
                    break;
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
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable || selectedItem.item.type == ItemType.Equipable);
        dropButton.SetActive(selectedItem.item.type == ItemType.Consumable);
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
        uiCanvas = GameObject.Find("InGame Canvas").GetComponent<Canvas>();
        GameObject useItemUI = Instantiate(useItemTextPrefab, uiCanvas.transform) as GameObject;
        string useText = "";

        if (selectedItem.item.type == ItemType.Consumable)
        {  
            int randNum = Random.Range(0, 10), i = 0;
            if (randNum < 3) i = 0;
            else if (randNum < 8) i = 1;
            else if (randNum < 10) i = 2;

            switch (selectedItem.item.consumables[i].type) {
                case ConsumableType.POWER:
                    GameDirector.instance.PlayerControl.IncreaseAtk((int)selectedItem.item.consumables[i].value);
                    useText = "공격력 ";
                    break;
                case ConsumableType.HEALTH:
                    GameDirector.instance.PlayerControl.IncreaseMaxHp((int)selectedItem.item.consumables[i].value);
                    useText = "최대 체력 ";
                    break;
                case ConsumableType.MOVESPEED:
                    GameDirector.instance.PlayerControl.IncreaseSpeed((int)selectedItem.item.consumables[i].value);
                    useText = "이동 속도 ";
                    break;
            }
            if ((int)selectedItem.item.consumables[i].value > 0) useText += (int)selectedItem.item.consumables[i].value + " 증가!";
            else useText += -(int)selectedItem.item.consumables[i].value + " 감소!";
            
        } else if(selectedItem.item.type == ItemType.Equipable) {
            GameDirector.instance.PlayerControl.isAttackEnhance = true;
            useText += "원거리 공격으로 강화 완료!";
        }
        useItemUI.GetComponent<ItemText>().itemText = useText;
        RemoveSelectedItem();
    }

    public void OnDropButton()
    {
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

    public void UseGem(ResourceType resourceType) {
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].item == null || slots[i].item.resource != resourceType)
                continue;

            // 자원의 수량을 감소시키고, 수량이 0이 되면 해당 아이템을 제거
            slots[i].quantity--;
            if (slots[i].quantity <= 0) {
                slots[i].item = null;
            }

            ClearSelectItemWindow();
            UpdateUI();
            break;
        }
    }
}