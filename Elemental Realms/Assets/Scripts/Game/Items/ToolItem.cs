using Game.Enum;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Tool", menuName = "Items/Tool Item", order = 0)]
    public class ToolItem : Item
    {
        [Header("Tool Properties")]
        public int Durability;
    }
}