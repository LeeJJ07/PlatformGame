using UnityEngine;

public class WarningZoneBehavior : MonoBehaviour,ITag
{
    [Header("Detail Tag"), SerializeField]
    string detailTag = "";
    [SerializeField] Color32 beginColor;
    [SerializeField]Material warningZoneMaterial;
    [SerializeField, Range(0,5f)] float alphaChangeSpeed = 1;
    [SerializeField, Range(0, 1f)]
    float min = 0.5f, max = 0.9f;

    int ColorChangeSign = 1;
    Color warningZoneNewColor = new Color();

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
        warningZoneNewColor= beginColor;
    }
    void Update()
    {
        if(warningZoneNewColor.a >= max)
        {
            warningZoneNewColor.a = max;
            ColorChangeSign *= -1;
        }
        else if(warningZoneNewColor.a <= min)
        {
            warningZoneNewColor.a = min;
            ColorChangeSign *= -1;
        }
        warningZoneNewColor.a -= alphaChangeSpeed * ColorChangeSign * Time.deltaTime;
        warningZoneMaterial.color = warningZoneNewColor;
    }
}
