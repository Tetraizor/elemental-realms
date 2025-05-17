using Game.Data;
using UnityEngine;

namespace Game.Interactions.Effects
{
    [System.Serializable]
    public abstract class InteractionEffect
    {
        public abstract void ApplyEffect(GameObject target, InteractionContext context);
    }
}