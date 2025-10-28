using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject player;

    void Start()
    {
        Vector3 playerPosition = player.transform.position;
    }

    void Update()
    {
        PlayerMove();
    }
    void PlayerMove()
    {

        
    }
}
