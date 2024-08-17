using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamManager : MonoBehaviour
{
    public Transform camTarget;

    public Vector2Int minRange;
    public Vector2Int maxRange;

    [Range(0f, 2f)]
    public float distX = 1.0f;//타켓과의 거리

    [Range(0f, 2f)]
    public float distY = 1.0f;

    [Range(1.0f, 10.0f)]
    public float smoothX = 5.0f;//추적시 부드러움 정도

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
            camPosX = Mathf.Lerp(transform.position.x, camTarget.position.x, smoothX * Time.deltaTime);//X축 추적
        }
        if (checkDistanceY())
        {
            camPosY = Mathf.Lerp(transform.position.y, camTarget.position.y, smoothY * Time.deltaTime);//Y축 추적
        }

        camPosX = Mathf.Clamp(camPosX, minRange.x,maxRange.x);//X축 추적
        camPosY = Mathf.Clamp(camPosY, minRange.y, maxRange.y);//Y축 추적

        transform.position = new Vector3(camPosX, camPosY, transform.position.z);//위치 갱신
    }
    // Update is called once per frame
    void Update()
    {
        cameraTracking();
    }
}
