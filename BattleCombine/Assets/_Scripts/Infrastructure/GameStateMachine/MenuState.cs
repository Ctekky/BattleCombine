using BattleCombine.Infrastructure;
using BattleCombine.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : IPayloadedState<string>
{
    private GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private BtnLoadPreBattleScene _button;
    public MenuState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
        _gameStateMachine = stateMachine;
        _sceneLoader = sceneLoader;
    }



    public void Exit()
    {
        _gameStateMachine.Enter<LoadLevelState, string>("Game_LoopDS");
    }

    public void Enter(string sceneName)
    {
        _sceneLoader.Load(sceneName, OnLoaded);
        Debug.Log("EnterMenu");
      
    }

    private void OnLoaded()
    {
       
        _gameStateMachine.Enter<GameLoopState>();
    }
}
