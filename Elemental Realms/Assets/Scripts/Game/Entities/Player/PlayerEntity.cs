using System;
using Game.Components;
using Game.Controllers;
using Game.Entities.Common;
using Game.Interactions;
using Game.Tools;
using UnityEngine;

namespace Game.Entities.Player
{
    [RequireComponent(typeof(MoveableComponent))]
    public class PlayerEntity : Entity
    {
        [HideInInspector] public MoveableComponent Moveable { get; private set; }
        [HideInInspector] public OrbitComponent Orbit { get; private set; }
        [HideInInspector] public InteractorComponent ItemPicker { get; private set; }
        [HideInInspector] public IInteractionSource InteractionSource { get; set; }

        [SerializeField] private GameObject _fistPrefab;
        [SerializeField] private float _dashSpeed = 1;
        [SerializeField] private float _dashDuration = .3f;
        [SerializeField] private float _dashCooldown = 1;

        private float _dashCooldownTimer = 1;

        protected override void Awake()
        {
            base.Awake();

            Moveable = GetComponent<MoveableComponent>();
            Orbit = GetComponent<OrbitComponent>();
            ItemPicker = GetComponent<InteractorComponent>();

            Health.Changed.AddListener(OnHealthChanged);
        }

        public void SetLookDirection(Vector2 lookDirection)
        {
            Orbit.Direction = lookDirection;
            Moveable.LookDirection = lookDirection;
        }

        private void OnHealthChanged(float newHealth)
        {
            if (newHealth != 0)
                StateManager.SetState(new PlayerDamageState(this));
        }

        protected override void Start()
        {
            base.Start();

            StateManager.SetState(new PlayerIdleState(this));
        }

        protected override void Update()
        {
            base.Update();

            _dashCooldownTimer = Math.Max(0, _dashCooldownTimer - Time.deltaTime);
        }

        protected override void Kill()
        {
            base.Kill();

            StateManager.SetState(new PlayerKillState(this));
        }

        public void Dash()
        {
            if (_dashCooldownTimer <= 0)
            {
                StateManager.SetState(new PlayerDashState(this, Moveable.MovementDirection, Moveable.GetFinalSpeedMultiplier(), _dashSpeed, _dashDuration));
                _dashCooldownTimer = _dashCooldown;
            }
        }
    }
}