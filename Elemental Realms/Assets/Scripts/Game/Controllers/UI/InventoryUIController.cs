using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Entities.Player;
using Game.Enum;
using Game.Inventories;
using Game.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Controllers.UI
{
    public class InventoryUIController : MonoBehaviour
    {
        public UnityEvent<ItemSlot> SlotSelected;

        private InventoryController _inventory;
        protected List<ItemSlot> _slots = new();

        public ItemSlot ActiveSlot { get; protected set; } = null;

        [SerializeField] private RectTransform _slotContainer;
        [SerializeField] private GameObject _slotPrefab;

        protected virtual InventoryType Type => InventoryType.MaterialInventory;
        protected virtual int GridWidth => 5;
        protected virtual int GridHeight => 5;

        protected virtual void Start()
        {
            _inventory = InventoryController.Instance;

            for (int i = 0; i < _inventory.Inventories[Type].Capacity; i++)
            {
                var slot = Instantiate(_slotPrefab, _slotContainer).GetComponent<ItemSlot>();
                slot.Setup(this);

                _slots.Add(slot);
            }

            SelectSlot(_slots.First());

            _inventory.InventoryChanged.AddListener(UpdateInventorySlots);
            UpdateInventorySlots(Type, _inventory.Inventories[Type]);
        }

        public void UpdateInventorySlots(InventoryType type, List<SlotData> inventorySlotData)
        {
            if (type != Type) return;

            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var slotData = inventorySlotData[i];

                slot.SetItem(slotData);
            }
        }

        public void SelectSlot(ItemSlot slot)
        {
            if (ActiveSlot != null)
                ActiveSlot.Deselect();

            ActiveSlot = slot;
            ActiveSlot.Select();

            SlotSelected?.Invoke(ActiveSlot);
        }

        protected virtual void DropItem()
        {
            if (ActiveSlot.Item != null)
            {
                int slotIndex = _slots.FindIndex(slot => slot == ActiveSlot);
                int itemId = ActiveSlot.Item.Id;

                if (InventoryController.Instance.RemoveItemFromSlot(Type, slotIndex))
                {
                    Vector2 spawnDirection = new Vector2(
                            UnityEngine.Random.Range(-1, 1),
                            UnityEngine.Random.Range(-1, 1)).normalized * 2;

                    Vector2 spawnPosition = FindFirstObjectByType<PlayerEntity>().transform.position +
                        (Vector3)spawnDirection;

                    var spawnedObject = ItemSpawnerController.Instance.SpawnItem(
                        itemId,
                        spawnPosition
                    );

                    if (spawnedObject.TryGetComponent(out Rigidbody2D rb))
                    {
                        rb.AddForce(spawnDirection * 4 * rb.mass, ForceMode2D.Impulse);
                    }
                }
            }
        }

        private void OnUpButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int newIndex = (currentIndex - GridWidth + _slots.Count) % _slots.Count;
            SelectSlot(_slots[newIndex]);
        }

        private void OnDownButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int newIndex = (currentIndex + GridWidth) % _slots.Count;
            SelectSlot(_slots[newIndex]);
        }

        private void OnLeftButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int rowStart = currentIndex - (currentIndex % GridWidth);
            int newIndex = (currentIndex == rowStart) ? rowStart + GridWidth - 1 : currentIndex - 1;
            SelectSlot(_slots[newIndex]);
        }

        private void OnRightButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int rowEnd = currentIndex - (currentIndex % GridWidth) + GridWidth - 1;
            int newIndex = (currentIndex == rowEnd) ? rowEnd - GridWidth + 1 : currentIndex + 1;
            SelectSlot(_slots[newIndex]);
        }

        public virtual void ActivateInput()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Up.performed += OnUpButtonPressed;
            inputController.Controls.Player.Down.performed += OnDownButtonPressed;
            inputController.Controls.Player.Left.performed += OnLeftButtonPressed;
            inputController.Controls.Player.Right.performed += OnRightButtonPressed;
        }

        public virtual void DeactivateInput()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Up.performed -= OnUpButtonPressed;
            inputController.Controls.Player.Down.performed -= OnDownButtonPressed;
            inputController.Controls.Player.Left.performed -= OnLeftButtonPressed;
            inputController.Controls.Player.Right.performed -= OnRightButtonPressed;
        }
    }
}