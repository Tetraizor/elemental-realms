
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerIdleState : StateBase
    {
        private PlayerEntity _player;

        public PlayerIdleState(PlayerEntity player)
        {
            _player = player;
        }

        public override void Enter()
        {
            _player.GetComponent<Animator>().SetTrigger("PlayerIdle");
        }

        public override bool Exit(StateBase newState) => true;

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (_player.Moveable.MovementDirection.magnitude > 0.1f)
            {
                _player.StateManager.SetState(new PlayerMovementState(_player));
            }
        }
    }
}