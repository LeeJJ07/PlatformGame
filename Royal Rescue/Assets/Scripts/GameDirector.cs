using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameDirector : MonoBehaviour
{
    private static GameDirector _instance = null;
    public static GameDirector instance => _instance;

    public PlayerControlManagerFix PlayerControl => playerControl;
    private PlayerControlManagerFix playerControl;

    [SerializeField] private GameObject loadingScreen, loadScreenFade;
    [SerializeField] private Camera loadingScreenCam;
    [SerializeField] private Animator loadscreenFadeAnim;
    [SerializeField] private List<string> stageNames;
    private int stageIndex = 0;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Init()
    {
        playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerControlManagerFix>();
        loadingScreen.SetActive(false);
        loadScreenFade.SetActive(true);
    }

    public IEnumerator LoadNextStage()
    {
        ++stageIndex;
        if (stageIndex >= stageNames.Count)
            stageIndex = stageNames.Count - 1;

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
        loadingScreen.gameObject.SetActive(true);
        loadingScreenCam.enabled = true;
    }

    IEnumerator ExitLoadingScreen()
    {
        yield return new WaitForSeconds(1.5f);

        loadscreenFadeAnim.Play(AnimationHash.LOADSCREEN_FADEOUT);
        yield return new WaitForSeconds(1.5f);

        Camera mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        loadingScreenCam.enabled = false;
        mainCam.enabled = true;
        loadingScreen.gameObject.SetActive(false);
    }
}
