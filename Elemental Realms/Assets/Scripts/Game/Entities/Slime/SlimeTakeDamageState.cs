using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeTakeDamageState : StateBase
    {
        private SlimeEntity _slime;

        private const float WAIT_INTERVAL = 1;
        private float _currentWaitTime = 0;

        public SlimeTakeDamageState(EntityBase entity)
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
        }

        public override void Enter()
        {
            _slime.MovementDirection = Vector2.zero;

            _slime.EntityAnimator.SetTrigger("SlimeGetHit");
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}
