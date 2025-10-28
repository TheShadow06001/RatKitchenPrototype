using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    Vector3 moveDirection;
    void Start()
    {
        
    }

    void Update()
    {
        PlayerMove();
    }
    void PlayerMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
