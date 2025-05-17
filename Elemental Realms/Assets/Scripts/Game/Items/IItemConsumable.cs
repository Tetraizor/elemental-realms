using Game.Data;

namespace Game.Items
{
    public interface IItemConsumable
    {
        public ConsumeEffectAttribute[] Consume();
    }
}