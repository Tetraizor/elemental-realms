using Game.Components;
using Game.Entities.Common;
using UnityEngine;

namespace Game.Entities.Player
{
    [RequireComponent(typeof(MoveableComponent))]
    public class PlayerEntity : Entity
    {
        [HideInInspector] public MoveableComponent Moveable { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Health = GetComponent<HealthComponent>();
            Moveable = GetComponent<MoveableComponent>();
        }

        protected override void Start()
        {
            base.Start();

            StateManager.SetState(new PlayerIdleState(this));
        }
    }
}