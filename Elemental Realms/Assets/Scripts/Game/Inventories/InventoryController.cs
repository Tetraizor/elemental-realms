using System.Collections.Generic;
using Game.Data;
using Game.Enum;
using Game.Items;
using Tetraizor.MonoSingleton;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Inventories
{
    public class InventoryController : MonoSingleton<InventoryController>
    {
        private const int MATERIAL_CAPACITY = 25;
        private const int EQUIPMENT_CAPACITY = 16;
        private const int TOOLS_CAPACITY = 16;

        [HideInInspector] public Dictionary<InventoryType, List<SlotData>> Inventories { get; private set; }
        [HideInInspector] public UnityEvent<InventoryType, List<SlotData>> InventoryChanged;

        [HideInInspector] public UnityEvent<ItemInstance> ItemPicked;
        [HideInInspector] public UnityEvent<ItemInstance> ItemRemoved;

        protected override void Init()
        {
            base.Init();

            Inventories = new Dictionary<InventoryType, List<SlotData>> {
                { InventoryType.MaterialInventory, new List<SlotData>(MATERIAL_CAPACITY) },
                { InventoryType.GearInventory, new List<SlotData>(EQUIPMENT_CAPACITY) },
                { InventoryType.ToolsInventory, new List<SlotData>(TOOLS_CAPACITY) },
            };

            for (int i = 0; i < MATERIAL_CAPACITY; i++)
            {
                Inventories[InventoryType.MaterialInventory].Add(new SlotData());
            }

            for (int i = 0; i < EQUIPMENT_CAPACITY; i++)
            {
                Inventories[InventoryType.GearInventory].Add(new SlotData());
            }

            for (int i = 0; i < TOOLS_CAPACITY; i++)
            {
                Inventories[InventoryType.ToolsInventory].Add(new SlotData());
            }
        }

        public bool AddItem(InventoryType inventoryType, ItemInstance itemInstance, int count = 1)
        {
            var inventory = Inventories[inventoryType];

            // Check for non-filled stacks first
            for (int i = 0; i < inventory.Count; i++)
            {
                var slot = inventory[i];

                if (slot.ItemInstance != null && slot.ItemInstance == itemInstance && slot.Count + count <= itemInstance.Item.MaxStackSize)
                {
                    slot.Count += count;

                    ReorganizeInventory(inventory);
                    InventoryChanged?.Invoke(inventoryType, inventory);

                    ItemPicked?.Invoke(itemInstance);

                    return true;
                }
            }

            // Check for empty slots second
            for (int i = 0; i < inventory.Count; i++)
            {
                var slot = inventory[i];

                if (slot.ItemInstance == null)
                {
                    slot.ItemInstance = itemInstance;
                    slot.Count = count;

                    ReorganizeInventory(inventory);
                    InventoryChanged?.Invoke(inventoryType, inventory);

                    ItemPicked?.Invoke(itemInstance);

                    return true;
                }
            }

            return false;
        }

        public bool RemoveFirstItemByItemId(InventoryType inventoryType, int id, int count = 1)
        {
            var inventory = Inventories[inventoryType];

            for (int i = 0; i < inventory.Count; i++)
            {
                var slot = inventory[i];

                if (slot.ItemInstance != null && slot.ItemInstance.Item.Id == id)
                {
                    return RemoveItemFromSlot(inventoryType, slot, count);
                }
            }

            return false;
        }

        public bool RemoveFirstItemByInstance(InventoryType inventoryType, ItemInstance instance, int count = 1)
        {
            var inventory = Inventories[inventoryType];

            for (int i = 0; i < inventory.Count; i++)
            {
                var slot = inventory[i];

                if (slot.ItemInstance != null && slot.ItemInstance == instance)
                {
                    return RemoveItemFromSlot(inventoryType, slot, count);
                }
            }

            return false;
        }

        private bool RemoveItemFromSlot(InventoryType inventoryType, SlotData slot, int count = 1)
        {
            var inventory = Inventories[inventoryType];
            var itemInstance = slot.ItemInstance;

            if (itemInstance != null && slot.Count >= count)
            {
                slot.Count -= count;

                if (slot.Count == 0)
                {
                    slot.ItemInstance = null;
                    ReorganizeInventory(inventory);
                }

                InventoryChanged?.Invoke(inventoryType, inventory);

                ItemRemoved?.Invoke(itemInstance);

                return true;
            }

            return false;
        }

        public bool RemoveItemFromSlot(InventoryType inventoryType, int slotId, int count = 1)
        {
            var inventory = Inventories[inventoryType];
            if (slotId < 0 || slotId >= inventory.Count) return false;

            var slot = inventory[slotId];

            if (slot.ItemInstance != null && slot.Count >= count)
            {
                slot.Count -= count;

                if (slot.Count == 0)
                {
                    slot.ItemInstance = null;
                    ReorganizeInventory(inventory);
                }

                InventoryChanged?.Invoke(inventoryType, inventory);

                return true;
            }

            return false;
        }

        public ItemInstance HasItemWithId(InventoryType inventoryType, int id)
        {
            var inventory = Inventories[inventoryType];

            foreach (var slot in inventory)
            {
                if (slot.ItemInstance?.Item?.Id == id) return slot.ItemInstance;
            }

            return null;
        }

        public ItemInstance HasItemWithType(InventoryType inventoryType, ItemType type)
        {
            var inventory = Inventories[inventoryType];

            foreach (var slot in inventory)
            {
                if (slot.ItemInstance?.Item?.Type == type) return slot.ItemInstance;
            }

            return null;
        }

        private void ReorganizeInventory(List<SlotData> inventory)
        {
            var nonEmptySlots = new List<SlotData>();

            foreach (var slot in inventory)
            {
                if (slot.ItemInstance != null) nonEmptySlots.Add(new SlotData { ItemInstance = slot.ItemInstance, Count = slot.Count });
            }

            nonEmptySlots.Sort((a, b) => a.ItemInstance.Item.Id.CompareTo(b.ItemInstance.Item.Id));

            for (int i = 0; i < inventory.Count; i++)
            {
                if (i < nonEmptySlots.Count)
                {
                    inventory[i].ItemInstance = nonEmptySlots[i].ItemInstance;
                    inventory[i].Count = nonEmptySlots[i].Count;
                }
                else
                {
                    inventory[i].ItemInstance = null;
                    inventory[i].Count = 0;
                }
            }
        }
    }
}
