using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject platformDestructionPoint;
    //reference to obstacles

    private void Start()
    {
        platformDestructionPoint = GameObject.Find("PlatformDestructionPoint");
    }

    private void Update()
    {
        if (transform.position.x > platformDestructionPoint.transform.position.x)
        {
            gameObject.SetActive(false);
            //obstacles SetActive(false) with platforms
        }
    }
}
