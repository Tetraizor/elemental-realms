using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeIdleState : StateBase
    {
        private SlimeEntity _slime;

        private const float WAIT_INTERVAL = 4;
        private const float SEARCH_INTERVAL = .5f;

        private float _currentWaitTime = 0;
        private float _currentSearchTime = 0;

        public SlimeIdleState(Entity entity)
        {
            _slime = entity as SlimeEntity;
        }

        public override void Tick(float deltaTime)
        {
            _currentWaitTime += deltaTime;

            if (_currentWaitTime > WAIT_INTERVAL)
            {
                _slime.StateManager.SetState(new SlimePatrolState(_slime));
            }

            _currentSearchTime += deltaTime;

            if (_currentSearchTime > SEARCH_INTERVAL)
            {
                _slime.SearchForTargets();
                _currentSearchTime = 0;
            }
        }

        public override void Enter()
        {
            _slime.Moveable.MovementDirection = Vector2.zero;

            _slime.GetComponent<Animator>().SetTrigger("SlimeIdle");
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override bool Exit(StateBase newState)
        {
            return true;
        }
    }
}
