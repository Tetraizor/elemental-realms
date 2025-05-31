using System.Collections.Generic;
using Game.Data;

namespace Game.Items
{
    public interface IItemConsumer
    {
        public void Consume(List<ConsumeEffectInstance> consumables);
    }
}