using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventorySingleton : MonoBehaviour
{
    #region Singleton
    public static InventorySingleton Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public delegate void OnSlotCntChange(int val);
    public OnSlotCntChange onSlotCntChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<TestItem> items = new List<TestItem>();

    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCntChange.Invoke(slotCnt);
        }
    }


    void Start()
    {
        slotCnt = 12;
    }

    public bool AddItem(TestItem _item)
    {
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            if (onChangeItem != null)
            onChangeItem.Invoke();
            return true;
        }
        return false;
    }
   
    public void RemoveItem(int slotNum)
    {
        if (slotNum < 0 || slotNum >= items.Count) 
            return;
        items.RemoveAt(slotNum);
        onChangeItem.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FieldItem"))
        {
            TestFieldItem fieldItems = other.GetComponent<TestFieldItem>();
            if(AddItem(fieldItems.GetItem()))
            {
                fieldItems.DestroyItem();
            }
        }
    }
}
