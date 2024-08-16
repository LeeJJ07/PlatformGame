using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private bool followPlayer;
    /*
        Room 게임 오브젝트를 기준으로 카메라가 이동할 x, y 경계를 정한다.
        Room 게임 오브젝트는 해당 방에 존재하는 모든 오브젝트들을 자식 오브젝트로 가진다.
    */
    [SerializeField] private float minX, maxX, minY, maxY;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 nextPos;
    private Transform target;

    void Start()
    {
        if (followPlayer)
            target = GameDirector.instance.PlayerControl.transform;
    }

    void FixedUpdate()
    {
       FollowTarget();
    }

    void FollowTarget()
    {
        if (target == null)
            return;
        
        nextPos = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);

        // z 위치 고정
        nextPos.z = transform.position.z;
        transform.position = nextPos;

        // x, y 한계점 설정
        float limitX = Mathf.Clamp(transform.localPosition.x, minX, maxX);
        float limitY = Mathf.Clamp(transform.localPosition.y, minY, maxY);
        transform.localPosition = new Vector3(limitX, limitY, transform.localPosition.z);
    }
}
