using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BeholderProjectile : MonoBehaviour
{
    GameObject player;
    Vector3 moveDir;
    [SerializeField] GameObject explosion;
    [SerializeField] float speed = 10f;

    void Start()
    {
        player = GameDirector.instance.PlayerControl.gameObject;

        moveDir = player.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(moveDir).normalized;
        transform.eulerAngles -= new Vector3(90f, 0f, 0f);

        StartCoroutine(ObjectDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir.normalized * Time.deltaTime * speed;
    }

    IEnumerator ObjectDestroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Debug.Log("플레이어 공격 성공! 여기에 데미지 넣기");
            Destroy(gameObject);
        }
    }
}
