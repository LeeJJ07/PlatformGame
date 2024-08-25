using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningZoneBehavior : MonoBehaviour,ITag
{
    [Header("Detail Tag"), SerializeField]
    string detailTag = "";
    [SerializeField]Material warningZoneMaterial;
    [SerializeField, Range(0,2f)] float alphaChangeSpeed = 1;
    int sign = 1;
    Color warningZoneNewColor = new Color();
    [SerializeField, Range(0, 1f)]
    float min = 0.5f, max = 0.9f;

    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }

    public string GetTag()
    {
        return detailTag;
    }

    private void Start()
    {
        
        warningZoneNewColor = warningZoneMaterial.color;
        warningZoneNewColor.a = 0.5f;
    }
    void Update()
    {
        if(warningZoneNewColor.a >= max)
        {
            sign *= -1;
        }
        else if(warningZoneNewColor.a <= min)
        {
            sign *= -1;
        }
        warningZoneNewColor.a -= alphaChangeSpeed * sign * Time.deltaTime;
        warningZoneMaterial.color = warningZoneNewColor;
    }
}
