using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossCam : DoorTrap
{
    private MiniBossAI boss;
    [SerializeField] private GameObject reward2;
    [SerializeField] protected Camera bossCamera;
    [SerializeField] protected Animator ironWallAnim2;
    [SerializeField] protected RoomPortal portal2;

    protected override void Start()
    {
        boss = monsterHub.GetComponentInChildren<MiniBossAI>(true);
        trapTrigger = GetComponent<BoxCollider>();
    }

    protected override IEnumerator TrapPlayer()
    {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        portal.gameObject.SetActive(false);
        portal2.gameObject.SetActive(false);
        SwitchCamera(mainCamera, doorCamera);
        
        ironWallAnim.Play("IronWall_close");
        ironWallAnim2.Play("IronWall_close");
        SoundManager.Instance.PlaySound("cell_bars_close");
        yield return new WaitForSeconds(1f);

        SwitchCamera(mainCamera, doorCamera);
        StartCoroutine(BossPlayer());
    }
    protected override IEnumerator ReleasePlayer()
    {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        SwitchCamera(mainCamera, gemCamera);
        yield return new WaitForSeconds(0.2f);

        if (reward)
        {
            reward.SetActive(true);
            reward2.SetActive(true);
        }
        yield return new WaitForSeconds(1f);

        SwitchCamera(gemCamera, doorCamera);
        ironWallAnim.Play("IronWall_open");
        ironWallAnim2.Play("IronWall_open");
        SoundManager.Instance.PlaySound("cell_bars_open");

        yield return new WaitForSeconds(1.5f);

        SwitchCamera(doorCamera, mainCamera);
        trapTrigger.enabled = false;
        portal.gameObject.SetActive(true);
        portal2.gameObject.SetActive(true);

        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }
    protected IEnumerator BossPlayer()
    {
        SwitchCamera(mainCamera, bossCamera);

        SoundManager.Instance.PlaySound("MiniBossStartSound");
        yield return new WaitForSeconds(1f);
        SwitchCamera(bossCamera, mainCamera);
        yield return new WaitForSeconds(0.5f);
        
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
        boss.hpbarUi.ActivateUI();
    }    

    protected override bool CheckRoomClear()
    {
        return !(boss.gameObject.activeSelf);
    }
}
