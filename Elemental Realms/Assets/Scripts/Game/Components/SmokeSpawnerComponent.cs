using System.Linq;
using Game.Data;
using Game.Entities.Common;
using Game.Items;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Entity))]
    public class SmokeSpawnerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _smokePrefab;
        private void Start()
        {
            GetComponent<Entity>().Killed.AddListener(OnKilled);
        }

        private void OnKilled()
        {
            var smoke = Instantiate(_smokePrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(smoke.gameObject, 5);
        }
    }
}