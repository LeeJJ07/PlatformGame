using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Object", menuName = "Items / Weaphon")]
public class SwordObject : TestItemData
{
    // Start is called before the first frame update
    private void Awake()
    {
        Type = ItemType.Weaphon;
    }
}
