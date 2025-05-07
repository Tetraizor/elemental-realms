using System.Linq;
using Game.Components;
using Game.Data;
using Game.Enum;
using UnityEngine;

namespace Game.Interactions.Effects
{
    [CreateAssetMenu(fileName = "Area Attack Effect", menuName = "Effects/Area Attack Effect", order = 0)]
    public class AreaAttackEffectSO : InteractionEffectSO
    {
        public DamageType Type = DamageType.Blunt;
        public float Damage = 4;

        public string[] TargetTags = new string[] { "Player" };

        public override void ApplyEffect(GameObject target, InteractionContext context)
        {
            if (!TargetTags.ToList().Contains(target.tag)) return;

            if (target.TryGetComponent(out HealthComponent health))
            {
                health.TakeDamage(Damage);
            }

            if (target.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(context.HitDirection * rb.mass * 10, ForceMode2D.Impulse);
            }
        }
    }
}