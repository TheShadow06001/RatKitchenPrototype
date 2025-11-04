using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Current Level")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private LevelSettings currentSettings; // manual override of level settings

    [Header("Scaling")]
    public bool useDynamicScaling = true;
    public float maxLevel = 10f; // time-based instead? or just set it very high?

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public int CurrentLevel => currentLevel;
    public LevelSettings CurrentSettings => currentSettings;

    public void SetLevel(int newLevel)
    {
        currentLevel = newLevel;
    }

    public float GetNormalizedLevel()
    {
        return Mathf.Clamp01(currentLevel / maxLevel);
    }

    public void NextLevel()
    {
        SetLevel(currentLevel + 1);
    }
}
