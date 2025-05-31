using Game.Entities;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimePatrolState : StateBase
    {
        private SlimeEntity _slime;
        private Vector2 _moveDirection = Vector2.zero;

        private float _currentWaitTime = 0;
        private float _waitInterval = 4;

        private float _currentSearchTime = 0;
        private const float SEARCH_INTERVAL = .3f;

        public SlimePatrolState(SlimeEntity slime)
        {
            _slime = slime;
        }

        public override void Enter()
        {
            _waitInterval *= Random.Range(.8f, 1.3f);

            var targetPosition = _slime.SpawnPosition + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * Random.Range(0, 5.0f);
            _moveDirection = (targetPosition - (Vector2)_slime.transform.position).normalized;

            _slime.Moveable.MovementDirection = _moveDirection;
            _slime.Moveable.LookDirection = _moveDirection;

            _slime.GetComponent<Animator>().SetTrigger("SlimeWalk");

            _currentSearchTime = SEARCH_INTERVAL;
        }

        public override bool Exit(StateBase newState) => true;

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Tick(float deltaTime)
        {
            _currentWaitTime += deltaTime;

            if (_currentWaitTime > _waitInterval)
            {
                _slime.StateManager.SetState(new SlimeIdleState(_slime));
            }

            _currentSearchTime += deltaTime;

            if (_currentSearchTime > SEARCH_INTERVAL)
            {
                _slime.SearchForTargets();
            }
        }
    }
}