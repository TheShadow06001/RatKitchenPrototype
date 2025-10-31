using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject anchorPoint;
    [SerializeField] float forwardAcceleration = 1f;
    [SerializeField] float backAcceleration = 3f;
    [SerializeField] float minSpeedMultiplier = 0.5f;
    [SerializeField] float maxSpeedMultiplier = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] PlayerChangeLane laneChanger;

    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashFadeTime = 0.5f;
    private float currentDashSpeed = 0f;

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
        HandleDash();
        Vector3 currentPos = transform.position;
        if (currentDashSpeed > 0.1)
        {
            currentDashSpeed = Mathf.Lerp(currentDashSpeed, 0.01f, Time.deltaTime / dashFadeTime);
        }
        if (!Input.GetKey(KeyCode.Space))
        {
            float totalSpeed = moveSpeed + currentDashSpeed;

            currentPos.z += totalSpeed * Time.deltaTime;
            transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);
        }
    }
    void HandleForwardSpeed()
    {
        if (transform.position.z < anchorPoint.transform.position.z - 0.1f)
        {
            float targetSpeed1 = cameraSpeed * maxSpeedMultiplier;
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed1, Time.deltaTime * forwardAcceleration);
        }
        else if (transform.position.z > anchorPoint.transform.position.z + 0.1f)
        {
            float targetSpeed2 = cameraSpeed * minSpeedMultiplier;
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed2, Time.deltaTime * backAcceleration);
        }
        else
        {
            moveSpeed = cameraSpeed;
        }
    }
    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !laneChanger.isChangingLanes && currentDashSpeed <= 0.1f)
        {
            currentDashSpeed = dashSpeed;
        }
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
