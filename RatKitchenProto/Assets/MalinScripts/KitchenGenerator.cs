using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    [SerializeField] private List<PlatformType> platformTypes = new(); // scriptable objects
    [SerializeField] private List<WallType> wallTypes = new();
    Vector3 wallOffset;

    [SerializeField] private Transform generationPoint;
    [SerializeField] private float distanceBetween = 0.01f;
    [SerializeField] private int maxPlatformsPerRun = 20;
    [SerializeField] private int currentLevel = 1;

    [SerializeField] private GameObject endPlatformPrefab;
    [SerializeField] private GameObject endWallPrefab;

    private Dictionary<string, int> platformSpawnCounts = new();
    [SerializeField] private int spawnedPlatforms = 0;
    private bool isLevelComplete = false;
    private string lastTag = "";

    private Dictionary<string, int> wallSpawnCounts = new();


    private void Start()
    {
        foreach (var type in platformTypes)
        {
            platformSpawnCounts[type.tag] = 0; 
        }

        foreach (var type in wallTypes)
        {
            wallSpawnCounts[type.tag] = 0;
        }
    }

    // When game manager is used, rename to UpdateKitchenGenerator and call on ResetKitchenGenerator
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
            PlatformType chosenType = PlatformTypeToSpawn();
            if (chosenType == null)
                return;

            WallType chosenWall = WallTypeToSpawn();
            if (chosenWall == null)
                return;

            //GameObject prefabToSpawn = chosenType.GetRandomPrefab(); // för varianter, återkom om aktuellt
            Vector3 spawnPos = new Vector3(transform.position.x + distanceBetween, transform.position.y, transform.position.z);
            Vector3 wallposition = new Vector3(0, transform.position.y + 2f, transform.position.z + 3f);
            wallOffset = new Vector3(0, chosenType.prefab.transform.localScale.y / 2, (chosenType.prefab.transform.localScale.z / 2) + (chosenWall.prefab.transform.localScale.z / 2));

            GameObject newPlatform = KitchenPool.Instance.GetPooledObject(chosenType, spawnPos, Quaternion.identity);
            newPlatform.SetActive(true);

            GameObject newWall = KitchenPool.Instance.GetPooledWall(chosenWall, spawnPos + wallOffset, Quaternion.identity);
            newWall.SetActive(true);

            transform.position = spawnPos; // uppdate platform generator position
            spawnedPlatforms++;

            platformSpawnCounts[chosenType.tag]++;
            lastTag = chosenType.tag;
        }
    }

    private PlatformType PlatformTypeToSpawn()
    {
        List<PlatformType> validType = new();

        foreach (var type in platformTypes)
        {
            if (platformSpawnCounts[type.tag] >= type.MaxCountPerRun) //skippar
                continue;

            if (!type.CanSpawnAtLevel(currentLevel)) //behövs kanske inte?? Eller beror väl på hur man lägger upp prefabs och levlar
                continue;

            if (IsInvalidNeighbour(type.tag)) // stove och sink får inte vara grannar
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return platformTypes.Find(p => p.isBaseCase == true); // standard-valet med Counter

        float totalSpawnWeight = 0f;
        foreach (var type in validType)
        {
            totalSpawnWeight += type.spawnWeight;
        }

        float pickRandomPlatform = Random.value * totalSpawnWeight;
        float cumulative = 0;

        foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (pickRandomPlatform <= cumulative)
                return type;
        }

        return validType[0];
    }

    private WallType WallTypeToSpawn()
    {
        List<WallType> validType = new();

        foreach(var type in wallTypes)
        {
            if (wallSpawnCounts[type.tag] >= type.MaxCountPerRun) //skippar
                continue;

            if (!type.CanSpawnAtLevel(currentLevel))
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return wallTypes.Find(p => p.isBaseCase == true); // standard-valet 
        
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

    private bool IsInvalidNeighbour(string nextTag)
    {
        if ((lastTag == "Sink" && nextTag == "Stove") || (lastTag == "Stove" && nextTag == "Sink")) // kan detta specas i sctiptable object istället? För att kunna byggas på sen utan att röra koden?
            return true;

        return false;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ResetKitchenGenerator();
            //Fade? call other function for respawn
        }
    }

    public void ResetKitchenGenerator()
    {
        spawnedPlatforms = 0;
        isLevelComplete = false;

        // call UpdateKitchenGenerator
    }
}
