using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFindTarget : MonoBehaviour
{
    [SerializeField]RouteFinder routeFinder;
    [SerializeField] Transform target;
    List<Vector3> routes =new List<Vector3>();
    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
             routes=routeFinder.FindRoute(transform.position, target.position);
        }
        Debug.Log(routes.Count);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(routes.Count>0)
        for (int i = 1; i < routes.Count; i++)
            Gizmos.DrawLine(routes[i - 1], routes[i]);
    }
}
