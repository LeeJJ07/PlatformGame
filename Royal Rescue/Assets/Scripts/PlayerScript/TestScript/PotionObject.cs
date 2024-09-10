using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Potion Object", menuName = "Items / Potion")]
public class PotionObject : TestItemData
{
    // Start is called before the first frame update
    private void Awake()
    {
        Type = ItemType.Potion;
    }
}
