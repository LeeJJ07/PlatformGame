using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameDirector : MonoBehaviour
{
    public static GameDirector instance => _instance;
    public RoomController CurrentRoomControl => currentRoomControl;
    public bool IsLoadingScreen => loadingScreenCam.enabled;

    public PlayerControlManagerFix PlayerControl
    {
        get
        {
            if (playerControl == null)
                playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerControlManagerFix>();
            return playerControl;
        }
    }
    private static GameDirector _instance = null;
    private PlayerControlManagerFix playerControl;
    private RoomController currentRoomControl;

    [SerializeField] private GameObject uiCanvas, playerUiCanvas, playerInventoryCanvas, loadScreenFade;
    [SerializeField] private Camera loadingScreenCam;
    [SerializeField] private Animator loadscreenFadeAnim, shroomAnim, respawnAnim;
    [SerializeField] private float loadScreenDelay, respawnDelay;
    [SerializeField] private List<StageInfo> stageInfos;
    
    private int stageIndex = -1;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            SetCurrentStageIndex();
            uiCanvas.SetActive(false);
            loadScreenFade.SetActive(true);

            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void SetCurrentStageIndex()
    {
        Scene scene = SceneManager.GetActiveScene();
        for (int i = 0; i < stageInfos.Count; i++)
        {
            if (stageInfos[i].stageName == scene.name)
            {
                stageIndex = i;
                return;
            }
        }
        stageIndex = -1;
    }

    public IEnumerator LoadNextStage()
    {
        SetStageLoopBgm(false);

        ++stageIndex;
        if (stageIndex == stageInfos.Count)
            stageIndex = 0;

        AltarControl.ResetAltar();
        PlayerControl.transform.SetParent(transform);
        PlayerControl.FixatePlayerRigidBody(true);
        SetPlayerInventoryUI(false);

        yield return new WaitForSeconds(0.1f);
        var asyncLoadStage = SceneManager.LoadSceneAsync(stageInfos[stageIndex].stageName, LoadSceneMode.Single);

        while (asyncLoadStage.progress < 0.9f)
        {
            yield return null;
        }
        StartCoroutine(ExitLoadingScreen());
    }

    public void LoadTitleScreen()
    {
        PlayerControl.transform.SetParent(transform);
        PlayerControl.ResetPlayerStatus();
        SetStageLoopBgm(false);
        stageIndex = -1;
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        SetStageLoopBgm(true);
    }

    public void ShowLoadingScreen()
    {
        SetPlayerStatusUI(false);

        uiCanvas.gameObject.SetActive(true);
        loadingScreenCam.enabled = true;
        shroomAnim.Play(AnimationHash.SHROOM_CHASE, -1, 0f);
    }

    IEnumerator ExitLoadingScreen()
    {
        yield return new WaitForSeconds(loadScreenDelay);

        loadscreenFadeAnim.Play(AnimationHash.LOADSCREEN_FADEOUT);
        yield return new WaitForSeconds(loadScreenDelay);

        respawnAnim.Play(AnimationHash.RESPAWN_SCREEN_HIDE);
        SetPlayerStatusUI(true);

        loadingScreenCam.enabled = false;
        uiCanvas.gameObject.SetActive(false);
        PlayerControl.FixatePlayerRigidBody(false);
        SetStageLoopBgm(true);
    }

    public IEnumerator RespawnScreenTransition()
    {
        yield return new WaitForSeconds(respawnDelay);

        respawnAnim.Play(AnimationHash.RESPAWN_SCREEN_SHOW);
        yield return new WaitForSeconds(0.8f);

        currentRoomControl.SetPlayerRespawnPosition(); 
        PlayerControl.RevivePlayer();

        respawnAnim.Play(AnimationHash.RESPAWN_SCREEN_HIDE);
    }

    public void SetCurrentRoomControl(RoomController roomControl)
    {
        currentRoomControl = roomControl;
    }

    public void SetPlayerStatusUI(bool state)
    {
        if (!_instance) return;
        playerUiCanvas.SetActive(state);
    }

    public void SetPlayerInventoryUI(bool state)
    {
        playerInventoryCanvas.SetActive(state);
    }

    public void SetCursorVisibility(bool state)
    {
        Cursor.visible = state;
    }

    public void SetStageLoopBgm(bool isPlaying)
    {
        string bgmName = stageIndex == -1 ? "BlizzardCastle" : stageInfos[stageIndex].loopBgm;

        if (isPlaying)
            SoundManager.Instance.PlaySound(bgmName, true, SoundType.BGM);
        else
            SoundManager.Instance.StopLoopSound(bgmName);
    }
}

[System.Serializable]
public class StageInfo
{
    public string stageName;
    public string loopBgm;
}