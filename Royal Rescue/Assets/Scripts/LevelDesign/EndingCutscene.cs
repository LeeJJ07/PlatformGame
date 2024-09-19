using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCutscene : MonoBehaviour
{
    [SerializeField] private Animator cutsceneAnim, playerAnim, princessAnim;
    [SerializeField] private GameObject guillotineFlameThrowers;

    private bool canPlayEnding = false;

    void Update()
    {
        if (canPlayEnding && UIMenu.pressedConfirmBtn)
        {
            canPlayEnding = false;
            PlayEndingCutscene();
        }
    }

    public void PlayRescueCutscene()
    {
        guillotineFlameThrowers.SetActive(false);
        GameDirector.instance.SetPlayerStatusUI(false);
        GameDirector.instance.PlayerControl.SetPlayerEnabled(false);
        cutsceneAnim.Play(AnimationHash.ENDING_CUTSCENE_RESCUE, -1, 0f);
    }

    private void PlayEndingCutscene()
    {
        cutsceneAnim.Play(AnimationHash.ENDING_CUTSCENE_EXIT, -1, 0f);
    }

    void EnableEndingCutscene()
    {
        canPlayEnding = true;
    }

    void PlayPlayerJump()
    {
        playerAnim.Play(AnimationHash.PLAYER_FULLSPIN);
    }

    void PlayPlayerFallEnd()
    {
        playerAnim.Play(AnimationHash.PLAYER_FALL_END);
    }

    void PlayPlayerIdle()
    {
        playerAnim.Play(AnimationHash.PLAYER_IDLE);
    }

    void PlayPlayerVictory()
    {
        playerAnim.Play(AnimationHash.PLAYER_VICTORY);
    }

    void PlayPrincessFallStart()
    {
        princessAnim.Play(AnimationHash.PRINCESS_FALL_START);
    }

    void PlayPrincessFallEnd()
    {
        princessAnim.Play(AnimationHash.PRINCESS_FALL_END);
    }

    void PlayPrincessIdle()
    {
        princessAnim.Play(AnimationHash.PRINCESS_IDLE);
    }

    void PlayPrincessVictory()
    {
        princessAnim.Play(AnimationHash.PRINCESS_VICTORY);
    }

    void PlayPlayerWalk()
    {
        playerAnim.Play(AnimationHash.PLAYER_WALK);
    }

    void PlayPrincessWalk()
    {
        princessAnim.Play(AnimationHash.PRINCESS_WALK);
    }

    void LoadTitleScreen()
    {
        GameDirector.instance.LoadTitleScreen();
    }
}