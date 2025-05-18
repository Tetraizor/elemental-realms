using System.Collections.Generic;
using Game.Interactions.Effects;

namespace Game.Interactions
{
    public interface IInteractionSource
    {
        public void Activate(int actionCode = 0);
        public void Deactivate(int actionCode = 0);
    }
}