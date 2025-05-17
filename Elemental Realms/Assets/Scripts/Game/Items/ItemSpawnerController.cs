using Game.Data;
using Tetraizor.MonoSingleton;
using UnityEngine;

namespace Game.Items
{
    public class ItemSpawnerController : MonoSingleton<ItemSpawnerController>
    {
        [SerializeField] private GameObject _pickablePrefab;

        public GameObject SpawnItem(int id, Vector2 position)
        {
            if (ItemSystem.Instance.Items.Count <= id) return null;
            var item = ItemSystem.Instance.Items[id];

            var itemInstance = new ItemInstance { Item = item, Durability = 0 };

            return SpawnItem(itemInstance, position);
        }

        public GameObject SpawnItem(ItemInstance itemInstance, Vector2 position)
        {
            var prefab = itemInstance.Item.Prefab == null ? _pickablePrefab : itemInstance.Item.Prefab;

            var spawnedItem = Instantiate(prefab, position, Quaternion.identity);

            spawnedItem.GetComponent<IItemInitializable>().InitializeWithItem(itemInstance);

            return spawnedItem;
        }
    }
}