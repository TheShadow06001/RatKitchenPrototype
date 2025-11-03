using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    [SerializeField] private List<PlatformType> platformTypes = new();
    [SerializeField] private List<WallType> wallTypes = new();
    Vector3 wallOffset;

    [SerializeField] private Transform generationPoint;
    [SerializeField] private float distanceBetween = 0f;
    [SerializeField] private int maxPlatformsPerRun = 20;
    [SerializeField] private int currentLevel = 1;

    [SerializeField] private GameObject endPlatformPrefab;
    [SerializeField] private GameObject endWallPrefab;

    private Dictionary<PlatformType, int> platformSpawnCounts = new();
    private Dictionary<WallType, int> wallSpawnCounts = new();

    [SerializeField] private int spawnedPlatforms = 0;
    private bool isLevelComplete = false;

    private PlatformType lastPlatformType;
    private PlatformType secondLastPlatformType;

    private WallType lastWallType;
    private WallType secondLastWallType;


    private void Start()
    {
        foreach (var type in platformTypes)
        {
            platformSpawnCounts[type] = 0;
        }

        foreach (var type in wallTypes)
        {
            wallSpawnCounts[type] = 0;
        }
    }

    // When game manager is used, rename to UpdateKitchenGenerator and call on ResetKitchenGenerator?
    private void Update()
    {
        if (isLevelComplete)
            return;

        if (spawnedPlatforms >= maxPlatformsPerRun)
        {
            SpawnEndPlatform();
            isLevelComplete = true;
            return;
        }

        if (transform.position.x < generationPoint.position.x)
        {
            //1 vägg + 5 plattformar ska genereras när transform.position.x < generationPoint.position.x
            
            PlatformType chosenPlatform = PlatformTypeToSpawn();
            if (chosenPlatform == null)
                return;

            WallType chosenWall = WallTypeToSpawn();
            if (chosenWall == null)
                return;

            GameObject prefabToSpawn = chosenPlatform.GetRandomPrefab(); // currently only counters
            
            Vector3 spawnPos = new Vector3(transform.position.x + distanceBetween, transform.position.y, transform.position.z);
            //Vector3 wallposition = new Vector3(0, transform.position.y + 2f, transform.position.z + 3f); // magic numbers, justera denna (används den ens??)
            wallOffset = new Vector3(0, chosenPlatform.prefab.transform.localScale.y / 2, (chosenPlatform.prefab.transform.localScale.z / 2) + (chosenWall.prefab.transform.localScale.z / 2));

            GameObject newPlatform = KitchenPool.Instance.GetPooledObject(chosenPlatform, spawnPos, Quaternion.identity);
            newPlatform.SetActive(true);

            GameObject newWall = KitchenPool.Instance.GetPooledWall(chosenWall, spawnPos + wallOffset, Quaternion.identity);
            newWall.SetActive(true);

            platformSpawnCounts[chosenPlatform]++;
            wallSpawnCounts[chosenWall]++;

            secondLastPlatformType = lastPlatformType;
            lastPlatformType = chosenPlatform;

            secondLastWallType = lastWallType;
            lastWallType = chosenWall;

            transform.position = spawnPos; // uppdate platform generator position
            spawnedPlatforms++;
        }
    }

    private PlatformType PlatformTypeToSpawn()
    {
        List<PlatformType> validType = new();

        foreach (var type in platformTypes)
        {
            if (!type.CanSpawnAtLevel(currentLevel)) 
                continue;

            if (platformSpawnCounts[type] >= GetScaledMaxCount(type))
                continue;

            if (IsInvalidPlatformNeighbour(type))
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return platformTypes.Find(p => p.isBaseCase) ?? platformTypes[0];

        //weighted random algorithm
        float totalSpawnWeight = 0f;
        foreach (var type in validType)
        {
            totalSpawnWeight += type.spawnWeight;
        }

        float randomPick = Random.value * totalSpawnWeight;
        float cumulative = 0;

        foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (randomPick <= cumulative)
                return type;
        }

        return validType[0];
    }

    private WallType WallTypeToSpawn()
    {
        List<WallType> validType = new();

        foreach (var type in wallTypes)
        {
            if (!type.CanSpawnAtLevel(currentLevel))
                continue;

            if (wallSpawnCounts[type] >= GetScaledMaxCount(type))
                continue;

            if (IsInvalidWallNeighbour(type))
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return wallTypes.Find(w => w.isBaseCase) ?? wallTypes[0]; // standard-pick

        //weighted random algorithm
        float totalSpawnWeight = 0f;
        foreach (var type in validType)
        {
            totalSpawnWeight += type.spawnWeight;
        }

        float pickRandomWall = Random.value * totalSpawnWeight;
        float cumulative = 0;

        foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (pickRandomWall <= cumulative)
                return type;
        }

        return validType[0];
    }

    private bool IsInvalidPlatformNeighbour(PlatformType next)
    {
        if (lastPlatformType != null && next.cannotHaveNeighbour.Contains(lastPlatformType.tag))
            return true;

        if (next.mustHaveCounterBetween && (lastPlatformType?.tag == next.tag || secondLastPlatformType?.tag == next.tag))
            return true;

        return false;
    }

    private bool IsInvalidWallNeighbour(WallType next)
    {
        if (lastWallType != null && next.cannotHaveNeighbour.Contains(lastWallType.tag))
            return true;

        return false;
    }

    private int GetScaledMaxCount(PlatformType type)
    {
        int baseCount = type.baseMaxCount;
        float scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);
    }

    private int GetScaledMaxCount(WallType type)
    {
        int baseCount = type.baseMaxCount;
        float scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);    
    }

    private void SpawnEndPlatform()
    {
        if (!endPlatformPrefab)
            return;

        Vector3 platformSpawnPos = transform.position + new Vector3(distanceBetween, 0, 0);
        Instantiate(endPlatformPrefab, platformSpawnPos, Quaternion.identity);

        if(endWallPrefab)
        {
            Vector3 wallSpawnPos = platformSpawnPos + wallOffset;
            Instantiate(endWallPrefab, wallSpawnPos, Quaternion.identity);
        }
    }

    /* FOR RATHOLE */
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //GameManager.Instance.NextLevel();
            //Fade? call other function for respawn
        }
    }

    public void SetDifficulty(LevelSettings settings)
    {
        maxPlatformsPerRun = settings.maxPlatforms;
    }

    /* NOT CALLED ANYWHERE YET */
    public void ResetKitchenGenerator()
    {
        spawnedPlatforms = 0;
        isLevelComplete = false;

        foreach (var key in platformSpawnCounts.Keys)
            platformSpawnCounts[key] = 0;

        foreach (var key in wallSpawnCounts.Keys)
            wallSpawnCounts[key] = 0;

        lastPlatformType = null;
        secondLastPlatformType = null;

        // call UpdateKitchenGenerator
    }
}
