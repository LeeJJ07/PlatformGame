using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneBehavior : MonoBehaviour
{
    [SerializeField]Material dangerZoneMaterial;
    [SerializeField, Range(0,2f)] float alphaChangeSpeed = 1;
    int sign = 1;
    Color dangerzoneNewColor = new Color();
    [SerializeField, Range(0, 1f)]
    float min = 0.5f, max = 0.9f;

    private void Start()
    {
        
        dangerzoneNewColor = dangerZoneMaterial.color;
        dangerzoneNewColor.a = 0.5f;
    }
    void Update()
    {
        if(dangerzoneNewColor.a >= max)
        {
            sign *= -1;
        }
        else if(dangerzoneNewColor.a <= min)
        {
            sign *= -1;
        }
        dangerzoneNewColor.a -= alphaChangeSpeed * sign * Time.deltaTime;
        dangerZoneMaterial.color = dangerzoneNewColor;
    }
}
