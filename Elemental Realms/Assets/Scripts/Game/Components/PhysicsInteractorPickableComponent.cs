using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Interactions;
using Game.Items;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsInteractorPickableComponent : PickableComponent
    {
        [Header("Physics Properties")]
        [SerializeField] private float _activationVelocityThreshold = 20;
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

            if (_itemInstance.Item is ToolItem toolItem)
            {
                toolItem.AttackEffects.ForEach(effect => effect.ApplyEffect(collision.gameObject, ctx));
            }
        }

        private void Update()
        {
            if (!IsActive) return;

            if (_rigidbody.linearVelocity.magnitude < _activationVelocityThreshold)
            {
                IsActive = false;

                _rigidbody.angularDamping = _groundAngularFriction;
                _rigidbody.linearDamping = _groundLinearFriction;
            }
        }
    }
}