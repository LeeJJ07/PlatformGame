using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SkillSlot : MonoBehaviour
{
    [SerializeField]private UnityEngine.UI.Text text_Count;
    [SerializeField]private GameObject go_CountImage;
    public GameObject player;
    private int fireBallCnt;

    // Update is called once per frame
    void Update()
    {
        fireBallCnt = player.GetComponent<PlayerControlManagerFix>().skillCount;
        text_Count.text = fireBallCnt.ToString();
    }
}
