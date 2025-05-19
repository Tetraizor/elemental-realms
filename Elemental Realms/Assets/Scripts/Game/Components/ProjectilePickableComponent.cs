using Game.Data;
using Game.Items;
using UnityEngine;

namespace Game.Components
{
    public class ProjectilePickableComponent : PhysicsInteractorPickableComponent
    {
        [Header("Projectile Properties")]
        [SerializeField] private float _destroyOnHitChance = .5f;
        [SerializeField] private float _stickOnHitChance = .5f;

        private void StickToCollider(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                var joint = gameObject.AddComponent<FixedJoint2D>();
                joint.connectedBody = rb;
            }
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsActive) return;
            base.OnCollisionEnter2D(collision);

            IsActive = false;

            if (Random.Range(0, 1.0f) < _destroyOnHitChance) Destroy(gameObject);

            if (Random.Range(0, 1.0f) < _stickOnHitChance)
            {
                if (!collision.gameObject.TryGetComponent(out PickableComponent pickable))
                {
                    StickToCollider(collision);
                }
            }
        }
    }
}