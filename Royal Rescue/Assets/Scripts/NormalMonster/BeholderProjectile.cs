using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BeholderProjectile : MonoBehaviour
{
    PlayerControlManagerFix player;
    Vector3 moveDir;
    [SerializeField] GameObject explosion;
    [SerializeField] float speed = 10f;
    [SerializeField] int damage = 10;

    void Start()
    {
        player = GameDirector.instance.PlayerControl;

        moveDir = player.gameObject.transform.position - transform.position;

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
            player.HurtPlayer(damage + Random.Range(0, 6));
            Destroy(gameObject);
        }
    }
}
