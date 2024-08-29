using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    [SerializeField] private Camera mainCamera, doorCamera, gemCamera;
    [SerializeField] private Animator ironWallAnim;
    [SerializeField] private RoomPortal portal;
    [SerializeField] private Transform monsterHub;
    [SerializeField] private GameObject reward;
    private Monster[] monsters;
    private bool isTrapActivated = false;
    private bool hasClearedRoom = false;

    void Start()
    {
        monsters = monsterHub.GetComponentsInChildren<Monster>(true);
    }

    void Update()
    {
        if (!hasClearedRoom && CheckRoomClear())
        {
            hasClearedRoom = true;
            StartCoroutine(ReleasePlayer());
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!isTrapActivated && other.gameObject.CompareTag("Player"))
        {
            isTrapActivated = true;
            StartCoroutine(TrapPlayer());
        }
    }
    IEnumerator TrapPlayer()
    {
        portal.gameObject.SetActive(false);
        SwitchCamera(mainCamera, doorCamera);
        CloseIronWall();
        yield return new WaitForSeconds(1f);
        
        SwitchCamera(mainCamera, doorCamera);
    }
    IEnumerator ReleasePlayer()
    {
        SwitchCamera(mainCamera, gemCamera);
        yield return new WaitForSeconds(0.2f);

        reward.SetActive(true);
        yield return new WaitForSeconds(1f);

        SwitchCamera(gemCamera, doorCamera);
        OpenIronWall();
        yield return new WaitForSeconds(1.5f);
        
        SwitchCamera(doorCamera, mainCamera);
        portal.gameObject.SetActive(true);
    }
    private void CloseIronWall()
    {
        ironWallAnim.Play("IronWall_close");
    }
    private void OpenIronWall()
    {
        ironWallAnim.Play("IronWall_open");
    }
    private void SwitchCamera(Camera currentCamera, Camera subCamera)
    {
        currentCamera.enabled = !currentCamera.enabled;
        subCamera.enabled = !doorCamera.enabled;
    }
    bool CheckRoomClear()
    {
        foreach (Monster monster in monsters)
        {
            if (monster.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}

