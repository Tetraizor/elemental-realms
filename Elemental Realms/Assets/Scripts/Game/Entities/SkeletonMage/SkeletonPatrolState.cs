using Game.Entities;
using UnityEngine;

namespace Game.Entities.Skeleton
{
    public class SkeletonPatrolState : StateBase
    {
        private SkeletonEntity _skeleton;
        private Vector2 _moveDirection = Vector2.zero;

        private float _currentWaitTime = 0;
        private float _waitInterval = 4;

        private float _currentSearchTime = 0;
        private const float SEARCH_INTERVAL = .3f;

        public SkeletonPatrolState(SkeletonEntity skeleton)
        {
            _skeleton = skeleton;
        }

        public override void Enter()
        {
            _waitInterval *= Random.Range(.8f, 1.3f);

            var targetPosition = _skeleton.SpawnPosition + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * Random.Range(0, 5.0f);
            _moveDirection = (targetPosition - (Vector2)_skeleton.transform.position).normalized;

            _skeleton.Moveable.MovementDirection = _moveDirection;
            _skeleton.Moveable.LookDirection = _moveDirection;

            _skeleton.GetComponent<Animator>().SetTrigger("SkeletonWalk");

            _currentSearchTime = SEARCH_INTERVAL;
        }

        public override bool Exit(StateBase newState) => true;

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Tick(float deltaTime)
        {
            _currentWaitTime += deltaTime;

            if (_currentWaitTime > _waitInterval)
            {
                _skeleton.StateManager.SetState(new SkeletonIdleState(_skeleton));
            }

            _currentSearchTime += deltaTime;

            if (_currentSearchTime > SEARCH_INTERVAL)
            {
                _skeleton.SearchForTargets();
            }
        }
    }
}