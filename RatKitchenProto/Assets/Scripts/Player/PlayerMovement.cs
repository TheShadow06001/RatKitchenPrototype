using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject anchorPoint;
    float moveSpeed;
    float cameraSpeed;
    [SerializeField] float jumpForce = 5f;

    PlayerChangeLane laneChanger;
    void Start()
    {
        cameraSpeed = mainCamera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;
    }

    void Update()
    {
        HandleForwardSpeed();
        Jump();
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
    void Jump()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && Input.GetKeyDown(KeyCode.F) && transform.position.y < 1.05)
        {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
