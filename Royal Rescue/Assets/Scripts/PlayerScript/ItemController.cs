using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    //public GameObject fireballScroll;
    public int scrollCnt = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerControlManagerFix player;
        if(other.gameObject.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerControlManagerFix>();
            if (player.skillCount + scrollCnt > 5)
            {
                Debug.Log("��� ���� Ƚ���� �ִ�ġ�� ��");
                player.skillCount = 5;
            }
            else
            {
                Debug.Log("��� ���� Ƚ�� " + scrollCnt + " ����");
                player.skillCount += scrollCnt;
            }
            Debug.Log("���� ���� Ƚ�� : " + player.skillCount);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        
    }
}
/*
 * 
 */