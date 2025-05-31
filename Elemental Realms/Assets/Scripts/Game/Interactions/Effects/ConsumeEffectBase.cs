using Game.Data;
using UnityEngine;

namespace Game.Interactions.Effects
{
    public abstract class ConsumeEffectBase
    {
        public abstract void GetConsumed(GameObject go, ConsumeEffectInstance instance);
    }
}