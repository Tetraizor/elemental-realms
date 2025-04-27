using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeKillState : StateBase
    {
        private SlimeEntity _slime;

        public SlimeKillState(EntityBase entity)
        {
            _slime = entity as SlimeEntity;
        }

        public override void Enter()
        {
            _slime.MovementDirection = Vector2.zero;

            _slime.EntityAnimator.SetTrigger("SlimeKill");
        }

        public override void Tick(float deltaTime) { }

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Exit() { }
    }
}
