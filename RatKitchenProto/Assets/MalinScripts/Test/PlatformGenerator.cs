using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlatformGenerator : MonoBehaviour
{
    //public GameObject thePlatform;      // the platforms to be generated ahead of the player
    //public GameObject theWall;          // => set 2-3 platforms in scene to always start with?
                                        // for example always have 2-3 counters present at the beginning of the rat game to let the generation point build from
    public Transform generationPoint;   // the point ahead of player, attached to camera
    public float distanceBetween;

    private int platformSelector;
    private int wallSelector;
    public PlatformPooler[] thePlatformPools;
    public WallPooler[] theWallPools;

    public ObstaclePooler[] theObstaclePools;   // test
    private int obstacleSelector;               // test
    public Transform[] obstacleSpawnPoints;

    private float[] platformWidths;
    private float[] wallWidths;
    private float wallOffset;

    [SerializeField] private int maxAmountOfPlatforms = 20;
    [SerializeField] private int spawnedPlatforms = 0;

    private bool isLevelComplete = false;
    private bool isEndPlatformSpawned = false;
    public GameObject endPlatformPrefab;
    public GameObject endWallPrefab;

    private string lastPlatformTag = "";
    private string secondLastPlatformTag = "";

    [SerializeField] private int maxSinks = 2;
    [SerializeField] private int maxStoves = 2;
    private int sinkCount = 0;
    private int stoveCount = 0;

    private void Start()
    {
        platformWidths = new float[thePlatformPools.Length];
        wallWidths = new float[theWallPools.Length];
        wallOffset = endPlatformPrefab.transform.localScale.z / 2f;

        for (int i = 0; i < thePlatformPools.Length; i++)
        {
            platformWidths[i] = thePlatformPools[i].pooledObject.transform.localScale.x;
        }

        for (int i = 0; i < theWallPools.Length; i++)
        {
            wallWidths[i] = theWallPools[i].pooledWallObject.transform.localScale.x;
        }
    }

    private void Update()
    {
        if (spawnedPlatforms >= maxAmountOfPlatforms)
        {
            if (!isEndPlatformSpawned)
            {
                SpawnEndPlatform(); // rat hole section
                isEndPlatformSpawned = true;
            }

            else if (!isLevelComplete)
            {
                isLevelComplete = true;
                Debug.LogWarning("end of level reached, no more platforms will be generated");
                // call function to teleport back to start or other next-level-logic
            }
            return;
        }

        if (transform.position.x < generationPoint.position.x) // generate/set active new platform
        {
            //spawning constraints
            GameObject spawnCandidate = null;
            string spawnCandidateTag = "";
            bool isValidToSpawn = false;

            int safetyCounter = 0;
            while (!isValidToSpawn && safetyCounter < 50)
            {
                platformSelector = Random.Range(0, thePlatformPools.Length);
                spawnCandidate = thePlatformPools[platformSelector].pooledObject;
                spawnCandidateTag = spawnCandidate.tag;

                isValidToSpawn = IsValidNextPlatform(spawnCandidateTag);
                safetyCounter++;
            }

            //moves the PlatformGenerator point
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, transform.position.y, transform.position.z);
            Vector3 wallposition = new Vector3(0, transform.position.y + 2f, transform.position.z + 3f);    // to be adjusted

            // spawn platforms
            GameObject newPlatform = thePlatformPools[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            if (newPlatform.CompareTag("Sink"))
                sinkCount++;

            if (newPlatform.CompareTag("Stove"))
                stoveCount++;


            secondLastPlatformTag = lastPlatformTag;
            lastPlatformTag = newPlatform.tag;

            wallSelector = Random.Range(0, theWallPools.Length);
            obstacleSelector = Random.Range(0, theObstaclePools.Length); // test

            GameObject newObstacle = theObstaclePools[obstacleSelector].GetPooledObstacle();
            //fixa spawn points här med referens till active prefab?
            //Transform lanePositions = newPlatform.transform.Find("SpawnPoints");
         
            // wall-spawner
            GameObject newWall = theWallPools[wallSelector].GetPooledWallObject();
            newWall.transform.position = transform.position + wallposition;
            newWall.transform.rotation = transform.rotation;
            newWall.SetActive(true);

            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);
            spawnedPlatforms++;   
            
            // behöver återkomma till spawn pos för scriptable objects
        }
    }

    private bool IsValidNextPlatform(string nextTag)
    {
        if (IsSinkOrStove(lastPlatformTag) && IsSinkOrStove(nextTag))
            return false;

        if (nextTag == "Sink" && sinkCount >= maxSinks)
            return false;

        if (nextTag == "Stove" && stoveCount >= maxStoves)
            return false;

        return true;
    }

    private bool IsSinkOrStove(string tag)
    {
        return tag == "Sink" || tag == "Stove";
    }

    public void SpawnEndPlatform()
    {
        //float wallOffset = endPlatformPrefab.transform.localScale.z / 2f;

        Vector3 platformSpawnPos = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);                  // to be adjusted
        Vector3 wallSpawnPos = new Vector3(transform.position.x + 5f, transform.position.y + 2f, transform.position.z + wallOffset);    // to be adjusted

        Instantiate(endPlatformPrefab, platformSpawnPos, Quaternion.identity);
        Instantiate(endWallPrefab, wallSpawnPos, Quaternion.identity);  
    }
    public void ResetPlatformGenerator()
    {
        spawnedPlatforms = 0;
        isLevelComplete = false;
        isEndPlatformSpawned = false;

        sinkCount = 0;
        stoveCount = 0;
    }
}
