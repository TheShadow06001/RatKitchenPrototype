using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public Vector3 moveDirection = Vector3.left;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }
}
