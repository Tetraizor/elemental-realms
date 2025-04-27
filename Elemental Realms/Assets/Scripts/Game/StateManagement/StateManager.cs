using System;
using UnityEngine.Events;

namespace Game.StateManagement
{
    public class StateManager
    {
        public UnityEvent<Type> StateChanged = new();

        public StateBase CurrentState { private set; get; }


        public void SetState(StateBase state)
        {
            CurrentState?.Exit();

            CurrentState = state ?? throw new Exception("State cannot be null.");
            StateChanged?.Invoke(state.GetType());

            CurrentState.Enter();
        }

        public void TickState(float deltaTime)
        {
            CurrentState?.Tick(deltaTime);
        }

        public void FixedTickState(float fixedDeltaTime)
        {
            CurrentState?.FixedTick(fixedDeltaTime);
        }
    }
}