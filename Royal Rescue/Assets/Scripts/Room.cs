using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomController RoomControl => roomControl;
    public int RoomId => roomId;

    [SerializeField] private int roomId;
    private RoomController roomControl;

    public void Init(RoomController control, int id)
    {
        roomControl = control;
        roomId = id;
    }
}
