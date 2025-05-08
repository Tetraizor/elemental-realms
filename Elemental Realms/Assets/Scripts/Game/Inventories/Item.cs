using Game.Enum;
using UnityEngine;

namespace Game.Inventories
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
    public class Item : ScriptableObject
    {
        public int Id = 0;
        public string Name = "Item Name";
        public string Description = "Item Description";
        public Sprite Sprite = null;

        public ItemType Type = ItemType.Material;

        public int Mass = 5;
        public int MaxStackSize = 99;

        public GameObject Prefab;
    }
}