using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR;

enum EState
{
    PATROL,
    CHASE,
    ATTACK,
    DEATH
}
public class MonsterStateContext
{
    public IState CurrentState { get; set; }
    private readonly Monster controller;
    public MonsterStateContext(Monster controller)
    {
        this.controller = controller;
    }

    public void Transition(IState state)
    {
        if (CurrentState != null)
            CurrentState.ExitState();

        CurrentState = state;

        CurrentState.EnterState();
    }
}