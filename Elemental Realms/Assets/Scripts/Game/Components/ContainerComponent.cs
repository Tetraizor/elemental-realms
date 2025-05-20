using System.Linq;
using Game.Data;
using Game.Entities.Common;
using Game.Items;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Entity))]
    public class ContainerComponent : MonoBehaviour
    {
        public LootTableItem[] LootTable;

        private void Start()
        {
            GetComponent<Entity>().Killed.AddListener(OnKilled);
        }

        private void OnKilled()
        {
            LootTable.ToList().ForEach(lootTableItem =>
            {
                float rolledChance = Random.Range(0.0f, 1.0f);

                if (lootTableItem.Chance >= rolledChance)
                {
                    for (int i = 0; i < lootTableItem.Count; i++)
                    {
                        var direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;

                        ItemSpawnerController.Instance.SpawnPickable(
                            new ItemInstance() { Item = lootTableItem.Item },
                            gameObject.transform.position + direction
                        );
                    }
                }
            });
        }
    }

    [System.Serializable]
    public class LootTableItem
    {
        public Item Item;
        public int Count = 1;
        public float Chance = .5f;
    }
}