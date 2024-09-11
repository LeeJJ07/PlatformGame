using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : DoorTrap
{
    [SerializeField] private Animator cutsceneCamAnim;

    private BossBehaviour boss;

    protected override void Start()
    {
        boss = monsterHub.GetComponentInChildren<BossBehaviour>(true);
    }

    protected override IEnumerator TrapPlayer()
    {
        yield return base.TrapPlayer();
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        cutsceneCamAnim.gameObject.SetActive(true);
        float cameraAnimationLength = AnimationHash.GetAnimationLength(cutsceneCamAnim, "GuillotineCam_cage");
        yield return new WaitForSeconds(cameraAnimationLength + 1f);

        cameraAnimationLength = AnimationHash.GetAnimationLength(cutsceneCamAnim, "GuillotineCam_player");
        cutsceneCamAnim.Play(AnimationHash.BOSSROOM_CUTSCENE_PLAYER, -1, 0f);
        yield return new WaitForSeconds(cameraAnimationLength + 1f);
        
        cutsceneCamAnim.gameObject.SetActive(false);
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }

    protected override bool CheckRoomClear()
    {
        return !(boss.gameObject.activeSelf);
    }

    protected override IEnumerator ReleasePlayer()
    {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        SwitchCamera(mainCamera, gemCamera);
        yield return new WaitForSeconds(0.2f);

        reward.SetActive(true);
        yield return new WaitForSeconds(1f);

        SwitchCamera(gemCamera, doorCamera);
        OpenIronWall();
        yield return new WaitForSeconds(1.5f);

        SwitchCamera(doorCamera, mainCamera);
        portal.gameObject.SetActive(true);

        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }
}
