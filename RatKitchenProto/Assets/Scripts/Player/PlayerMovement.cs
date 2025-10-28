using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] GameObject anchorPoint;
    float moveSpeed;
    float cameraSpeed;
    void Start()
    {
        cameraSpeed = camera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;
    }

    void Update()
    {
        HandleForwardSpeed();
        
        Vector3 currentPos = transform.position;

        if (Input.GetKey(KeyCode.Space) == false)
        {
            currentPos.z += moveSpeed * Time.deltaTime;
        }
        transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);
    }
    void HandleForwardSpeed()
    {
        if (gameObject.transform.position.z < anchorPoint.transform.position.z)
        {
            if (moveSpeed < cameraSpeed * 2)
            {
                moveSpeed *= 2f;
            }
        }
        else
        {
            moveSpeed = cameraSpeed;
        }
    }
}
