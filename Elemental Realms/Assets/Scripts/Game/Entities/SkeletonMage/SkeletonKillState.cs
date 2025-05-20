using System.Linq;
using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Skeleton
{
    public class SkeletonKillState : StateBase
    {
        private SkeletonEntity _skeleton;

        public SkeletonKillState(Entity entity)
        {
            _skeleton = entity as SkeletonEntity;
        }

        public override void Enter()
        {
            _skeleton.Moveable.MovementDirection = Vector2.zero;
            _skeleton.gameObject.GetComponentsInChildren<Collider2D>().ToList().ForEach((collider) => collider.enabled = false);

            _skeleton.GetComponent<Animator>().SetTrigger("SkeletonKill");
        }
        public override void Tick(float deltaTime) { }

        public override void FixedTick(float fixedDeltaTime) { }

        public override bool Exit(StateBase newState) => false;
    }
}
