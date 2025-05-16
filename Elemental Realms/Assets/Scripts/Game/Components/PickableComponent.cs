using Game.Interactions;
using Game.Inventories;
using DG.Tweening;
using UnityEngine;
using Game.Enum;
using Game.Items;

namespace Game.Components
{
    public class PickableComponent : MonoBehaviour, IInteractable, IItemInitializable
    {
        [SerializeField] private Item _itemData;
        private SpriteRenderer _renderer;

        public Vector2 InteractablePosition => gameObject.transform.position;

        private void Start()
        {
            if (_itemData != null)
            {
                InitializeWithItem(_itemData);
            }
        }

        public void InitializeWithItem(Item itemData)
        {
            if (itemData != null)
                _itemData = itemData;

            _renderer = GetComponentInChildren<SpriteRenderer>();

            if (_renderer != null)
                _renderer.sprite = _itemData.Sprite;

            ToggleSelection(false);
        }

        public void Pickup()
        {
            InventoryType inventoryType = InventoryType.GearInventory;

            if (_itemData.Type == ItemType.Material)
            {
                inventoryType = InventoryType.MaterialInventory;
            }
            else if (_itemData.Type == ItemType.Tool || _itemData.Type == ItemType.Spell)
            {
                inventoryType = InventoryType.ToolsInventory;
            }

            if (InventoryController.Instance.AddItem(inventoryType, _itemData))
            {
                _renderer.transform.parent = null;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(_renderer.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack));
                sequence.Append(_renderer.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack));

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
