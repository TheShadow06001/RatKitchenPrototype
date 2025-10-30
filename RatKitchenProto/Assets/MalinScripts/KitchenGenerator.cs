using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    [SerializeField] private List<PlatformType> platformTypes = new(); // scriptable objects

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

    private void Start()
    {
        foreach (var type in platformTypes)
        {
            platformSpawnCounts[type.tag] = 0; 
        }
    }

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

            //GameObject prefabToSpawn = chosenType.GetRandomPrefab();
            Vector3 spawnPos = new Vector3(transform.position.x + distanceBetween, transform.position.y, transform.position.z);

            GameObject newPlatform = KitchenPool.Instance.GetPooledObject(chosenType, spawnPos, Quaternion.identity);
            newPlatform.SetActive(true);

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
            return platformTypes.Find(p => p.tag == "Counter"); // standard-valet med Counter

        // för slumpmässigt val
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

    private bool IsInvalidNeighbour(string nextTag)
    {
        if ((lastTag == "Sink" && nextTag == "Stove") || (lastTag == "Stove" && nextTag == "Sink"))
            return true;

        return false;
    }

    private void SpawnEndPlatform()
    {
        if (!endPlatformPrefab)
            return;

        Vector3 platformSpawnPos = transform.position + new Vector3(5f, 0, 0);
        Instantiate(endPlatformPrefab, platformSpawnPos, Quaternion.identity);

        if(endWallPrefab)
        {
            Vector3 wallSpawnPos = platformSpawnPos + new Vector3(0, 2f, 3f);
            Instantiate(endWallPrefab, wallSpawnPos, Quaternion.identity);
        }
    }
}
