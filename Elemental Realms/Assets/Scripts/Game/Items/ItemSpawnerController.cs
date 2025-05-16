using System.Collections;
using System.Threading.Tasks;
using Game.Components;
using Tetraizor.MonoSingleton;
using UnityEngine;

namespace Game.Items
{
    public class ItemSpawnerController : MonoSingleton<ItemSpawnerController>
    {
        [SerializeField] private GameObject _pickablePrefab;

        public void SpawnItem(int id, Vector2 position)
        {
            _ = SpawnItemAsync(id, position);
        }

        public async Task<GameObject> SpawnItemAsync(int id, Vector2 position)
        {
            if (ItemSystem.Instance.Items.Count <= id) return null;
            var item = ItemSystem.Instance.Items[id];
            var prefab = item.Prefab == null ? _pickablePrefab : item.Prefab;

            var spawnedItem = (await InstantiateAsync(prefab, position, Quaternion.identity))[0];

            spawnedItem.GetComponent<IItemInitializable>().InitializeWithItem(item);

            return spawnedItem;
        }
    }
}