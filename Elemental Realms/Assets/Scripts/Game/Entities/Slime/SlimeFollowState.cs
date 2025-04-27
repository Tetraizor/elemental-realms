using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeFollowState : StateBase
    {
        private SlimeEntity _slime;
        private EntityBase _target;

        public SlimeFollowState(SlimeEntity slime, EntityBase target)
        {
            _slime = slime;
            _target = target;
        }

        public override void Enter()
        {
            _slime.SetSpeedMultiplier(1.5f);

            _slime.EntityAnimator.SetTrigger("SlimeFollow");
        }

        public override void Exit()
        {
            _slime.SetSpeedMultiplier(1);
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            Vector2 targetPositionDifference = _target.transform.position - _slime.transform.position;
            _slime.MovementDirection = targetPositionDifference.normalized;
            _slime.LookDirection = targetPositionDifference.normalized;

            if (targetPositionDifference.magnitude > 15)
            {
                _slime.StateManager.SetState(new SlimeIdleState(_slime));
            }

            if (targetPositionDifference.magnitude < 2.5f)
            {
                _slime.StateManager.SetState(new SlimeAttackState(_slime, _target));
            }
        }

        public override void Tick(float deltaTime) { }
    }
}