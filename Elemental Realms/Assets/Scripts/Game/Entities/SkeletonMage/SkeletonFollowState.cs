using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Skeleton
{
    public class SkeletonFollowState : StateBase
    {
        private SkeletonEntity _skeleton;
        private Entity _target;

        public SkeletonFollowState(SkeletonEntity skeleton, Entity target)
        {
            _skeleton = skeleton;
            _target = target;
        }

        public override void Enter()
        {
            _skeleton.Moveable.SetBaseSpeedMultiplier(1.5f);

            _skeleton.GetComponent<Animator>().SetTrigger("SkeletonFollow");
        }

        public override bool Exit(StateBase newState)
        {
            _skeleton.Moveable.SetBaseSpeedMultiplier(1);

            return true;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            Vector2 targetPositionDifference = _target.transform.position - _skeleton.transform.position;
            _skeleton.Moveable.MovementDirection = targetPositionDifference.normalized;
            _skeleton.Moveable.LookDirection = targetPositionDifference.normalized;

            if (targetPositionDifference.magnitude > 15)
            {
                _skeleton.StateManager.SetState(new SkeletonIdleState(_skeleton));
            }

            if (targetPositionDifference.magnitude < 5f)
            {
                _skeleton.StateManager.SetState(new SkeletonAttackState(_skeleton, _target));
            }
        }

        public override void Tick(float deltaTime) { }
    }
}