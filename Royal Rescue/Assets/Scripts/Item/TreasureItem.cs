using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TreasureItem : MonoBehaviour
{
    float time = 0.0f;
    float speed = 30.0f;

    GameObject player;
    void Start()
    {
        player = GameDirector.instance.PlayerControl.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime;
        if (time < 1.5f)
            return;
        transform.position += (player.transform.position - transform.position).normalized * Time.deltaTime * speed;
    }
}
