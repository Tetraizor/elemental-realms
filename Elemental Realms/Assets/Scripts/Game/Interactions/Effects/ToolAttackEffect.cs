using Game.Components;
using Game.Data;
using Game.Enum;
using UnityEngine;

namespace Game.Interactions.Effects
{
    [System.Serializable]
    public class AttackEffect : InteractionEffect
    {
        public DamageType Type = DamageType.Blunt;
        public float Damage = 5;
        public float Knockback = 1;

        public override void ApplyEffect(GameObject target, InteractionContext context)
        {
            base.ApplyEffect(target, context);

            if (target.TryGetComponent(out HealthComponent health))
            {
                health.TakeDamage(Damage, Type);
            }

            if (target.TryGetComponent(out Rigidbody2D rb))
            {
                float knockbackResistance = health != null ? health.KnockbackResistance : .5f;

                rb.AddForce(context.HitDirection * rb.mass * 20 * Knockback * (1 - knockbackResistance), ForceMode2D.Impulse);
            }
        }
    }
}