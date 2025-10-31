using UnityEngine;

public class PlayerChangeLane : MonoBehaviour
{
    private int lane = 1;

    public float leftLaneX = -0.5f;
    public float middleLaneX = 0f;
    public float rightLaneX = 0.5f;
    private float targetHorizontalZ;
    [SerializeField] private float laneChangeSpeed = 10f;

    public bool isChangingLanes;
    [SerializeField] private float laneChangeThreshold = 0.1f;

    [Header("KeyBinds")]
    public KeyCode _PlayerLeft;
    public KeyCode _PlayerRight;


    void Start()
    {
        targetHorizontalZ = middleLaneX;
        isChangingLanes = false;
    }

    void Update()
    {
        PlayerChangeLine();

        Vector3 currentPos = transform.position;

        if (Mathf.Abs(currentPos.z - targetHorizontalZ) > laneChangeThreshold)
        {
            isChangingLanes = true;
        }
        else { isChangingLanes = false; } 

        float newZ = Mathf.Lerp(currentPos.z, targetHorizontalZ, Time.deltaTime * laneChangeSpeed);
        transform.position = new Vector3(currentPos.x, currentPos.y, newZ);
    }
    void PlayerChangeLine()
    {
        if (Input.GetKeyDown(_PlayerRight))
        {
            if (lane < 2)
            {
                lane++;
            }

        }
        if (Input.GetKeyDown(_PlayerLeft))
        {
            if (lane > 0)
            {
                lane--;
            }
        }
        if (lane == 0)
        {
            targetHorizontalZ = leftLaneX;
        }
        else if (lane == 1)
        {
            targetHorizontalZ = middleLaneX;
        }
        else if (lane == 2)
        {
            targetHorizontalZ = rightLaneX;
        }
    }
}
