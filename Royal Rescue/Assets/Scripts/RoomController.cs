using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public CameraFollow roomCamera;
    public Room[] rooms;

    private Room currentRoom = null;
    public Room CurrentRoom => currentRoom;

    void Awake()
    {
        rooms = GetComponentsInChildren<Room>(true);
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].Init(this, i);
        }
        // 현재 스테이지의 첫 번째 방(index: 0)이 hierarchy의 맨 위에 있어야 합니다.
        roomCamera.transform.SetParent(rooms[0].transform);
        currentRoom = rooms[0];

        roomCamera.ResetCameraPosition(this);
    }

    public void SwitchRoom(int nextRoomId)
    {
        roomCamera.SetCameraFollow(false);
        roomCamera.transform.SetParent(rooms[nextRoomId].transform);
        roomCamera.ResetCameraPosition(this);

        currentRoom = rooms[nextRoomId];
    }
}
