using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject anchorPoint;

    [SerializeField] float forwardAcceleration = 1f;
    [SerializeField] float maxSpeedMultiplier = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] PlayerChangeLane laneChanger;

    float moveSpeed;
    float cameraSpeed;

    void Start()
    {
        cameraSpeed = mainCamera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;

        laneChanger = laneChanger.GetComponent<PlayerChangeLane>();
    }

    void Update()
    {
        HandleForwardSpeed();
        Jump();
        Vector3 currentPos = transform.position;

        float totalSpeed = moveSpeed;

        currentPos.z += totalSpeed * Time.deltaTime;
        transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);
    }
    void HandleForwardSpeed()
    {
        if (transform.position.z < anchorPoint.transform.position.z - 0.1f)
        {
            float targetSpeed1 = cameraSpeed * maxSpeedMultiplier;
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed1, Time.deltaTime * forwardAcceleration);
        }
        else { moveSpeed = cameraSpeed; }
    }
    void Jump()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && Input.GetKeyDown(KeyCode.F) && transform.position.y < 1.05 && !laneChanger.isChangingLanes)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
