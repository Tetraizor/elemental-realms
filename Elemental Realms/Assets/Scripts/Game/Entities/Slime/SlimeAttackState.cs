using System.Collections;
using Game.Data;
using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeAttackState : StateBase
    {
        private SlimeEntity _slime;
        private Entity _target;

        private const float ATTACK_INTERVAL = 1.5f;

        private float _currentAttackTime = 0;
        private Coroutine _attackCoroutine = null;

        public SlimeAttackState(SlimeEntity slime, Entity target)
        {
            _slime = slime;
            _target = target;
        }

        public override void Tick(float deltaTime)
        {
            _currentAttackTime += deltaTime;

            if (_currentAttackTime > ATTACK_INTERVAL)
            {

                Vector2 targetPositionDifference = _target.transform.position - _slime.transform.position;
                if (targetPositionDifference.magnitude > 2.5f)
                {
                    _slime.StateManager.SetState(new SlimeFollowState(_slime, _target));
                }
                else
                {
                    _slime.StateManager.SetState(new SlimeAttackState(_slime, _target));
                }
            }

            _slime.Moveable.LookDirection = (_target.transform.position - _slime.transform.position).normalized;
        }

        public override void Enter()
        {
            _slime.Moveable.MovementDirection = Vector2.zero;

            _attackCoroutine = _slime.StartCoroutine(StartAttack());
        }

        public override void FixedTick(float fixedDeltaTime) { }

        public override bool Exit(StateBase newState)
        {
            if (newState is SlimeTakeDamageState) return false;

            if (_attackCoroutine != null) _slime.StopCoroutine(_attackCoroutine);
            _slime.AreaDamager.Deactivate();

            return true;
        }

        private IEnumerator StartAttack()
        {
            _slime.GetComponent<Animator>().SetTrigger("SlimeAttack");

            yield return new WaitForSeconds(.7f);

            _slime.AreaDamager.Activate();

            yield return new WaitForSeconds(.5f);
        }
    }
}
