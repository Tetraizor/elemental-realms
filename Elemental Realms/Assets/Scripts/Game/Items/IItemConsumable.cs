using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Items
{
    public interface IItemConsumable
    {
        public List<ConsumeEffectInstance> GetEffects();
    }
}