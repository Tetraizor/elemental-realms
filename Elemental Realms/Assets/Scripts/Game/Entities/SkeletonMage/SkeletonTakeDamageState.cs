using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Skeleton
{
    public class SkeletonTakeDamageState : StateBase
    {
        private SkeletonEntity _skeleton;

        private const float WAIT_INTERVAL = .3f;
        private float _currentWaitTime = 0;

        public SkeletonTakeDamageState(Entity entity)
        {
            _skeleton = entity as SkeletonEntity;
        }

        public override void Tick(float deltaTime)
        {
            _currentWaitTime += deltaTime;

            if (_currentWaitTime > WAIT_INTERVAL)
            {
                _skeleton.StateManager.SetState(new SkeletonPatrolState(_skeleton));
            }
        }

        public override void Enter()
        {
            _skeleton.Moveable.MovementDirection = Vector2.zero;

            _skeleton.GetComponent<Animator>().SetTrigger("SkeletonGetHit");
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override bool Exit(StateBase newState) => true;
    }
}
