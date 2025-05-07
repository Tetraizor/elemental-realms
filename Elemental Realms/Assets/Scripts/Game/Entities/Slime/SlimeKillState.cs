using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeKillState : StateBase
    {
        private SlimeEntity _slime;

        public SlimeKillState(Entity entity)
        {
            _slime = entity as SlimeEntity;
        }

        public override void Enter()
        {
            _slime.Moveable.MovementDirection = Vector2.zero;

            _slime.GetComponent<Animator>().SetTrigger("SlimeKill");
        }

        public override void Tick(float deltaTime) { }

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Exit() { }
    }
}
