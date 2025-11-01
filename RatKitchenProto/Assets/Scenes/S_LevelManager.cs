using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_LevelManager : MonoBehaviour
{
    public static S_LevelManager Instance;

    [Header("Level Manager")] [SerializeField]
    private GameObject LoadingScreenCanvas;

    [SerializeField] private Image LoadingScreenBar;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Singleton
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelRoutine(levelName));
    }

    private IEnumerator LoadLevelRoutine(string levelName)
    {
        var scene = SceneManager.LoadSceneAsync(levelName);
        scene.allowSceneActivation = false;

        if (LoadingScreenCanvas != null)
            LoadingScreenCanvas.SetActive(true);

        // Show progress until Unity reports 0.9 (that's when it has loaded but not activated)
        while (scene.progress < 0.9f)
        {
            if (LoadingScreenBar != null)
            {
                // scene.progress goes 0..0.9, map it to 0..1 for the UI
                LoadingScreenBar.fillAmount = scene.progress;
            }

            yield return null; // wait one frame
        }

        yield return new WaitForSeconds(0.2f);

        scene.allowSceneActivation = true;
    }
}