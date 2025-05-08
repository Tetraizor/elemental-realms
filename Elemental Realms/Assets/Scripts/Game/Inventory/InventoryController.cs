using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Enum;
using Tetraizor.MonoSingleton;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Inventory
{
    public class InventoryController : MonoSingleton<InventoryController>
    {
        private const int MATERIAL_CAPACITY = 25;
        private const int EQUIPMENT_CAPACITY = 20;
        private const int TOOLS_CAPACITY = 20;

        [HideInInspector] public Dictionary<InventoryType, List<SlotData>> Inventories { get; private set; }
        [HideInInspector] public UnityEvent<InventoryType, List<SlotData>> InventoryChanged;

        protected override void Init()
        {
            base.Init();

            Inventories = new Dictionary<InventoryType, List<SlotData>> {
                { InventoryType.MaterialInventory, new List<SlotData>(MATERIAL_CAPACITY) },
                { InventoryType.EquipmentInventory, new List<SlotData>(EQUIPMENT_CAPACITY) },
                { InventoryType.ToolsInventory, new List<SlotData>(TOOLS_CAPACITY) },
            };

            for (int i = 0; i < MATERIAL_CAPACITY; i++)
            {
                Inventories[InventoryType.MaterialInventory].Add(new SlotData());
            }

            for (int i = 0; i < EQUIPMENT_CAPACITY; i++)
            {
                Inventories[InventoryType.EquipmentInventory].Add(new SlotData());
            }

            for (int i = 0; i < TOOLS_CAPACITY; i++)
            {
                Inventories[InventoryType.ToolsInventory].Add(new SlotData());
            }
        }

        public bool AddItem(InventoryType inventoryType, Item item, int count = 1)
        {
            for (int i = 0; i < Inventories.Count; i++)
            {
                var slot = Inventories[inventoryType][i];

                if (slot.Item && slot.Item.Id == item.Id && slot.Count + count <= item.MaxStackSize)
                {
                    slot.Count = slot.Count + count;

                    InventoryChanged?.Invoke(inventoryType, Inventories[inventoryType]);

                    return true;
                }
            }

            for (int i = 0; i < Inventories.Count; i++)
            {
                var slot = Inventories[inventoryType][i];

                if (slot.Item == null)
                {
                    slot.Item = item;
                    slot.Count = count;

                    InventoryChanged?.Invoke(inventoryType, Inventories[inventoryType]);

                    return true;
                }
            }

            return false;
        }

        public bool RemoveItem(InventoryType inventoryType, Item item, int count)
        {
            for (int i = 0; i < Inventories.Count; i++)
            {
                var slot = Inventories[inventoryType][i];

                if (slot.Item && slot.Item.Id == item.Id && slot.Count >= count)
                {
                    slot.Count -= count;

                    if (slot.Count == 0)
                    {
                        slot.Item = null;
                    }

                    InventoryChanged?.Invoke(inventoryType, Inventories[inventoryType]);

                    return true;
                }
            }

            return false;
        }
    }
}
