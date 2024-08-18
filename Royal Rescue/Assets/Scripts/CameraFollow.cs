using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private bool followPlayer;

    private RoomController roomControl;
    [SerializeField] private float cameraDepth, cameraHorizontalOffset, cameraBotOffset, cameraTopOffset;
    /*
        Room 게임 오브젝트를 기준으로 카메라가 이동할 x, y 경계를 정한다.
        Room 게임 오브젝트는 해당 방에 존재하는 모든 오브젝트들을 자식 오브젝트로 가진다.
    */
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 nextPos;
    private Transform target;

    private bool enableFollow = false;
    private float leftLimit, rightLimit, botLimit, topLimit, limitX, limitY;

    void Start()
    {
        SetCameraFollow(true);

        if (followPlayer)
            target = GameDirector.instance.PlayerControl.transform;
    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (target == null || !enableFollow)
            return;
        
        nextPos = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);

        leftLimit = roomControl.CurrentRoom.cameraLeftBound.position.x + cameraHorizontalOffset;
        rightLimit = roomControl.CurrentRoom.cameraRightBound.position.x - cameraHorizontalOffset;
        botLimit = roomControl.CurrentRoom.cameraLeftBound.position.y + cameraBotOffset;
        topLimit = roomControl.CurrentRoom.cameraTopBound.position.y - cameraTopOffset;

        if (leftLimit > rightLimit)
        {
            Debug.LogWarning("카메라의 offset을 너무 크게 설정하셨습니다. 오른쪽 오프셋을 자동 설정합니다.");
            rightLimit = leftLimit + 1;
        }
        if (botLimit > topLimit)
        {
            Debug.LogWarning("카메라의 offset을 너무 크게 설정하셨습니다. 위쪽 오프셋을 자동 설정합니다.");
            topLimit = botLimit + 1;
        }
        limitX = Mathf.Clamp(nextPos.x, leftLimit, rightLimit);
        limitY = Mathf.Clamp(nextPos.y, botLimit, topLimit);
        transform.position = new Vector3(limitX, limitY, transform.position.z);
    }

    public void ResetCameraPosition(RoomController roomControl)
    {
        if (this.roomControl == null)
            this.roomControl = roomControl;
        
        leftLimit = roomControl.CurrentRoom.cameraLeftBound.position.x + cameraHorizontalOffset;
        rightLimit = roomControl.CurrentRoom.cameraRightBound.position.x - cameraHorizontalOffset;

        if (leftLimit > rightLimit)
        {
            Debug.LogWarning("카메라의 offset을 너무 크게 설정하셨습니다. 오른쪽 오프셋을 자동 설정합니다.");
            rightLimit = leftLimit + 1;
        }
        limitX = Mathf.Clamp(transform.position.x, leftLimit, rightLimit);
        limitY = roomControl.CurrentRoom.cameraLeftBound.position.y + cameraBotOffset;
        transform.position = new Vector3(limitX, limitY, cameraDepth);
    }

    public void SetCameraFollow(bool isFollowing)
    {
        enableFollow = isFollowing;
    }
}
