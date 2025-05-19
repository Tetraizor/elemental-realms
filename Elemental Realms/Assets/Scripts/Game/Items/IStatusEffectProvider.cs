using System.Collections.Generic;
using Game.Status;

namespace Game.Items
{
    public interface IStatusEffectProvider
    {
        public List<StatusBaseSO> GetStatusEffects();
    }
}