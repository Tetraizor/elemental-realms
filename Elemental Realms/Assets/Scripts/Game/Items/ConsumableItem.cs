using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable Item", order = 0)]
    public class ConsumableItem : Item, IItemConsumable
    {
        [Header("Consumable Properties")]
        public List<ConsumeEffectInstance> Effects;

        public List<ConsumeEffectInstance> GetEffects() => Effects;
    }
}