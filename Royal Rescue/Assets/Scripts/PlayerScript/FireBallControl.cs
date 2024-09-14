using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallControl : MonoBehaviour
{
    Rigidbody rigidbody;
    public float throwForce;
    public bool isFireball = false;
    public float gravity = -9.81f;
    public Vector3 ballDir;

    public int bombDamage = 50; // ��ź�� ������ �ִ� ������
    public GameObject explosionEffect; // ���� ȿ��
    public GameObject target;
    PlayerControlManagerFix player;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody >();
        rigidbody.AddForce((ballDir + Vector3.up * 1.5f) * throwForce, ForceMode.Impulse);//������
    }
    private void Update()
    {
        Destroy(this.gameObject, 2f);
        
    }
    // Update is called once per frame
    // ��ź�� �߻�� �� ȣ���ϴ� �Լ�
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // ������ �������� ���� (����)
            //EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            //if (enemyHealth != null)
            //enemyHealth.TakeDamage(damage);
            EnemyControler enemyHP = other.gameObject.GetComponent<EnemyControler>();
            enemyHP.health -= bombDamage;
            Debug.Log("������ ���̾ ����");
        }
        if (other.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �浹�� ��� �ƹ� �۾��� ���� ����
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

            // ��ƼŬ�� ��� �ð��� ����� ��, �ش� �ð��� ������ ��ƼŬ ������Ʈ�� ����
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(explosion, 1.0f);
            }
            else
            {
                // ��ƼŬ �ý����� ���� ���, �����ϰ� ���� �ð� �Ŀ� ����
                Destroy(explosion, 3.0f);
            }
        }
        Destroy(gameObject);
    }
}
