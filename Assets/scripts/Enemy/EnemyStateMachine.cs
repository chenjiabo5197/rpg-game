using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyStatus currentState {  get; private set; }

    public void Initialize(EnemyStatus _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyStatus _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
