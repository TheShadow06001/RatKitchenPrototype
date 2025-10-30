using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject thePlatform;      // the platforms to be generated ahead of the player
                                        // => set 2-3 platforms in scene to always start with?
                                        // for example always have 2-3 counters present at the beginning of the rat game to let the generator build from
    public Transform generationPoint;   // the point ahead of player, attached to camera
    public float distanceBetween;

    private int platformSelector;
    public PlatformPooler[] thePlatformPools;

    private float[] platformWidths;

    public int maxAmountOfPlatforms = 20;
    public int spawnedPlatforms = 0;

    private bool isLevelComplete = false;
    private bool isEndPlatformSpawned = false;
    public GameObject endPlatformPrefab;

    private void Start()
    {
        platformWidths = new float[thePlatformPools.Length];

        for (int i = 0; i < thePlatformPools.Length; i++)
        {
            platformWidths[i] = thePlatformPools[i].pooledObject.transform.localScale.x;
        }
    }

    private void Update()
    {
        // if spawned platforms are greater than or equal to max platforms
        // => if levelComplete is not true => change to true
        // => return

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

        if (transform.position.x < generationPoint.position.x) // if player position is more to the left than the generation point, generate new platform
        {
            platformSelector = Random.Range(0, thePlatformPools.Length);
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, transform.position.y, transform.position.z);

            GameObject newPlatform = thePlatformPools[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);
            spawnedPlatforms++;        
        }
    }

    public void SpawnEndPlatform()
    {
        Vector3 spawnPos = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);

        Instantiate(endPlatformPrefab, spawnPos, Quaternion.identity);
    }
    public void ResetPlatformGenerator()
    {
        spawnedPlatforms = 0;
        isLevelComplete = false;
        isEndPlatformSpawned = false;
    }
}
