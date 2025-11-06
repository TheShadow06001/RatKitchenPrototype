using UnityEngine;
using UnityEngine.UI;

public class PlayingState : State
{
    [SerializeField] private S_TimerAndScore TimerAndScore;
    [SerializeField] private KitchenGenerator KitchenGenerator;
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
        //TimerAndScore.UpdateTimer();
        KitchenGenerator.UpdateKitchenGenerator();
    }
}
