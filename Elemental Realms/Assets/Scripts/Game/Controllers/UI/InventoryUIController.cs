using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Input;
using Game.Inventory;
using Tetraizor.MonoSingleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Controllers.UI
{
    public class InventoryUIController : MonoSingleton<InventoryUIController>, IInputConsumer
    {
        public UnityEvent<ItemSlot> SlotSelected;

        public bool IsOn { get; private set; }

        [HideInInspector] public UnityEvent<bool> StateChanged;

        private InventoryController _inventory;
        private List<ItemSlot> _slots = new();

        private ItemSlot _selectedSlot = null;

        [SerializeField] private RectTransform _slotContainer;
        [SerializeField] private GameObject _slotPrefab;

        private const int GRID_WIDTH = 5;
        private const int GRID_HEIGHT = 5;

        protected override void Init()
        {
            _inventory = GetComponent<InventoryController>();
        }

        private void Start()
        {
            for (int i = 0; i < _inventory.Slots.Capacity; i++)
            {
                var slot = Instantiate(_slotPrefab, _slotContainer).GetComponent<ItemSlot>();
                _slots.Add(slot);
            }

            SelectSlot(_slots.First());

            _inventory.InventoryChanged.AddListener(UpdateInventorySlots);
            UpdateInventorySlots(_inventory.Slots);
        }

        public void UpdateInventorySlots(List<SlotData> inventorySlotData)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var slotData = inventorySlotData[i];

                slot.SetItem(slotData);
            }
        }

        public void Toggle()
        {
            Toggle(!IsOn);
        }

        public void SelectSlot(ItemSlot slot)
        {
            if (_selectedSlot != null)
                _selectedSlot.Deselect();

            _selectedSlot = slot;
            _selectedSlot.Select();

            SlotSelected?.Invoke(_selectedSlot);
        }

        public void Toggle(bool state)
        {
            if (state == IsOn) return;
            IsOn = state;

            SelectSlot(_slots.First());

            if (IsOn)
            {
                GetComponent<Animator>().SetTrigger("In");
                FindFirstObjectByType<InputController>().ActivateConsumer(this);
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Out");
                FindFirstObjectByType<InputController>().DeactivateConsumer(this);
            }

            StateChanged.Invoke(IsOn);
        }

        public void Activate()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed += OnInventoryButtonPressed;

            inputController.Controls.Player.Up.performed += OnUpButtonPressed;
            inputController.Controls.Player.Down.performed += OnDownButtonPressed;
            inputController.Controls.Player.Left.performed += OnLeftButtonPressed;
            inputController.Controls.Player.Right.performed += OnRightButtonPressed;
        }

        public void Deactivate()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed -= OnInventoryButtonPressed;

            inputController.Controls.Player.Up.performed -= OnUpButtonPressed;
            inputController.Controls.Player.Down.performed -= OnDownButtonPressed;
            inputController.Controls.Player.Left.performed -= OnLeftButtonPressed;
            inputController.Controls.Player.Right.performed -= OnRightButtonPressed;
        }

        private void OnInventoryButtonPressed(InputAction.CallbackContext context)
        {
            Toggle(false);
        }

        private void OnUpButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(_selectedSlot);
            int newIndex = (currentIndex - GRID_WIDTH + _slots.Count) % _slots.Count;
            SelectSlot(_slots[newIndex]);
        }

        private void OnDownButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(_selectedSlot);
            int newIndex = (currentIndex + GRID_WIDTH) % _slots.Count;
            SelectSlot(_slots[newIndex]);
        }

        private void OnLeftButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(_selectedSlot);
            int rowStart = currentIndex - (currentIndex % GRID_WIDTH);
            int newIndex = (currentIndex == rowStart) ? rowStart + GRID_WIDTH - 1 : currentIndex - 1;
            SelectSlot(_slots[newIndex]);
        }

        private void OnRightButtonPressed(InputAction.CallbackContext context)
        {
            int currentIndex = _slots.IndexOf(_selectedSlot);
            int rowEnd = currentIndex - (currentIndex % GRID_WIDTH) + GRID_WIDTH - 1;
            int newIndex = (currentIndex == rowEnd) ? rowEnd - GRID_WIDTH + 1 : currentIndex + 1;
            SelectSlot(_slots[newIndex]);
        }

    }
}