
using System.Collections;
using Game.StateManagement;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerKillState : StateBase
    {
        private PlayerEntity _player;

        public PlayerKillState(PlayerEntity player)
        {
            _player = player;
        }

        public override void Enter()
        {
            _player.GetComponent<Animator>().SetTrigger("PlayerKill");
            _player.Moveable.CanMove = false;
        }

        public override bool Exit(StateBase newState) => false;

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Tick(float deltaTime) { }
    }
}