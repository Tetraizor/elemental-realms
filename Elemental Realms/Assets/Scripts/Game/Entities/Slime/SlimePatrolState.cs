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
        private float _searchInterval = .5f;

        public SlimePatrolState(SlimeEntity slime)
        {
            _slime = slime;
        }

        public override void Enter()
        {
            _moveDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            _slime.Moveable.MovementDirection = _moveDirection;
            _slime.Moveable.LookDirection = _moveDirection;

            _slime.GetComponent<Animator>().SetTrigger("SlimeWalk");
        }

        public override void Exit()
        {
        }

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Tick(float deltaTime)
        {
            _currentWaitTime += deltaTime;

            if (_currentWaitTime > _waitInterval)
            {
                _slime.StateManager.SetState(new SlimeIdleState(_slime));
            }

            _currentSearchTime += deltaTime;

            if (_currentSearchTime > _searchInterval)
            {
                _slime.SearchForTargets();
            }
        }
    }
}