
using Game.Enum;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerDashState : StateBase
    {
        private PlayerEntity _player;
        private float _dashDuration = .3f;
        private float _totalDashDuration = .3f;
        private Vector2 _dashDirection = Vector2.zero;
        private float _speedMultiplier = 1;
        private float _dashSpeed = 1;

        public PlayerDashState(PlayerEntity player, Vector2 dashDirection, float speedMultiplier, float dashSpeed, float dashDuration)
        {
            _player = player;
            _dashDirection = dashDirection;
            _dashSpeed = dashSpeed;
            _speedMultiplier = speedMultiplier;
            _dashDuration = dashDuration;
            _totalDashDuration = dashDuration;

            if (dashDirection.magnitude <= .1f)
            {
                _player.StateManager.SetState(new PlayerIdleState(_player));
            }
            else
            {
                _player.GetComponent<Animator>().SetTrigger("PlayerDash");
                _player.EntityRigidbody.linearVelocity = Vector2.zero;
                _player.Moveable.CanMove = false;
                _player.Moveable.DirectionMode = MoveableDirectionMode.SetByMovementVector;
            }
        }

        public override void Enter() { }

        public override bool Exit(StateBase newState)
        {
            bool canTransition = _dashDuration <= 0;

            if (canTransition) _player.Moveable.CanMove = true;

            _player.Moveable.DirectionMode = MoveableDirectionMode.SetByLookVector;

            return canTransition;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            if (_dashDuration / _totalDashDuration > .5f)
                _player.EntityRigidbody.AddForce(_dashDirection * _player.EntityRigidbody.mass * _dashSpeed * _speedMultiplier * 200);
        }

        public override void Tick(float deltaTime)
        {
            if (_dashDuration < 0) _player.StateManager.SetState(new PlayerIdleState(_player));

            _dashDuration -= deltaTime;
        }
    }
}