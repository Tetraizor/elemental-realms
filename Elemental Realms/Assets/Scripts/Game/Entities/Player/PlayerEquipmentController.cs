using Game.Data;
using Game.Interactions;
using Game.Items;
using Game.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Entities.Player
{
    public class PlayerEquipmentController : MonoBehaviour
    {
        public ToolItemInstance ToolInstanceData { get; private set; }
        public GameObject ToolInstance { get; private set; }

        [SerializeField] private ToolItem _fist;

        private PlayerEntity _player;

        public UnityEvent<ToolItemInstance, ToolItemInstance> ToolChanged;

        private void Start()
        {
            _player = FindFirstObjectByType<PlayerEntity>();

            EquipTool(new ToolItemInstance { Data = _fist, Durability = -1 });
        }

        public void EquipTool(ToolItemInstance toolInstance)
        {
            var previousToolInstanceData = ToolInstanceData;
            var previousToolInstance = ToolInstance;
            var tool = toolInstance.Data;

            ToolInstance = Instantiate(tool.Prefab, _player.EntityRenderer.transform.position, Quaternion.identity);
            ToolInstance.transform.parent = _player.Orbit.TargetTransform;

            if (ToolInstance.TryGetComponent(out GenericMeleeWeapon weapon))
            {
                weapon.Setup(_player.gameObject);
            }

            if (previousToolInstance != null)
            {
                Destroy(previousToolInstance.gameObject);
            }

            if (ToolInstance.TryGetComponent(out IInteractionSource interactionSource))
            {
                _player.InteractionSource = interactionSource;
            }
            else
            {
                _player.InteractionSource = null;
            }

            ToolChanged.Invoke(previousToolInstanceData, toolInstance);
        }

        public void SheathTool()
        {
            EquipTool(new ToolItemInstance { Data = _fist, Durability = -1 });
        }
    }
}