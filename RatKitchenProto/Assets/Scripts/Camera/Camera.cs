using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Tooltip("Represents Player and Camera movement speed")] 
    public float moveSpeed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
