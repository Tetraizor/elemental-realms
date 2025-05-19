using System;
using Game.Components;
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

            InventoryController.Instance.ItemPicked.AddListener(OnItemPicked);
        }

        private void OnItemPicked(ItemInstance itemInstance)
        {
            if ((ToolInstance == null || ToolInstance.Item.Id == _fist.Id) && itemInstance.Item is ToolItem)
            {
                EquipTool(itemInstance);
            }
        }

        public void EquipTool(ItemInstance itemInstance)
        {
            if (itemInstance == ToolInstance) return;

            var previousToolInstanceData = ToolInstance;
            var previousToolInstance = ToolGameObject;
            var tool = itemInstance.Item as ToolItem;

            ToolInstance = itemInstance;
            ToolGameObject = Instantiate(tool.InteractorPrefab, _player.Orbit.TargetTransform);
            ToolGameObject.transform.localPosition = Vector3.zero;
            ToolGameObject.transform.localEulerAngles = Vector3.zero;
            ToolGameObject.transform.localScale = Vector3.one;

            // Clean previous item related stuff
            if (ToolGameObject.TryGetComponent(out ISpeedModifier speedModifier))
            {
                _player.Moveable.RegisterSpeedModifier(speedModifier);
            }

            if (previousToolInstance != null && previousToolInstance.TryGetComponent(out ISpeedModifier previousSpeedModifier))
            {
                _player.Moveable.DeregisterSpeedModifier(previousSpeedModifier);
            }

            // Assign new item related stuff
            if (ToolGameObject.TryGetComponent(out IToolInitializable initializable))
            {
                initializable.Setup(_player.gameObject, itemInstance);
            }

            if (ToolGameObject.TryGetComponent(out IItemThrowable newThrowable))
            {
                newThrowable.GetThrownEvent().AddListener(OnItemThrown);
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

        public void OnItemThrown(ItemInstance itemInstance)
        {
            SheathTool();

            if (InventoryController.Instance.RemoveFirstItemByInstance(Enum.InventoryType.ToolsInventory, itemInstance))
            {
                var direction = _player.Moveable.LookDirection;
                var position = _player.transform.position + (Vector3)(direction * 1.5f);

                var spawnedObject = ItemSpawnerController.Instance.SpawnPickable(itemInstance, position);

                if (spawnedObject.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce(direction * 20 * rb.mass, ForceMode2D.Impulse);
                    rb.AddTorque(300);
                }

                if (spawnedObject.TryGetComponent(out PickableComponent pickable))
                {
                    pickable.InitializeWithItem(itemInstance);
                }
            }
        }
    }
}