using Game.Controllers;
using Game.Controllers.UI;
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

        protected override void Start()
        {
            base.Start();

            SlotSelected.AddListener(OnSlotSelected);
        }

        private void OnSlotSelected(ItemSlot slot)
        {
            if (slot.Item != null)
            {
                _titleText.SetText(slot.Item.Name);
                _descriptionText.SetText(slot.Item.Description);
            }
            else
            {
                _titleText.SetText("");
                _descriptionText.SetText("");
            }
        }

        private void OnDropPressed(InputAction.CallbackContext context)
        {
            DropItem();
        }

        private void OnInteractPressed(InputAction.CallbackContext context)
        {
            // TODO: Complete here.
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