using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Skip : MonoBehaviour
{
    private Room currentRoom;
    private RoomController roomController;
    private RoomPortal roomPortal;
    public Room[] rooms;

    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        roomController = GetComponent<RoomController>();
        rooms = GetComponentsInChildren<Room>(true);
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].Init(roomController, i);
            rooms[i].gameObject.SetActive(false);
        }
        currentRoom = rooms[i];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (i < rooms.Length - 1)
            {
                currentRoom.RoomControl.SwitchRoom(++i);
                currentRoom = rooms[i];
                roomPortal = currentRoom.GetComponentInChildren<RoomPortal>(true);
                GameDirector.instance.PlayerControl.transform.position = roomPortal.TeleportPosition.position;
                currentRoom.RoomControl.roomCamera.SetCameraFollow(true);             
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (i > 0)
            {
                currentRoom.RoomControl.SwitchRoom(--i);
                currentRoom = rooms[i];
                roomPortal = currentRoom.GetComponentInChildren<RoomPortal>(true);
                GameDirector.instance.PlayerControl.transform.position = roomPortal.TeleportPosition.position;
                currentRoom.RoomControl.roomCamera.SetCameraFollow(true);
            }
        }

    }
}
