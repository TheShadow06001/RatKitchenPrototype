using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_LevelManager : MonoBehaviour
{
    public static S_LevelManager Instance;
    
    [Header("Level Manager")]
    [SerializeField] private GameObject LoadingScreenCanvas;
    [SerializeField] private Image LoadingScreenBar;
    
    
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject); //Singleton Setup for LevelManager
        }
    }

    public async void LoadLevel(string LevelName)
    {
        var scene = SceneManager.LoadSceneAsync(LevelName);
        scene.allowSceneActivation = false; //Prevent Level from appearing on screen
        
        LoadingScreenCanvas.SetActive(true);

        do {
            LoadingScreenBar.fillAmount = scene.progress;
        } while (scene.progress < 0.9f); //Update fill ammount of loading bar, Await perhaps to not overload?
        
        scene.allowSceneActivation = true; //Allow Loaded level to appear on screen
        LoadingScreenCanvas.SetActive(false); //Remove Loading Canvas
        
    }
}
