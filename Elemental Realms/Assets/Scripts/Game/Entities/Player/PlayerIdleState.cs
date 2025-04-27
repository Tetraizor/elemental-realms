
using Game.Data;
using Game.Entities.Player;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerIdleState : StateBase
    {
        private PlayerEntity _player;

        public PlayerIdleState(PlayerEntity player)
        {
            _player = player;

            _player.InteractionStarted.AddListener(OnInteractionStarted);
        }

        public override void Enter()
        {
            _player.EntityAnimator.SetTrigger("PlayerIdle");
        }

        public override void Exit()
        {
            _player.InteractionStarted.RemoveListener(OnInteractionStarted);
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (_player.MovementDirection.magnitude > 0.1f)
            {
                _player.StateManager.SetState(new PlayerMovementState(_player));
            }
        }

        public void OnInteractionStarted(InteractionData data)
        {

        }

        public void OnInteractionEnded(InteractionData data)
        {

        }
    }
}