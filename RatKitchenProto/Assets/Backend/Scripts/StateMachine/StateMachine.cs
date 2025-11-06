using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public List<State> states = new List<State>();
    public State currentState = null;

    public void SwitchState<aState>()
    {
        foreach (State s in states)
        {
            if (s.GetType() == typeof(aState))
            {
                currentState?.ExitState();
                currentState = s;
                currentState.EnterState();
                return;
            }
        }

        Debug.LogWarning("State not found");
    }

    public virtual void UpdateStateMachine() 
    {
        currentState?.UpdateState();
    }

    public bool IsState<aState>()
    {
        if (!currentState) return false;
        return currentState.GetType() == typeof(aState);
    }
}
