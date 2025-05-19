using System;
using System.Collections.Generic;
using Game.Components;
using Game.Data;
using Game.Entities.Common;
using Game.Interactions;
using Game.Items;
using UnityEngine;

namespace Game.Entities.Player
{
    [RequireComponent(typeof(MoveableComponent))]
    public class PlayerEntity : Entity, IItemConsumer
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

        public void SetLookDirection(Vector2 lookDirection)
        {
            Orbit.Direction = lookDirection;
            Moveable.LookDirection = lookDirection;
        }

        private void OnHealthChanged(float oldHealth, float newHealth)
        {
            if (newHealth != 0)
                StateManager.SetState(new PlayerDamageState(this));
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

        public void Consume(List<ConsumeEffectInstance> consumables)
        {
            consumables.ForEach(consumable => consumable.GetConsumed(gameObject));
        }

        #region Input Handlers

        public void ActivateInteractionSourcePrimary() => InteractionSource?.Activate(0);
        public void DeactivateInteractionSourcePrimary() => InteractionSource?.Deactivate(0);
        public void ActivateInteractionSourceSecondary() => InteractionSource?.Activate(1);
        public void DeactivateInteractionSourceSecondary() => InteractionSource?.Deactivate(1);

        #endregion
    }
}