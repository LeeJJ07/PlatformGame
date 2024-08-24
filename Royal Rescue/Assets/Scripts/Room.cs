using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomController RoomControl => roomControl;
    public int RoomId => roomId;

    public Transform cameraLeftBound, cameraRightBound, cameraTopBound;
    private RoomController roomControl;
    private int roomId;

    public void Init(RoomController control, int id)
    {
        roomControl = control;
        roomId = id;
    }
}
