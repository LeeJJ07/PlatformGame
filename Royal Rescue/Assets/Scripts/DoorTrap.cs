using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    [SerializeField] private Camera mainCamera, cutSceneCamera;
    [SerializeField] private Animator ironWallAnim;
    [SerializeField] private RoomPortal portal;
    private bool isTrapActivated = false;
    
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
        SwitchCamera();
        CloseIronWall();
        yield return new WaitForSeconds(1f);
        
        SwitchCamera();
    }
    private void CloseIronWall()
    {
        portal.gameObject.SetActive(false);
        ironWallAnim.Play("IronWall_close");
    }
    private void OpenIronWall()
    {
        portal.gameObject.SetActive(false);
        ironWallAnim.Play("IronWall_open");
    }
    private void SwitchCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        cutSceneCamera.enabled = !cutSceneCamera.enabled;
    }
}

