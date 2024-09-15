using UnityEngine;

public class AnimationHash
{
    public static readonly int COFFINDOOR_OPEN               = Animator.StringToHash("CoffinDoor_open");
    public static readonly int SPRING                        = Animator.StringToHash("Spring");
    public static readonly int PORTALCAM_ZOOM                = Animator.StringToHash("StagePortalCamera_zoom");
    public static readonly int LOADSCREEN_FADEOUT            = Animator.StringToHash("LoadScreenFade_Out");
    public static readonly int CRUMBLEPLATFORM_RESPAWN       = Animator.StringToHash("CrumblePlatform_respawn");
    public static readonly int CRUMBLEPLATFORM_SHAKE         = Animator.StringToHash("CrumblePlatform_shake");
    public static readonly int CRUMBLEPLATFORM_FALL          = Animator.StringToHash("CrumblePlatform_crumble");
    public static readonly int SHROOM_CHASE                  = Animator.StringToHash("Chase");
    public static readonly int PLAYER_IDLE                   = Animator.StringToHash("Idle");
    public static readonly int RESPAWN_SCREEN_SHOW           = Animator.StringToHash("RespawnTransition_show");
    public static readonly int RESPAWN_SCREEN_HIDE           = Animator.StringToHash("RespawnTransition_hide");
    public static readonly int TITLESCREENCAM_INTRO          = Animator.StringToHash("TitleScreenCam_intro");
    public static readonly int BOSSROOM_CUTSCENE_CAGE        = Animator.StringToHash("GuillotineCam_cage");
    public static readonly int BOSSROOM_CUTSCENE_PLAYER      = Animator.StringToHash("GuillotineCam_player");
    public static readonly int BOSSROOM_CUTSCENE_BOSS_APPEAR = Animator.StringToHash("GuillotineCam_boss_appear");
    public static readonly int BOSSROOM_CUTSCENE_BOSS_SCREAM = Animator.StringToHash("GuillotineCam_boss_scream");
    public static readonly int BOSSROOM_CUTSCENE_SHAKE       = Animator.StringToHash("GuillotineCam_scream_shake");
    public static readonly int BOSSROOM_CUTSCENE_BOSS_DEATH  = Animator.StringToHash("BossDeathCam_focus");
    public static readonly int ENDING_CUTSCENE_RESCUE        = Animator.StringToHash("EndingCutscene_rescue");
    public static readonly int ENDING_CUTSCENE_EXIT          = Animator.StringToHash("EndingCutscene_exit");
    public static readonly int PLAYER_FULLSPIN               = Animator.StringToHash("FullSpin");
    public static readonly int PLAYER_FALL_END               = Animator.StringToHash("Fall_End");
    public static readonly int PLAYER_WALK                   = Animator.StringToHash("Walk");
    public static readonly int PLAYER_VICTORY                = Animator.StringToHash("Victory");
    public static readonly int PRINCESS_IDLE                 = Animator.StringToHash("Idle");
    public static readonly int PRINCESS_FALL_START           = Animator.StringToHash("Fall_Start");
    public static readonly int PRINCESS_FALL_END             = Animator.StringToHash("Fall_End");
    public static readonly int PRINCESS_WALK                 = Animator.StringToHash("Walk");
    public static readonly int PRINCESS_VICTORY              = Animator.StringToHash("Victory");


    public static float GetAnimationLength(Animator anim, string animName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animName)
                return clip.length;
        }
        return 0f;
    }
}
