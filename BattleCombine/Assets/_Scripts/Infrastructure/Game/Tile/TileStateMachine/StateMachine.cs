using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State startState)
        {
            CurrentState = startState;
            startState.Enter();
        }
        public void ChangeState(State newState)
        {
            CurrentState.Exit();

            CurrentState = newState;
            newState.Enter();
        }
    }
}

