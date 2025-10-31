using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += -transform.right * moveSpeed * Time.deltaTime;
    }
}
