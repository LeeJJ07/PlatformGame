using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPortal : MonoBehaviour
{
    public Transform TeleportPosition => teleportPos;
    public Room CurrentRoom => currentRoom;

    [SerializeField] private RoomPortal linkedPortal;
    [SerializeField] private Transform teleportPos;
    private Room currentRoom;

    void Awake()
    {
        currentRoom = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if (linkedPortal == null)
        {
            //Debug.LogWarning("다음으로 이동할 포탈이 연동되지 않았습니다.");
            return;
        }
        int nextRoomId = linkedPortal.CurrentRoom.RoomId;
        currentRoom.RoomControl.SwitchRoom(nextRoomId);
        TeleportPlayer();
    }
    public void TeleportPlayer()
    {
        GameDirector.instance.PlayerControl.transform.position = linkedPortal.TeleportPosition.position;
        linkedPortal.CurrentRoom.RoomControl.roomCamera.SetCameraFollow(true);
    }
    public void TeleportPlayer(Vector3 teleportPos)
    {
        GameDirector.instance.PlayerControl.transform.position = teleportPos;
    }
}
