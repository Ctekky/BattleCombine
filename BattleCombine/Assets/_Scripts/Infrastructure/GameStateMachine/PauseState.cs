using BattleCombine.Infrastructure;
using BattleCombine.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : IGameState
{
    private GameStateMachine _gameStateMachine;
   
    public PauseState(GameStateMachine stateMachine)
    {
        _gameStateMachine = stateMachine;
       
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }
}
