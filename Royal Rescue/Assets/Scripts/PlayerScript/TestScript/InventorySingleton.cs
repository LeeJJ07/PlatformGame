using System.Collections;
using System.Collections.Generic;
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

    List<TestItem> items = new List<TestItem>();

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
        slotCnt = 4;
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
