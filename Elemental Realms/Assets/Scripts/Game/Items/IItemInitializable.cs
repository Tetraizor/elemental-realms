using Game.Inventories;

namespace Game.Items
{
    public interface IItemInitializable
    {
        public void InitializeWithItem(Item item);
    }
}