using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Entities.Common;
using Game.Interactions;
using Game.Items;
using Game.Status;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsInteractorPickableComponent : PickableComponent
    {
        [Header("Physics Properties")]
        [SerializeField] public float ActivationVelocityThreshold = 5;
        [SerializeField] private float _groundLinearFriction = 8;
        [SerializeField] private float _groundAngularFriction = 5;

        // References
        private Rigidbody2D _rigidbody;

        public bool IsActive = true;

        public override void InitializeWithItem(ItemInstance instance)
        {
            base.InitializeWithItem(instance);

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsActive) return;

            var ctx = new InteractionContext
            {
                Source = gameObject,
                HitDirection = (collision.gameObject.transform.position - transform.position).normalized,
                HitPoint = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position),
                Speed = collision.relativeVelocity.magnitude
            };

            if (_itemInstance.Item is IInteractionEffectProvider effectProvider)
            {
                effectProvider.GetAttackEffects().ForEach(effect => effect.ApplyEffect(collision.gameObject, ctx));
            }

            if (_itemInstance.Item is IStatusEffectProvider statusProvider && collision.gameObject.TryGetComponent(out Entity entity))
            {
                statusProvider.GetStatusEffects().ForEach(status => entity.StatusManager.AddStatus(new StatusInstance { Status = status }));
            }
        }

        private void Update()
        {
            if (!IsActive) return;

            if (_rigidbody.linearVelocity.magnitude < ActivationVelocityThreshold)
            {
                IsActive = false;

                _rigidbody.angularDamping = _groundAngularFriction;
                _rigidbody.linearDamping = _groundLinearFriction;
            }
        }
    }
}