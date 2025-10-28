using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] GameObject anchor;
    private float moveSpeed;
    private float baseSpeed;

    private int lane = 1;

    float leftLaneX = -0.5f;
    float middleLaneX = 0f;
    float rightLaneX = 0.5f;

    Vector3 moveDirection;
    
    void Start()
    {
        baseSpeed = Camera.GetComponent<CameraScript>().moveSpeed;

        moveSpeed = baseSpeed;

    }

    void Update()
    {
        PlayerMoveForward();
        PlayerChangeLine();
    }
    void PlayerMoveForward()
    {
        if (gameObject.transform.position.z < anchor.transform.position.z)
        {
            if (moveSpeed < baseSpeed * 2)
            {
                moveSpeed *= 2;
            }
        }
        else
        {
            moveSpeed = baseSpeed;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        else 
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

            
    }
    void PlayerChangeLine()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (lane < 2)
            {
                lane++;
                SwapLane(lane);
            }

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (lane > 0)
            {
                lane--;
                SwapLane(lane);
            }

        }
        
    }

    private void SwapLane(int value)
    {
        Vector3 currentPos = transform.position;
        if (value == 1)
        {
            currentPos.x = middleLaneX;
        }
        else if (value == 2)
        {
            currentPos.x = rightLaneX;
        }
        else
        {
            currentPos.x = leftLaneX;
        }
        transform.position = currentPos;
    }


}

