using UnityEngine;

[CreateAssetMenu(fileName = "PlatformType", menuName = "Scriptable Objects/Platform Type")]
public class PlatformType : ScriptableObject
{
    //public string ElementName;
    //public int MaxCount;
    //public GameObject Prefab;

    public string typeOfPlatform;
    public string tag;
    public GameObject prefab;

    [Header("Spawn Settings")]
    public int MaxCountPerRun = 2;
    public float spawnWeight = 1f;

    [Header("Variants, if applicable")]
    public GameObject[] variantPrefabs;

    [Header("Difficulty Scaling")]
    public int minLevelToAppear = 1;
    public int maxLevelToAppear = 999;

    public bool CanSpawnAtLevel(int level)
    {
        return level >= minLevelToAppear && level <= maxLevelToAppear;
    }

    public GameObject GetRandomPrefab()
    {
        if (variantPrefabs != null && variantPrefabs.Length > 0 && Random.value < 0.5f)
            return variantPrefabs[Random.Range(0, variantPrefabs.Length)];
        return prefab;
    }
}
