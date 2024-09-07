using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameDirector : MonoBehaviour
{
    private static GameDirector _instance = null;
    public static GameDirector instance => _instance;

    public PlayerControlManagerFix PlayerControl
    {
        get
        {
            if (playerControl == null)
                playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerControlManagerFix>();
            return playerControl;
        }
    }
    private PlayerControlManagerFix playerControl;
    public RoomController CurrentRoomControl => currentRoomControl;
    [SerializeField] private RoomController currentRoomControl;

    [SerializeField] private GameObject uiCanvas, loadScreenFade;
    [SerializeField] private Camera loadingScreenCam;
    [SerializeField] private Animator loadscreenFadeAnim, shroomAnim, respawnAnim;
    [SerializeField] private float loadScreenDelay, respawnDelay;
    [SerializeField] private List<string> stageNames;
    private int stageIndex = 0;

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
        for (int i = 0; i < stageNames.Count; i++)
        {
            if (stageNames[i] == scene.name)
            {
                stageIndex = i;
                return;
            }
        }
        stageIndex = 0;
    }

    public IEnumerator LoadNextStage()
    {
        ++stageIndex;
        if (stageIndex >= stageNames.Count)
            stageIndex = stageNames.Count - 1;

        playerControl.transform.SetParent(transform);

        yield return new WaitForSeconds(0.1f);
        var asyncLoadStage = SceneManager.LoadSceneAsync(stageNames[stageIndex], LoadSceneMode.Single);

        while (asyncLoadStage.progress < 0.9f)
        {
            yield return null;
        }
        StartCoroutine(ExitLoadingScreen());
    }

    public void ShowLoadingScreen()
    {
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

        loadingScreenCam.enabled = false;
        uiCanvas.gameObject.SetActive(false);
    }

    public IEnumerator RespawnScreenTransition()
    {
        yield return new WaitForSeconds(respawnDelay);

        respawnAnim.Play(AnimationHash.RESPAWN_SCREEN_SHOW);
        yield return new WaitForSeconds(0.8f);

        currentRoomControl.SetPlayerRespawnPosition(); 
        playerControl.RevivePlayer();

        respawnAnim.Play(AnimationHash.RESPAWN_SCREEN_HIDE);
    }

    public void SetCurrentRoomControl(RoomController roomControl)
    {
        currentRoomControl = roomControl;
    }
}
