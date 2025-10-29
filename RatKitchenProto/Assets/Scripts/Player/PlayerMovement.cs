using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject camera;
    [SerializeField] GameObject anchorPoint;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private PlayerChangeLane playerChangeLane;

    [Header("Jump Settings")]
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private float jumpDuration = 0.5f;

    private bool switching;
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
            else
            {
                moveSpeed = cameraSpeed;
            }
        }
    }
    void Jump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundLayer))
        {
            bool isSwitching = playerChangeLane.GetLane();

            if (Input.GetKeyDown(KeyCode.F) && !isSwitching)
            {

                StartCoroutine(Jumping());

            }

            isSwitching = false;
        }


    }

    IEnumerator Jumping()
    {
        rigidbody.useGravity = false;
        Vector3 playerPos = transform.position;
        float targetHeight = playerPos.y + jumpHeight;
        for (float i = 0; i < 1; i += Time.deltaTime * jumpDuration)
        {
            playerPos.y = Mathf.Lerp(playerPos.y, targetHeight, i);
            transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
            yield return null;
        }



        rigidbody.useGravity = true;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * rayLength);
    }
}

