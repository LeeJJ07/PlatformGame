using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HP,�ǰ�,����ִϸ��̼�, �����浹ó��
public class WeaponControl : MonoBehaviour
{
    //public enum Type { Melee, Range };
    //public Type type;
    public int damage;
    public float rate;
    public bool isAttackWeapon;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public GameObject player;
    public BoxCollider boxCollider;

    public void Start()
    {
        boxCollider = this.gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }
    public void WeaponUse()
    {
        /*if(type == Type.Melee)
        {  
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }*/
        //isAttackButton && isAttackPossible
        
    }
    IEnumerator Swing()
    {
        boxCollider.enabled = true;
        yield return new WaitForSeconds(2f);//0.1�� ���
        //meleeArea.enabled = true;
        boxCollider.enabled = false;

        /*
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);//0.3�� ���
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);//0.3�� ���
        trailEffect.enabled = false;*/
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
