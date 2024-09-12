using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemDataBase : MonoBehaviour
{
    public static TestItemDataBase Instance;
    private void Awake()
    {
        Instance = this;
    }
    public List<TestItem> ItemsDB = new List<TestItem>();

    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    private void Start()
    {
        for(int i = 0; i < 12; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<TestFieldItem>().SetItem(ItemsDB[Random.Range(0, 4)]);
        }
    }
}
