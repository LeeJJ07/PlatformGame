using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallControl : MonoBehaviour
{
    //Rigidbody rigidbody;
    public float throwForce = 10;
    public GameObject player;
    public bool isDirPlayer;
    public Vector3 ballDir;
    public int damage = 50; // ��ź�� ������ �ִ� ������
    public GameObject explosionEffect; // ���� ȿ��
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //Vector3 dir = player.GetComponent<PlayerControlManagerFix>().isDirRight ? Vector3.right : Vector3.left;
        transform.position += throwForce * ballDir * Time.deltaTime;
        //������
       // rigidbody.AddForce(direction * throwForce, ForceMode.Impulse);
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
            enemyHP.health -= damage;
            Debug.Log("������ ���̾ ����");
            
        }
        if (other.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �浹�� ��� �ƹ� �۾��� ���� ����
            return;
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
