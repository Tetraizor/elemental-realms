using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Tetraizor.MonoSingleton;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Inventory
{
    public class InventoryController : MonoSingleton<InventoryController>
    {
        [HideInInspector] public List<SlotData> Slots { get; private set; }
        [HideInInspector] public UnityEvent<List<SlotData>> InventoryChanged;

        protected override void Init()
        {
            base.Init();

            Slots = new List<SlotData>(25);
            for (int i = 0; i < 25; i++)
            {
                Slots.Add(new SlotData());
            }
        }

        public bool AddItem(Item item, int count = 1)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                var slot = Slots[i];

                if (slot.Item && slot.Item.Id == item.Id && slot.Count + count <= item.MaxStackSize)
                {
                    slot.Count = slot.Count + count;

                    InventoryChanged?.Invoke(Slots);

                    return true;
                }
            }

            for (int i = 0; i < Slots.Count; i++)
            {
                var slot = Slots[i];

                if (slot.Item == null)
                {
                    slot.Item = item;
                    slot.Count = count;

                    InventoryChanged?.Invoke(Slots);

                    return true;
                }
            }

            return false;
        }

        public bool RemoveItem(Item item, int count)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                var slot = Slots[i];

                if (slot.Item && slot.Item.Id == item.Id && slot.Count >= count)
                {
                    slot.Count -= count;

                    if (slot.Count == 0)
                    {
                        slot.Item = null;
                    }

                    InventoryChanged?.Invoke(Slots);

                    return true;
                }
            }

            return false;
        }
    }
}
