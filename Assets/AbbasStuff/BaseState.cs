using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public StateMachine stateMachine;
    public Ghost ghost;
    public abstract void Enter();
    public abstract void Execute();

    public abstract void Exit();


}
