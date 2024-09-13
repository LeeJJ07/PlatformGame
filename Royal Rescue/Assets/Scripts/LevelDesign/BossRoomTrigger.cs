using System.Collections;
using UnityEngine;

public class BossRoomTrigger : DoorTrap
{
    [SerializeField] private Animator cutsceneCamAnim, bossDeathCamAnim, bossAnim;
    [SerializeField] private Camera bossDeathCamera;

    private BossBehaviour boss;
    private DieNode bossDeathNode;

    protected override void Start()
    {
        cutsceneCamAnim.gameObject.SetActive(false);
        boss = monsterHub.GetComponentInChildren<BossBehaviour>(true);
        bossDeathNode = (DieNode)boss.GetBossDeathNode();
    }
    protected override IEnumerator TrapPlayer()
    {
        yield return base.TrapPlayer();
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        cutsceneCamAnim.gameObject.SetActive(true);

        float cameraAnimationLength = AnimationHash.GetAnimationLength(cutsceneCamAnim, "GuillotineCam_cage");
        cutsceneCamAnim.Play(AnimationHash.BOSSROOM_CUTSCENE_CAGE, -1, 0f);
        yield return new WaitForSeconds(cameraAnimationLength + 1f);

        boss.ActivateBoss();

        cameraAnimationLength = AnimationHash.GetAnimationLength(cutsceneCamAnim, "GuillotineCam_boss_appear");
        cutsceneCamAnim.Play(AnimationHash.BOSSROOM_CUTSCENE_BOSS_APPEAR, -1, 0f);
        yield return new WaitForSeconds(cameraAnimationLength);

        yield return new WaitForSeconds(1.8f);

        cameraAnimationLength = AnimationHash.GetAnimationLength(cutsceneCamAnim, "GuillotineCam_boss_scream");
        cutsceneCamAnim.Play(AnimationHash.BOSSROOM_CUTSCENE_BOSS_SCREAM, -1, 0f);
        yield return new WaitForSeconds(cameraAnimationLength);

        cameraAnimationLength = AnimationHash.GetAnimationLength(cutsceneCamAnim, "GuillotineCam_scream_shake");
        cutsceneCamAnim.Play(AnimationHash.BOSSROOM_CUTSCENE_SHAKE, -1, 0f);
        yield return new WaitForSeconds(cameraAnimationLength);

        cameraAnimationLength = AnimationHash.GetAnimationLength(cutsceneCamAnim, "GuillotineCam_player");
        cutsceneCamAnim.Play(AnimationHash.BOSSROOM_CUTSCENE_PLAYER, -1, 0f);
        yield return new WaitForSeconds(cameraAnimationLength + 1f);

        cutsceneCamAnim.gameObject.SetActive(false);
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }
    protected override IEnumerator ReleasePlayer()
    {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);

        Vector3 deathCamPos = bossDeathCamera.transform.position;
        bool isBossFlipped = boss.transform.rotation.y >= 0.6f && boss.transform.rotation.y <= 0.8f;

        if (isBossFlipped)
        {
            bossDeathCamera.transform.position = new Vector3(boss.transform.position.x + 10f, deathCamPos.y, deathCamPos.z);
            bossDeathCamera.transform.localEulerAngles += new Vector3(0, -80, 0);
        }
        else
            bossDeathCamera.transform.position = new Vector3(boss.transform.position.x - 17f, deathCamPos.y, deathCamPos.z);

        SwitchCamera(mainCamera, bossDeathCamera);
        bossDeathCamAnim.Play(AnimationHash.BOSSROOM_CUTSCENE_BOSS_DEATH);
        yield return new WaitForSeconds(3f);

        SwitchCamera(bossDeathCamera, doorCamera);
        OpenIronWall();
        yield return new WaitForSeconds(1.5f);

        SwitchCamera(doorCamera, mainCamera);
        portal.gameObject.SetActive(true);

        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }

    protected override bool CheckRoomClear()
    {
        return bossDeathNode.IsActiveAnime;
    }
}
