using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    private ItemSlot curSlot;
    private Outline outline;

    public int index;
    public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        button.onClick.AddListener(OnButtonClick);
    }
    private void OnEnable()
    {
        outline.enabled = equipped;
    }
    public void Set(ItemSlot slot)
    {
        curSlot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
        quatityText.text = slot.quantity > 1 ? slot.quantity.ToString() : string.Empty;

    }
    public void Clear()
    {
        outline.enabled = false;
        curSlot = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;  
    }
    public void OnButtonClick()
    {
        GameDirector.instance.PlayerControl.inventory.SelectItem(index);
    }
    public void SetOutline(bool _enabled) { outline.enabled = _enabled; }
}
