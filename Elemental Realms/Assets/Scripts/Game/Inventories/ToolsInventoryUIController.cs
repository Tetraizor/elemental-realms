using System;
using System.Collections.Generic;
using Game.Controllers;
using Game.Controllers.UI;
using Game.Data;
using Game.Entities.Player;
using Game.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Inventories
{
    public class ToolsInventoryUIController : InventoryUIController
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        protected override InventoryType Type => InventoryType.ToolsInventory;
        protected override int GridWidth => 4;
        protected override int GridHeight => 4;

        private ItemSlot _equippedSlot;
        private PlayerEquipmentController _playerEquipmentController;

        protected override void Start()
        {
            base.Start();

            _playerEquipmentController = FindFirstObjectByType<PlayerEquipmentController>();
            _playerEquipmentController.ToolChanged.AddListener(OnToolChanged);

            SlotSelected.AddListener(OnSlotSelected);
        }

        private void OnToolChanged(ItemInstance previousItem, ItemInstance currentItem)
        {
            var itemSlot = _slots.Find(slot => slot.ItemInstance == currentItem);
            if (itemSlot != null)
            {
                EquipItemSlot(itemSlot);
            }
            else
            {
                UnequipItemSlot();
            }
        }

        private void OnSlotSelected(ItemSlot slot)
        {
            if (slot.ItemInstance != null)
            {
                _titleText.SetText(slot.ItemInstance.Item.Name);
                _descriptionText.SetText(slot.ItemInstance.Item.Description);
            }
            else
            {
                _titleText.SetText("");
                _descriptionText.SetText("");
            }
        }

        private void OnDropPressed(InputAction.CallbackContext context)
        {
            if (ActiveSlot == _equippedSlot) _playerEquipmentController.SheathTool();

            DropItem();
        }

        public override void UpdateInventorySlots(InventoryType type, List<SlotData> inventorySlotData)
        {
            ItemInstance prevEquippedItem = null;
            if (_equippedSlot != null)
            {
                prevEquippedItem = _equippedSlot.ItemInstance;
                UnequipItemSlot();
            }

            base.UpdateInventorySlots(type, inventorySlotData);

            var currentEquippedSlot = _slots.Find((slot) => slot.ItemInstance == prevEquippedItem);

            if (prevEquippedItem != null && currentEquippedSlot != null)
            {
                EquipItemSlot(currentEquippedSlot);
            }
            else
            {
                UnequipItemSlot();
            }
        }

        private void EquipItemSlot(ItemSlot slot)
        {
            if (_equippedSlot == slot)
            {
                UnequipItemSlot();
                return;
            }

            if (_equippedSlot != null) UnequipItemSlot();

            _equippedSlot = slot;
            _equippedSlot.EquipItem();
        }

        private void UnequipItemSlot()
        {
            if (_equippedSlot == null) return;

            _equippedSlot.UnequipItem();
            _equippedSlot = null;
        }

        private void OnInteractPressed(InputAction.CallbackContext context)
        {
            if (ActiveSlot.ItemInstance == null) return;

            if (ActiveSlot == _equippedSlot) _playerEquipmentController.SheathTool();
            else _playerEquipmentController.EquipTool(ActiveSlot.ItemInstance);
        }

        public override void ActivateInput()
        {
            base.ActivateInput();

            var inputController = InputController.Instance;

            inputController.Controls.Player.Drop.performed += OnDropPressed;
            inputController.Controls.Player.Interact.performed += OnInteractPressed;
        }

        public override void DeactivateInput()
        {
            base.DeactivateInput();

            var inputController = InputController.Instance;

            inputController.Controls.Player.Drop.performed -= OnDropPressed;
            inputController.Controls.Player.Interact.performed -= OnInteractPressed;
        }
    }
}