using System.Collections;
using Game.Data;
using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Skeleton
{
    public class SkeletonAttackState : StateBase
    {
        private SkeletonEntity _skeleton;
        private Entity _target;

        private Coroutine _attackCoroutine = null;
        private bool _finished = false;

        public SkeletonAttackState(SkeletonEntity skeleton, Entity target)
        {
            _skeleton = skeleton;
            _target = target;
        }

        public override void Tick(float deltaTime)
        {
            _skeleton.Moveable.LookDirection = (_target.transform.position - _skeleton.transform.position).normalized;
        }

        public override void Enter()
        {
            _skeleton.Moveable.MovementDirection = Vector2.zero;

            _attackCoroutine = _skeleton.StartCoroutine(StartAttack());
        }

        public override void FixedTick(float fixedDeltaTime) { }

        public override bool Exit(StateBase newState)
        {
            if (!_finished && newState is SkeletonTakeDamageState) return false;

            if (_attackCoroutine != null) _skeleton.StopCoroutine(_attackCoroutine);

            return true;
        }

        private IEnumerator StartAttack()
        {
            _skeleton.GetComponent<Animator>().SetTrigger("SkeletonCast");

            yield return new WaitForSeconds(2.7f);

            _skeleton.SpawnProjectile(_target.transform.position);
            _finished = true;

            yield return new WaitForSeconds(3);

            _skeleton.StateManager.SetState(new SkeletonPatrolState(_skeleton));
        }
    }
}
