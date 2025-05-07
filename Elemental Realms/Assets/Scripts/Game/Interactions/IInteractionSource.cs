using System.Collections.Generic;
using Game.Interactions.Effects;

namespace Game.Interactions
{
    public interface IInteractionSource
    {
        public List<InteractionEffectSO> Effects { get; }

        public void Activate();
        public void Deactivate();
    }
}