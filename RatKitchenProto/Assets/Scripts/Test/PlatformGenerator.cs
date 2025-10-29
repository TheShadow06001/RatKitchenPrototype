using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject thePlatform; //the platforms to be generated ahead of the player
    public Transform generationPoint; // the point ahead of player, attached to camera
    public float distanceBetween;

    private float platformWidth;

    private int platformSelector;
    //public GameObject[] thePlatforms; //the different platforms to be generated, in an array
    public PlatformPooler[] thePlatformPools;

    private float[] platformWidths;

    private void Start()
    {
        //platformWidth = thePlatform.GetComponent<BoxCollider>().size.x; // width of the platform

        platformWidths = new float[thePlatformPools.Length];

        for (int i = 0; i < thePlatformPools.Length; i++)
        {
            platformWidths[i] = thePlatformPools[i].pooledObject.transform.localScale.x;
        }
    }

    private void Update()
    {
        if (transform.position.x < generationPoint.position.x) // if player position is more to the left than the generation point, generate new platform
        {
            platformSelector = Random.Range(0, thePlatformPools.Length);
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, transform.position.y, transform.position.z);

            //Instantiate(/*thePlatform*/thePlatforms[platformSelector], transform.position, transform.rotation);

            GameObject newPlatform = thePlatformPools[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);
        }
    }

}
