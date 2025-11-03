using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_LevelManager : MonoBehaviour
{
    public static S_LevelManager Instance;

    [Header("Canvas Refs")] 
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject LoadingScreenCanvas;
    [Header("Loading Screen Refs")] 
    [SerializeField] private Slider LoadingScreenBarR;
    [SerializeField] private Slider LoadingScreenBarL;
    [SerializeField] private Image FadeImage;
    [SerializeField] private TMP_Text LoadingText;

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Singleton
        }

        #endregion
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(string LevelName)
    {
        MainMenuCanvas.SetActive(false);
        LoadingScreenCanvas.SetActive(true);
        StartCoroutine(LoadLevelAsync(LevelName));
    }

    private IEnumerator LoadLevelAsync(string LevelName)
    {
        var LoadOperation = SceneManager.LoadSceneAsync(LevelName);
        LoadOperation.allowSceneActivation = false;

        float StartTime = Time.realtimeSinceStartup; // For minimum loading screen time
        const float MinLoadingTime = 3f;             // to make sure Scene doesn't load too fast or unload too slow

        if (LoadingText != null && LoadingScreenBarL != null && LoadingScreenBarR != null)
        {
            LoadingText.text = "Loading... 0%";
            LoadingScreenBarL.value = 0f;
            LoadingScreenBarR.value = 0f;
        }
        else
        {
            Debug.LogError("Loading Screen References are not set in LevelManager.", this);
        }

        while (!LoadOperation.isDone)
        {
            float Progress = Mathf.Clamp01(LoadOperation.progress / 0.9f);
            LoadingScreenBarR.value = Progress;
            LoadingScreenBarL.value = Progress;
            LoadingText.text = "Loading... " + (int)(Progress * 100f) + "%";
            yield return null;
        }
    }
}