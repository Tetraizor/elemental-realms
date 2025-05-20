using Game.Data;
using Game.Items;
using UnityEngine;

namespace Game.Components
{
    public class ProjectilePickableComponent : PhysicsInteractorPickableComponent
    {
        [Header("Projectile Properties")]
        [SerializeField] public float DestroyOnHitChance = .5f;
        [SerializeField] public float StickOnHitChance = .5f;

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

            if (Random.Range(0, 1.0f) < DestroyOnHitChance) Destroy(gameObject);

            if (Random.Range(0, 1.0f) < StickOnHitChance)
            {
                if (!collision.gameObject.TryGetComponent(out PickableComponent pickable))
                {
                    StickToCollider(collision);
                }
            }
        }
    }
}