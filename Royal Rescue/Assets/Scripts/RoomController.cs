using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public CameraFollow roomCamera;
    public Room[] rooms;

    private Room currentRoom = null;
    private Room previousRoom = null;
    public Room CurrentRoom => currentRoom;

    void Start()
    {
        rooms = GetComponentsInChildren<Room>(true);
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].Init(this, i);
            rooms[i].gameObject.SetActive(false);
        }
        // 현재 스테이지의 첫 번째 방(index: 0)이 hierarchy의 맨 위에 있어야 합니다.
        roomCamera.transform.SetParent(rooms[0].transform);
        currentRoom = rooms[0];
        currentRoom.gameObject.SetActive(true);

        roomCamera.ResetCameraPosition(this);
        roomCamera.SetCameraFollow(true);
    }

    public void SwitchRoom(int nextRoomId)
    {
        roomCamera.SetCameraFollow(false);
        roomCamera.transform.SetParent(rooms[nextRoomId].transform);

        previousRoom = CurrentRoom;
        currentRoom = rooms[nextRoomId];
        currentRoom.gameObject.SetActive(true);

        roomCamera.ResetCameraPosition(this);
        previousRoom.gameObject.SetActive(false);
    }
}
