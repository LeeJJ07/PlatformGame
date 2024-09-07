using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Skip : MonoBehaviour
{
    private Room currentRoom => GameDirector.instance.CurrentRoomControl.CurrentRoom;
    private RoomController roomController;
    private RoomPortal roomPortal;
    public Room[] rooms => GameDirector.instance.CurrentRoomControl.rooms;

    private int i = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            SwitchRoom(currentRoom.RoomId + 1);

        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchRoom(currentRoom.RoomId - 1);
        }

    }
    private void SwitchRoom(int roomId)
    {
        if (roomId < 0 || roomId >= rooms.Length)
            return;

        currentRoom.RoomControl.SwitchRoom(roomId);
        roomPortal = currentRoom.GetComponentInChildren<RoomPortal>(true);
        GameDirector.instance.PlayerControl.transform.position = roomPortal.TeleportPosition.position;
        currentRoom.RoomControl.roomCamera.SetCameraFollow(true);
    }
}
