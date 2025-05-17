using System;
using Game.Data;
using Game.Interactions;
using Game.Inventories;
using Game.Items;
using Game.Modifiers;
using Game.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Entities.Player
{
    public class PlayerEquipmentController : MonoBehaviour
    {
        public ItemInstance ToolInstance { get; private set; }
        public GameObject ToolGameObject { get; private set; }

        [SerializeField] private ToolItem _fist;

        private PlayerEntity _player;

        public UnityEvent<ItemInstance, ItemInstance> ToolChanged;

        private void Start()
        {
            _player = FindFirstObjectByType<PlayerEntity>();

            SheathTool();

            var toolInventoryController = FindFirstObjectByType<ToolsInventoryUIController>();
        }

        public void EquipTool(ItemInstance itemInstance)
        {
            if (itemInstance == ToolInstance) return;

            var previousToolInstanceData = ToolInstance;
            var previousToolInstance = ToolGameObject;
            var tool = itemInstance.Item as ToolItem;

            ToolGameObject = Instantiate(tool.InteractorPrefab, _player.Orbit.TargetTransform);
            ToolGameObject.transform.localPosition = Vector3.zero;
            ToolGameObject.transform.localEulerAngles = Vector3.zero;
            ToolGameObject.transform.localScale = Vector3.one;

            if (ToolGameObject.TryGetComponent(out ISpeedModifier speedModifier))
            {
                _player.Moveable.RegisterSpeedModifier(speedModifier);
            }

            if (previousToolInstance != null && previousToolInstance.TryGetComponent(out ISpeedModifier previousSpeedModifier))
            {
                _player.Moveable.RegisterSpeedModifier(previousSpeedModifier);
            }

            if (ToolGameObject.TryGetComponent(out GenericMeleeWeapon weapon))
            {
                weapon.Setup(_player.gameObject, itemInstance);
            }

            if (previousToolInstance != null)
            {
                Destroy(previousToolInstance.gameObject);
            }

            if (ToolGameObject.TryGetComponent(out IInteractionSource interactionSource))
            {
                _player.InteractionSource = interactionSource;
            }
            else
            {
                _player.InteractionSource = null;
            }

            ToolChanged.Invoke(previousToolInstanceData, itemInstance);
        }

        public void SheathTool()
        {
            EquipTool(new ItemInstance { Item = _fist, Durability = -1 });
        }
    }
}