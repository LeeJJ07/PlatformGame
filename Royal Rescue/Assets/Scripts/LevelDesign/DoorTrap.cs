using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    [SerializeField] protected Camera mainCamera, doorCamera,gemCamera;
    [SerializeField] protected Animator ironWallAnim;
    [SerializeField] protected RoomPortal portal;
    [SerializeField] protected Transform monsterHub;
    [SerializeField] protected GameObject reward;
    protected BoxCollider trapTrigger;
    protected PlayerControlManagerFix playerControl;
    protected bool isTrapActivated = false;
    protected bool hasClearedRoom = false;

    private Monster[] monsters;

    protected virtual void Start()
    {
        monsters = monsterHub.GetComponentsInChildren<Monster>(true);
        trapTrigger = GetComponent<BoxCollider>();
    }

    protected virtual void Update()
    {
        if (!hasClearedRoom && CheckRoomClear())
        {
            hasClearedRoom = true;
            StartCoroutine(ReleasePlayer());
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!isTrapActivated && other.gameObject.CompareTag("Player"))
        {
            isTrapActivated = true;
            StartCoroutine(TrapPlayer());
        }
    }
    protected virtual IEnumerator TrapPlayer()
    {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        portal.gameObject.SetActive(false);
        SwitchCamera(mainCamera, doorCamera);
        CloseIronWall();
        yield return new WaitForSeconds(1f);
        
        SwitchCamera(mainCamera, doorCamera);

        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }
    protected virtual IEnumerator ReleasePlayer()
    {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        SwitchCamera(mainCamera, gemCamera);
        yield return new WaitForSeconds(0.2f);

        if (reward) reward.SetActive(true);
        yield return new WaitForSeconds(1f);

        SwitchCamera(gemCamera, doorCamera);
        OpenIronWall();
        yield return new WaitForSeconds(1.5f);
        
        SwitchCamera(doorCamera, mainCamera);
        trapTrigger.enabled = false;
        portal.gameObject.SetActive(true);

        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }
    protected void CloseIronWall()
    {
        ironWallAnim.Play("IronWall_close");
    }
    protected void OpenIronWall()
    {
        ironWallAnim.Play("IronWall_open");
    }
    protected void SwitchCamera(Camera currentCamera, Camera subCamera)
    {
        currentCamera.enabled = !currentCamera.enabled;
        subCamera.enabled = !doorCamera.enabled;
    }
    protected virtual bool CheckRoomClear()
    {
        foreach (Monster monster in monsters)
        {
            if (monster.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}

