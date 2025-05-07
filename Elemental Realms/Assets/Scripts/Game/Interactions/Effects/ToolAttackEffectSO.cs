using Game.Components;
using Game.Data;
using Game.Enum;
using Game.Tools;
using UnityEngine;

namespace Game.Interactions.Effects
{
    [CreateAssetMenu(fileName = "Tool Attack Effect", menuName = "Effects/Tool Attack Effect", order = 0)]
    public class ToolAttackEffectSO : InteractionEffectSO
    {
        public DamageType Type = DamageType.Blunt;
        public float Damage = 8;

        public override void ApplyEffect(GameObject target, InteractionContext context)
        {
            if (target.TryGetComponent(out HealthComponent health))
            {
                health.TakeDamage(Damage);
            }

            if (target.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(context.HitDirection * rb.mass * 20, ForceMode2D.Impulse);
            }
        }
    }
}