using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPaused) GameManager.Instance.SwitchState<PauseState>(); isPaused = true;
            if (isPaused) GameManager.Instance.SwitchState<PlayingState>(); isPaused = false;
        }
    }
}
