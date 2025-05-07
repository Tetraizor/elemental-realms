using Game.Data;
using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeAttackState : StateBase
    {
        private SlimeEntity _slime;
        private Entity _target;

        private const float ATTACK_INTERVAL = 1.5f;

        private float _currentAttackTime = 0;

        public SlimeAttackState(SlimeEntity slime, Entity target)
        {
            _slime = slime;
            _target = target;
        }

        public override void Tick(float deltaTime)
        {
            _currentAttackTime += deltaTime;

            if (_currentAttackTime > ATTACK_INTERVAL)
            {
                _slime.StateManager.SetState(new SlimeAttackState(_slime, _target));
            }

            Vector2 targetPositionDifference = _target.transform.position - _slime.transform.position;
            if (targetPositionDifference.magnitude > 2.5f)
            {
                _slime.StateManager.SetState(new SlimeFollowState(_slime, _target));
            }

            _slime.Moveable.LookDirection = (_target.transform.position - _slime.transform.position).normalized;
        }

        public override void Enter()
        {
            _slime.Moveable.MovementDirection = Vector2.zero;
        }

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Exit()
        {
        }
    }
}
