using System;
using BattleCombine.Interfaces;
using UnityEngine;

namespace BattleCombine.Infrastructure
{
    public class BootStrapState : IGameState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private const string Initial = "Initial";

        public BootStrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegistryServices();

            _sceneLoader.Load(Initial, EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<MenuState, string>("Menu");//("Game_LoopDS");
        }

        private void RegistryServices()
        {
            Debug.Log("Load bootstrap state");
        }

        public void Exit()
        {
        }
    }
}