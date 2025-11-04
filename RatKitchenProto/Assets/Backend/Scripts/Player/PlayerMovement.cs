using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject anchorPoint;
    [SerializeField] float forwardAcceleration = 1.05f;
    [SerializeField] float maxSpeedMultiplier = 1.05f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] PlayerChangeLane laneChanger;

    [Header("Keybinds")]
    public KeyCode _PlayerJump;

    float moveSpeed;
    float cameraSpeed;
    Rigidbody rigidBody;

    [Header("Camera Script")]
    CameraScript cameraScript;
    Vector3 cameraMoveDirection;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        cameraSpeed = mainCamera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;

        cameraScript = mainCamera.GetComponent<CameraScript>();
        cameraSpeed = cameraScript.moveSpeed;
        cameraMoveDirection = cameraScript.moveDirection;
    }

    void Update()
    {

        HandleForwardSpeed();
        Jump();
        Vector3 currentPos = transform.position;

        float totalSpeed = moveSpeed;

        transform.position += cameraMoveDirection * totalSpeed * Time.deltaTime;



    }
    void HandleForwardSpeed()
    {
        Vector3 behindDirection = -cameraMoveDirection;

        Vector3 vecToPlayer = transform.position - anchorPoint.transform.position;

        float distanceBehind = Vector3.Dot(vecToPlayer, behindDirection);

        if (distanceBehind > 0.1f)
        {
            float targetSpeed1 = cameraSpeed * maxSpeedMultiplier;
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed1, Time.deltaTime * forwardAcceleration);
        }
        else
        {
            moveSpeed = cameraSpeed;
        }
    }

    void Jump()
    {

        if (rigidBody != null && Input.GetKeyDown(_PlayerJump) && transform.position.y < 1.1 && !laneChanger.isChangingLanes)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
