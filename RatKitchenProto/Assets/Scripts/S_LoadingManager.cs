using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_LoadingManager : MonoBehaviour
{
    public static S_LoadingManager Instance;

    [Header("Level Manager")]
    [SerializeField] private GameObject LoadingScreenCanvas;
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
    
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelRoutine(levelName));
        Debug.Log("Loading Level: ");
        Debug.Log(levelName);
    }

    private IEnumerator LoadLevelRoutine(string levelName)
    {
        var scene = SceneManager.LoadSceneAsync(levelName);
        if (scene != null)
        {
            scene.allowSceneActivation = false;

            if (LoadingScreenCanvas != null)
                LoadingScreenCanvas.SetActive(true);

            while (scene.progress < 0.9f) //Unity why is it not 1
            {
                if (LoadingScreenBar != null)
                {
                    LoadingScreenBar.fillAmount = Mathf.Clamp01(scene.progress / 0.9f);
                }

                yield return null;
            }

            if (LoadingScreenBar != null)
                LoadingScreenBar.fillAmount = 1f;

            yield return new WaitForSeconds(0.2f);

            scene.allowSceneActivation = true;
        }
    }
}