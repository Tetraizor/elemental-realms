using Game.Data;
using UnityEngine;

namespace Game.Interactions.Effects
{
    public abstract class InteractionEffectSO : ScriptableObject
    {
        public abstract void ApplyEffect(GameObject target, InteractionContext context);
    }
}