using Game.Entities.Player;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerMovementState : StateBase
    {
        private PlayerEntity _player;

        public PlayerMovementState(PlayerEntity player)
        {
            _player = player;
        }

        public override void Enter()
        {
            _player.EntityAnimator.SetTrigger("PlayerWalk");
        }

        public override void Exit()
        {
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (_player.MovementDirection.magnitude < 0.1f)
            {
                _player.StateManager.SetState(new PlayerIdleState(_player));
            }
        }
    }
}