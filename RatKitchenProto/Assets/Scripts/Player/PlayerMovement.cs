using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] GameObject anchor;
    private float moveSpeed;
    private float baseSpeed;

    private int lane = 1;

    Vector3 left;
    Vector3 right;
    Vector3 middle;

    Vector3 moveDirection;
    
    void Start()
    {
        baseSpeed = Camera.GetComponent<CameraScript>().moveSpeed;

        moveSpeed = baseSpeed;

        left = new Vector3(-0.5f, transform.position.y, transform.position.z);
        right = new Vector3(0.5f, transform.position.y, transform.position.z);
        middle = new Vector3(0f, transform.position.y, transform.position.z);

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
        if (value == 1)
        {
            transform.position = middle;
        }
        else if (value == 2)
        {
            transform.position = right;
        }
        else
        {
            transform.position = left;
        }
    }


}

