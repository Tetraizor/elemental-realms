using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Skeleton
{
    public class SkeletonIdleState : StateBase
    {
        private SkeletonEntity _skeleton;

        private const float WAIT_INTERVAL = 4;
        private const float SEARCH_INTERVAL = .5f;

        private float _currentWaitTime = 0;
        private float _currentSearchTime = 0;

        public SkeletonIdleState(Entity entity)
        {
            _skeleton = entity as SkeletonEntity;
        }

        public override void Enter()
        {
            _skeleton.Moveable.MovementDirection = Vector2.zero;

            _skeleton.GetComponent<Animator>().SetTrigger("SkeletonIdle");
        }

        public override bool Exit(StateBase newState)
        {
            return true;
        }

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Tick(float deltaTime)
        {
            _currentWaitTime += deltaTime;

            if (_currentWaitTime > WAIT_INTERVAL)
            {
                _skeleton.StateManager.SetState(new SkeletonPatrolState(_skeleton));
            }

            _currentSearchTime += deltaTime;

            if (_currentSearchTime > SEARCH_INTERVAL)
            {
                _skeleton.SearchForTargets();
                _currentSearchTime = 0;
            }
        }
    }
}