using UnityEngine;
using UnityEngine.UI;

public class PlayingState : State
{
    [SerializeField] private S_TimerAndScore TimerAndScore;
    [SerializeField] private KitchenGenerator KitchenGenerator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraScript cameraScript;

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        cameraScript.UpdateCamera();
        playerMovement.PlayerUpdate();
        TimerAndScore.UpdateTimer();
        KitchenGenerator.UpdateKitchenGenerator();

        
    }
}
