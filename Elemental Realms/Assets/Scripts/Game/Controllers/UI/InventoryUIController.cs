using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Enum;
using Game.Inventories;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Controllers.UI
{
    public class InventoryUIController : MonoBehaviour
    {
        public UnityEvent<ItemSlot> SlotSelected;

        private InventoryController _inventory;
        private List<ItemSlot> _slots = new();

        public ItemSlot ActiveSlot { get; protected set; } = null;

        [SerializeField] protected InventoryType Type = InventoryType.MaterialInventory;

        [SerializeField] private RectTransform _slotContainer;
        [SerializeField] private GameObject _slotPrefab;

        [SerializeField] private const int GRID_WIDTH = 5;
        [SerializeField] private const int GRID_HEIGHT = 5;

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

        private void OnUpButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int newIndex = (currentIndex - GRID_WIDTH + _slots.Count) % _slots.Count;
            SelectSlot(_slots[newIndex]);
        }

        private void OnDownButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int newIndex = (currentIndex + GRID_WIDTH) % _slots.Count;
            SelectSlot(_slots[newIndex]);
        }

        private void OnLeftButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int rowStart = currentIndex - (currentIndex % GRID_WIDTH);
            int newIndex = (currentIndex == rowStart) ? rowStart + GRID_WIDTH - 1 : currentIndex - 1;
            SelectSlot(_slots[newIndex]);
        }

        private void OnRightButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(ActiveSlot);
            int rowEnd = currentIndex - (currentIndex % GRID_WIDTH) + GRID_WIDTH - 1;
            int newIndex = (currentIndex == rowEnd) ? rowEnd - GRID_WIDTH + 1 : currentIndex + 1;
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