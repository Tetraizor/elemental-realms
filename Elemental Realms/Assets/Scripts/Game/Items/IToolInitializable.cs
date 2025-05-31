using Game.Data;
using UnityEngine;

namespace Game.Items
{
    public interface IToolInitializable
    {
        public void Setup(GameObject user, ItemInstance itemInstance);
    }
}