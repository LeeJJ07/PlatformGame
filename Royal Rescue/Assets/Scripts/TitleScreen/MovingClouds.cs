using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    [SerializeField] private Transform startPoint, endPoint;
    [SerializeField] private float speed;

    void Update()
    {
        MoveClouds();
    }

    private void MoveClouds()
    {
        foreach (Transform cloud in transform)
        {
            cloud.Translate(cloud.forward * speed * Time.deltaTime);
            
            if (cloud.transform.position.z >= endPoint.position.z)
                cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, startPoint.position.z);
        }
    }
}
