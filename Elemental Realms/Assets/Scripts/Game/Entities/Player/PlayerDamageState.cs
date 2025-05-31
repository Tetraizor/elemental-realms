
using System.Collections;
using Game.StateManagement;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerDamageState : StateBase
    {
        private PlayerEntity _player;
        private bool _canTransition = false;

        private Coroutine _exitDamageStateCoroutine = null;

        public PlayerDamageState(PlayerEntity player)
        {
            _player = player;
        }

        public override void Enter()
        {
            _player.GetComponent<Animator>().SetTrigger("PlayerDamage");

            _player.Moveable.CanMove = false;
            _player.Health.IsInvincible = true;

            _exitDamageStateCoroutine = _player.StartCoroutine(ExitDamageState());
        }

        private IEnumerator ExitDamageState()
        {
            yield return new WaitForSeconds(.3f);

            _canTransition = true;

            _player.StateManager.SetState(new PlayerIdleState(_player));
        }

        public override bool Exit(StateBase newState)
        {
            if (_canTransition || newState is PlayerDamageState || newState is PlayerKillState)
            {
                _player.Moveable.CanMove = true;
                _player.Health.IsInvincible = false;

                if (_exitDamageStateCoroutine != null) _player.StopCoroutine(_exitDamageStateCoroutine);

                return true;
            }

            return false;
        }

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Tick(float deltaTime) { }
    }
}