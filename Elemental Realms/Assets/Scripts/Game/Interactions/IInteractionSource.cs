using System.Collections.Generic;
using Game.Interactions.Effects;

namespace Game.Interactions
{
    public interface IInteractionSource
    {
        public void Activate();
        public void Deactivate();
    }
}