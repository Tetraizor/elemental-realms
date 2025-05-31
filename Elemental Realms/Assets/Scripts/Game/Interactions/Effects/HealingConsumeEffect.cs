using Game.Components;
using Game.Data;
using UnityEngine;

namespace Game.Interactions.Effects
{
    public class HealingConsumeEffect : ConsumeEffectBase
    {
        public override void GetConsumed(GameObject target, ConsumeEffectInstance instance)
        {
            if (target.TryGetComponent(out HealthComponent health))
            {
                health.Heal(instance.Magnitude);
            }
        }
    }
}