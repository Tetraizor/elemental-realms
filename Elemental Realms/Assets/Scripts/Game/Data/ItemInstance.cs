using Game.Items;

namespace Game.Data
{
    [System.Serializable]
    public class ItemInstance
    {
        public Item Item;
        public int Durability;

        public static bool operator ==(ItemInstance a, ItemInstance b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ItemInstance a, ItemInstance b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is ItemInstance other)
            {
                return !(Item is ToolItem) && Item.Id == other.Item.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return GetHashCode();
        }
    }
}