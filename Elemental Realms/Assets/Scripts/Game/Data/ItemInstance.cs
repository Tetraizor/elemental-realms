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

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
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
                if (Item == null || other.Item == null)
                    return false;

                return !(Item is ToolItem) && Item.Id == other.Item.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Item != null ? Item.Id.GetHashCode() : 0;
        }
    }
}