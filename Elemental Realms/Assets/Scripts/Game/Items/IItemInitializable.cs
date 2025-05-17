using Game.Data;

namespace Game.Items
{
    public interface IItemInitializable
    {
        public void InitializeWithItem(ItemInstance itemInstance);
    }
}