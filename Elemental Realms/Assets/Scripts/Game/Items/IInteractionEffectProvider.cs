using System.Collections.Generic;
using Game.Interactions.Effects;

namespace Game.Items
{
    public interface IInteractionEffectProvider
    {
        public List<AttackEffect> GetEffects();
    }
}