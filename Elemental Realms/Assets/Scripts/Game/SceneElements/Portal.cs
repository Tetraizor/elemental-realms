using Game.Entities.Common;
using Game.Enum;
using Game.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Game.SceneElements
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private EntityTag _targetTags;
        [HideInInspector] public UnityEvent Entered;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Entity entity))
            {
                if (entity.Tags.HasCommon(_targetTags))
                {
                    Entered?.Invoke();
                }
            }
        }
    }
}