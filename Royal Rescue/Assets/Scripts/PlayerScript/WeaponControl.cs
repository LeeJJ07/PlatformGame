using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HP,�ǰ�,����ִϸ��̼�, �����浹ó��
public class WeaponControl : MonoBehaviour
{
    //public enum Type { Melee, Range };
    //public Type type;
    public int damage = 25;
    public float rate;
    public bool isAttackWeapon;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public GameObject player;
    public BoxCollider boxCollider;

    
    public void WeaponUse()
    {
        /*if(type == Type.Melee)
        {  
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }*/
        //isAttackButton && isAttackPossible
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && isAttackWeapon)
        {
            EnemyControler enemyHP = other.gameObject.GetComponent<EnemyControler>();
            enemyHP.health -= damage;
            Debug.Log("���Ͱ���");
        }
    }
}
