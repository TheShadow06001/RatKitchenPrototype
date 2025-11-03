using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance;

    //public List<LevelSettings> allLevels;
    //public int currentLevel = 0;
    //public float elapsedTime = 0f;

    //public KitchenGenerator kitchenGenerator;
    ////public CameraController cameraController;

    //private void Awake()
    //{
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject);
    //}

    //private void Start()
    //{
    //    //apply settings
    //    ApplyLevelSettings(allLevels[currentLevel]);
    //}

    //private void Update()
    //{
    //    // if 15 min
    //    // end game
    //    elapsedTime += Time.deltaTime;
    //    if (elapsedTime >= 900f)
    //    {
    //        EndGame();
    //    }
    //}

    //public void NextLevel()
    //{
    //    currentLevel++;
    //    if (currentLevel >= allLevels.Count)
    //        currentLevel = allLevels.Count - 1;

    //    ApplyLevelSettings(allLevels[currentLevel]);
    //    kitchenGenerator.ResetKitchenGenerator();
    //    // cameraController.ResetToStart();
    //    // reset player to start position
    //}

    //private void ApplyLevelSettings(LevelSettings settings)
    //{
    //    kitchenGenerator.SetDifficulty(settings);
    //    // cameraController.SetSpeed(settings.cameraSpeed);
    //}

    //private void EndGame()
    //{
    //    Debug.LogWarning("15 min has passed! Game over");
        
    //    // call main menu? Game over view?
    //}
}
