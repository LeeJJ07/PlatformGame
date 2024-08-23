using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HP,피격,사망애니메이션, 무기충돌처리
public class WeaponControl : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public bool isAttackWeapon;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public GameObject player;

    public void WeaponUse()
    {
        if(type == Type.Melee)
        { 
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);//0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);//0.3초 대기
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);//0.3초 대기
        trailEffect.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && isAttackWeapon)
        {
            EnemyControler enemyHP = other.gameObject.GetComponent<EnemyControler>();
            enemyHP.health -= damage;
            Debug.Log("몬스터공격");
        }
    }
}
