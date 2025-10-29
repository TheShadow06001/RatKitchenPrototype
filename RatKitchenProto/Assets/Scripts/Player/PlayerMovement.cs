using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject anchorPoint;
    float moveSpeed;
    float cameraSpeed;
    [SerializeField] float forwardAcceleration = 1f;
    [SerializeField] float maxSpeedMultiplier = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float dashForce = 20f;
    [SerializeField] PlayerChangeLane laneChanger;

    private Rigidbody rb;

    void Start()
    {
        cameraSpeed = mainCamera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;

        rb = GetComponent<Rigidbody>();
        laneChanger = laneChanger.GetComponent<PlayerChangeLane>();
    }

    void Update()
    {
        HandleForwardSpeed();
        Jump();
        HandleDash();
        Vector3 currentPos = transform.position;

        if (!Input.GetKey(KeyCode.Space))
        {
            currentPos.z += moveSpeed * Time.deltaTime;
            transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);
        }
    }
    void HandleForwardSpeed()
    {
        if (transform.position.z < anchorPoint.transform.position.z)
        {
            float targetSpeed = cameraSpeed * maxSpeedMultiplier;
            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, Time.deltaTime * forwardAcceleration);
        }
        else
        {
            moveSpeed = cameraSpeed;
        }
    }
    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !laneChanger.isChangingLanes)
        {
            rb.AddForce(Vector3.forward * dashForce, ForceMode.Impulse);
        }
    }
    void Jump()
    {
        if (rb != null && Input.GetKeyDown(KeyCode.F) && transform.position.y < 1.05 && !laneChanger.isChangingLanes)
        {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
