using Game.Data;
using UnityEngine.Events;

namespace Game.Items
{
    public interface IItemThrowable
    {
        public void Throw(ItemInstance thrownItem);
        public UnityEvent<ItemInstance> GetThrownEvent();
    }
}