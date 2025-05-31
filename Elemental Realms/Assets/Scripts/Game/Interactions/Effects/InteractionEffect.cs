using Game.Data;
using UnityEngine;

namespace Game.Interactions.Effects
{
    [System.Serializable]
    public abstract class InteractionEffect
    {
        public virtual void ApplyEffect(GameObject target, InteractionContext context)
        {
            if (target.TryGetComponent(out IInteractionContextConsumer consumer))
            {
                consumer.ConsumeContext(context);
            }
        }
    }
}