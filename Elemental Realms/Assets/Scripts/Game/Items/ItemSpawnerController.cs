using Game.Data;
using Tetraizor.MonoSingleton;
using UnityEngine;

namespace Game.Items
{
    public class ItemSpawnerController : MonoSingleton<ItemSpawnerController>
    {
        [SerializeField] private GameObject _pickablePrefab;

        public GameObject SpawnPickable(ItemInstance itemInstance, Vector2 position)
        {
            var prefab = itemInstance.Item.Prefab == null ? _pickablePrefab : itemInstance.Item.Prefab;

            var spawnedItem = Instantiate(prefab, position, Quaternion.identity);

            spawnedItem.GetComponent<IItemInitializable>().InitializeWithItem(itemInstance);

            return spawnedItem;
        }
    }
}