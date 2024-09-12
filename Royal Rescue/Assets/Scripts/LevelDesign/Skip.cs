using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Skip : MonoBehaviour
{
    private Room currentRoom => GameDirector.instance.CurrentRoomControl.CurrentRoom;
    private RoomController roomController;
    private RoomPortal roomPortal;
    public Room[] rooms => GameDirector.instance.CurrentRoomControl.rooms;

    private AltarPortal altarPortal;
        
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.K))
        {
            if(GameObject.Find("RoomControl") != null)
                SwitchRoom(currentRoom.RoomId + 1);             
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (GameObject.Find("RoomControl") != null)
                SwitchRoom(currentRoom.RoomId - 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (GameObject.Find("AltarPortal") != null)
            {
                altarPortal = GameObject.Find("AltarPortal").GetComponent<AltarPortal>();
                altarPortal.TryOpenStagePortal(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameObject.Find("RoomControl") != null)
            {
                Monster[] monsters = currentRoom.GetComponentsInChildren<Monster>(true);
                foreach (Monster monster in monsters)
                {
                    monster.gameObject.SetActive(false);
                }
            }
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
