using Game.Interactions;
using Game.Inventories;
using DG.Tweening;
using UnityEngine;
using Game.Enum;
using Game.Items;
using Game.Data;
using Game.Entities.Player;

namespace Game.Components
{
    public class PickableComponent : MonoBehaviour, IInteractable, IItemInitializable
    {
        [SerializeField] protected ItemInstance _itemInstance;
        [HideInInspector] public ItemInstance ItemInstance => _itemInstance;
        private SpriteRenderer _renderer;

        public Vector2 InteractablePosition => gameObject.transform.position;

        protected virtual void Start()
        {
            if (_itemInstance != null)
            {
                InitializeWithItem(_itemInstance);
            }
        }

        public virtual void InitializeWithItem(ItemInstance itemInstance)
        {
            if (itemInstance != null)
                _itemInstance = itemInstance;

            _renderer = GetComponentInChildren<SpriteRenderer>();

            if (_renderer != null)
                _renderer.sprite = _itemInstance.Item.Sprite;

            ToggleSelection(false);
        }

        public void Pickup()
        {
            InventoryType inventoryType = InventoryType.GearInventory;

            if (_itemInstance.Item.Type == ItemType.Material)
            {
                inventoryType = InventoryType.MaterialInventory;
            }
            else if (_itemInstance.Item.Type == ItemType.Tool || _itemInstance.Item.Type == ItemType.Spell)
            {
                inventoryType = InventoryType.ToolsInventory;
            }

            if (InventoryController.Instance.AddItem(inventoryType, _itemInstance))
            {
                _renderer.transform.parent = null;
                Destroy(_renderer.gameObject, 1);

                var player = FindFirstObjectByType<PlayerEntity>();
                if (player != null)
                {
                    _renderer.transform.DOMove(player.transform.position, .2f);
                }
                _renderer.transform.DOScale(0, 0.2f);

                Destroy(gameObject);
            }
        }

        public void ToggleSelection(bool state)
        {
            if (!Application.isPlaying)
                return;

            if (state)
            {
                var material = _renderer.material;
                material.SetFloat("_ShowOutline", 1);
            }
            else
            {
                var material = _renderer.material;
                material.SetFloat("_ShowOutline", 0);
            }
        }

        public void Interact()
        {
            Pickup();
        }

        public void Activate()
        {
            ToggleSelection(true);
        }

        public void Deactivate()
        {
            ToggleSelection(false);
        }
    }
}
