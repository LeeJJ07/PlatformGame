using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private RoomController roomControl;
    [SerializeField] private bool enableFollow = false;
    [SerializeField] private float cameraDepth, cameraHorizontalOffset, cameraBotOffset, cameraTopOffset;
    /*
        Room 게임 오브젝트를 기준으로 카메라가 이동할 x, y 경계를 정한다.
        Room 게임 오브젝트는 해당 방에 존재하는 모든 오브젝트들을 자식 오브젝트로 가진다.
    */
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 nextPos;
    private Transform target;

    private float leftLimit, rightLimit, botLimit, topLimit, limitX, limitY;

    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (target == null || !enableFollow)
            return;

        nextPos = Vector3.SmoothDamp(transform.position, AdjustTargetCameraLevel(), ref velocity, smoothTime);

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

    public void Init(RoomController roomControl)
    {
        if (this.roomControl == null)
        {
            this.roomControl = roomControl;
            //target = GameDirector.instance.PlayerControl.transform;
        }
    }

    public void ResetCameraPosition()
    {
        //target.SetParent(roomControl.CurrentRoom.transform);

        limitX = roomControl.CurrentRoom.cameraLeftBound.position.x + cameraHorizontalOffset;
        limitY = roomControl.CurrentRoom.cameraLeftBound.position.y + cameraBotOffset;
        transform.position = new Vector3(limitX, limitY, transform.position.z);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, cameraDepth);
    }

    public void SetCameraFollow(bool isFollowing)
    {
        enableFollow = isFollowing;
    }

    public Vector3 AdjustTargetCameraLevel()
    {
        // 서 있는 땅의 높이 레벨에 따라 카메라를 좀 더 위로 올리도록 조정
        int groundLevel = (int)(target.localPosition.y / 4);
        return new Vector3(target.position.x, target.position.y + 2 * groundLevel, target.position.z);
    }

    public void SetFollowTarget(Transform newTarget = null)
    {
        //target = newTarget ? newTarget : GameDirector.instance.PlayerControl.transform;
    }
}
