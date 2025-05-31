using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Components;
using Game.Inventories;
using Tetraizor.Bootstrap.Base;
using Tetraizor.MonoSingleton;
using UnityEngine;

namespace Game.Items
{
    public class ItemSystem : MonoSingleton<ItemSystem>, IPersistentSystem
    {
        public List<Item> Items { get; private set; }

        public string GetName() => "ItemSystem";

        public IEnumerator LoadSystem()
        {
            List<Item> items = Resources.LoadAll<Item>("Items").ToList();
            items.Sort((item1, item2) => item1.Id - item2.Id);

            Items = items;

            yield return null;
        }
    }
}