using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamManager : MonoBehaviour
{
    public Transform camTarget;

    public Vector2Int minRange;
    public Vector2Int maxRange;

    [Range(0f, 2f)]
    public float distX = 1.0f;//Ÿ�ϰ��� �Ÿ�

    [Range(0f, 2f)]
    public float distY = 1.0f;

    [Range(1.0f, 10.0f)]
    public float smoothX = 5.0f;//������ �ε巯�� ����

    [Range(1.0f, 10.0f)]
    public float smoothY = 5.0f;

    private GizmoTracking gt;
    // Start is called before the first frame update
    void Start()
    {
        camTarget = GameObject.FindWithTag("CameraTarget").transform;
        gt = GameObject.Find("GizmoZone").GetComponent<GizmoTracking>();
        minRange = gt.minXAndY;
        maxRange = gt.maxXAndY;
    }

    bool checkDistanceX()
    {
        return Mathf.Abs(transform.position.x - camTarget.position.x) > distX;
    }
    bool checkDistanceY()
    {
        return Mathf.Abs(transform.position.y - camTarget.position.y) > distY;
    }
    void cameraTracking()
    {
        float camPosX = this.transform.position.x;
        float camPosY = this.transform.position.y;

        if (checkDistanceX())
        {
            camPosX = Mathf.Lerp(transform.position.x, camTarget.position.x, smoothX * Time.deltaTime);//X�� ����
        }
        if (checkDistanceY())
        {
            camPosY = Mathf.Lerp(transform.position.y, camTarget.position.y, smoothY * Time.deltaTime);//Y�� ����
        }

        camPosX = Mathf.Clamp(camPosX, minRange.x,maxRange.x);//X�� ����
        camPosY = Mathf.Clamp(camPosY, minRange.y, maxRange.y);//Y�� ����

        transform.position = new Vector3(camPosX, camPosY, transform.position.z);//��ġ ����
    }
    // Update is called once per frame
    void Update()
    {
        cameraTracking();
    }
}
