using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossCam : DoorTrap
{
    private MiniBossAI boss;
    [SerializeField] protected Camera bossCamera;
    protected override void Start()
    {
        boss = monsterHub.GetComponentInChildren<MiniBossAI>(true);
        trapTrigger = GetComponent<BoxCollider>();
    }

    protected override IEnumerator TrapPlayer()
    {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        portal.gameObject.SetActive(false);
        SwitchCamera(mainCamera, doorCamera);
        CloseIronWall();
        yield return new WaitForSeconds(1f);

        SwitchCamera(mainCamera, doorCamera);

        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
        StartCoroutine(BossPlayer());
    }

    protected IEnumerator BossPlayer()
    {
        SwitchCamera(mainCamera, bossCamera);

        yield return new WaitForSeconds(1f);
        SwitchCamera(bossCamera, mainCamera);
        yield return new WaitForSeconds(0.5f);
        boss.hpbarUi.ActivateUI();
    }    

    protected override bool CheckRoomClear()
    {
        return !(boss.gameObject.activeSelf);
    }
}
