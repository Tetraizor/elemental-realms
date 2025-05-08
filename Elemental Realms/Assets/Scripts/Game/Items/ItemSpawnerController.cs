using Game.Components;
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

            var spawnedItem = Instantiate(item.Prefab ?? _pickablePrefab, position, Quaternion.identity);
            spawnedItem.GetComponent<IItemInitializable>().InitializeWithItem(item);

            return spawnedItem.gameObject;
        }
    }
}