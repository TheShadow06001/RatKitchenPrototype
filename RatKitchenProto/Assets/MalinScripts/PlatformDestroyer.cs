using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject platformDestructionPoint;
    //reference to obstacles

    private void Start()
    {
        platformDestructionPoint = GameObject.Find("DestructionPoint");
    }

    private void Update()
    {
        if (transform.position.z < platformDestructionPoint.transform.position.z)
        {
            gameObject.SetActive(false);
            //obstacles SetActive(false) with platforms
        }
    }
}
