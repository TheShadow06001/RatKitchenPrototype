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
    [SerializeField] private int basePackagesPerRun = 3; // 1 wall fits 5 counters = 1 package

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

    public GameObject cameraMover;

    private void Start()
    {
        //var settings = DifficultyManager.Instance.CurrentSettings;
        //if (settings != null)
        //{
        //    currentLevel = settings.levelNumber;
        //    maxPlatformsPerRun = settings.maxPlatforms;
        //}   
        
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

        if (transform.position.x > generationPoint.position.x)
        {
            //1 vägg + 5 plattformar ska genereras när transform.position.x > generationPoint.position.x
            
            /*PlatformType chosenPlatform = PlatformTypeToSpawn();
            if (chosenPlatform == null)
                return;*/

            WallType chosenWall = WallTypeToSpawn();
            if (chosenWall == null)
                return;

            Vector3 wallSpawnPosition = transform.position;
            Quaternion wallRotation = Quaternion.Euler(0f, 90f, 0f);
            GameObject newWall = KitchenPool.Instance.GetPooledWall(chosenWall, wallSpawnPosition /*+ wallOffset*/, /*Quaternion.identity*/ wallRotation);
            newWall.SetActive(true);
            wallSpawnCounts[chosenWall]++;

            /* NYTT */
            //float platformSpacing = chosenPlatform.prefab.transform.localScale.x - distanceBetween;
            float platformSpacing = platformTypes[0].prefab.transform.localScale.x - distanceBetween;
            for (int i = 0; i < chosenWall.platformsPerWall; i++)
            {
                PlatformType chosenPlatform = PlatformTypeToSpawn();
                if (chosenPlatform == null)
                    return; 

                Vector3 platformPos = wallSpawnPosition + new Vector3( (i * platformSpacing) - (chosenPlatform.prefab.transform.localScale.z / 2 ), 0, 0.62f); // DENNA BEHÖVER SES ÖVER FÖR ATT SPAWNA RÄTT
                GameObject newPlatform = KitchenPool.Instance.GetPooledObject(chosenPlatform, platformPos, Quaternion.identity);
                newPlatform.SetActive(true);
           
                platformSpawnCounts[chosenPlatform]++;
                secondLastPlatformType = lastPlatformType;
                lastPlatformType = chosenPlatform;

                spawnedPlatforms++;
            }
            /* SLUT NYTT */

            //GameObject prefabToSpawn = chosenPlatform.GetRandomPrefab(); // currently only counters
            
            Vector3 spawnPos = new Vector3(transform.position.x - distanceBetween, transform.position.y, transform.position.z);
            //Vector3 wallposition = new Vector3(0, transform.position.y + 2f, transform.position.z + 3f); // magic numbers, justera denna (används den ens??)
            //wallOffset = new Vector3(0, chosenPlatform.prefab.transform.localScale.y / 2, (chosenPlatform.prefab.transform.localScale.z / 2) + (chosenWall.prefab.transform.localScale.z / 2));

            //GameObject newPlatform = KitchenPool.Instance.GetPooledObject(chosenPlatform, spawnPos, Quaternion.identity);
            //newPlatform.SetActive(true);



            //platformSpawnCounts[chosenPlatform]++;
            //wallSpawnCounts[chosenWall]++;

            //secondLastPlatformType = lastPlatformType;
            //lastPlatformType = chosenPlatform;

            secondLastWallType = lastWallType;
            lastWallType = chosenWall;

            //transform.position = spawnPos; // uppdate platform generator position
            //spawnedPlatforms++;

            transform.position = wallSpawnPosition + new Vector3(chosenWall.platformsPerWall * platformSpacing, 0, 0);
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
        float normalizedLevel = DifficultyManager.Instance.GetNormalizedLevel();
        float totalSpawnWeight = 0f;
        Dictionary<PlatformType,float> weightedChances = new();

        foreach (var type in validType)
        {
            float curveMultiplier = 1f;
            if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            {
                curveMultiplier = Mathf.Clamp01(type.spawnChanceCurve.Evaluate(normalizedLevel));
            }

            float weightedChance = type.spawnWeight * curveMultiplier;
            weightedChances[type] = weightedChance;
            totalSpawnWeight += weightedChance;

            //totalSpawnWeight += type.spawnWeight;
        }

        float randomPick = Random.value * totalSpawnWeight;
        float cumulative = 0;

        /*foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (randomPick <= cumulative)
                return type;
        }*/

        foreach (var pair in weightedChances)
        {
            cumulative += pair.Value;
            if (randomPick <= cumulative)
                return pair.Key;
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
        float normalizedLevel = DifficultyManager.Instance.GetNormalizedLevel();
        float totalSpawnWeight = 0f;
        Dictionary<WallType, float> weightedChances = new();

        foreach (var type in validType)
        {
            float curveMultiplier = 1f;
            if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            {
                curveMultiplier = Mathf.Clamp01(type.spawnChanceCurve.Evaluate(normalizedLevel));
            }

            float weightedChance = type.spawnWeight * curveMultiplier;
            weightedChances[type] = weightedChance;
            totalSpawnWeight += weightedChance;

            //totalSpawnWeight += type.spawnWeight;
        }

        float pickRandomWall = Random.value * totalSpawnWeight;
        float cumulative = 0;

       /* foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (pickRandomWall <= cumulative)
                return type;
        }*/
        foreach (var pair in weightedChances)
        {
            cumulative += pair.Value;
            if (pickRandomWall <= cumulative)
                return pair.Key;
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

    // spawns complete end-prefab - change to only end-wall?
    private void SpawnEndPlatform()
    {
        if (!endPlatformPrefab)
            return;

        Vector3 platformSpawnPos = transform.position + new Vector3(0.298f, 0.539f, 0.325f); // magic numbers - to change
        Instantiate(endPlatformPrefab, platformSpawnPos, Quaternion.identity);

       /* if(endWallPrefab)
        {
            Vector3 wallSpawnPos = platformSpawnPos + wallOffset;
            Instantiate(endWallPrefab, wallSpawnPos, Quaternion.identity);
        }*/
    }

    /* FOR RATHOLE */
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //GameManager.Instance.NextLevel();
            //ResetKitchenGenerator();
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
