using System;
using Game.Controllers;
using Game.Controllers.UI;
using Game.Entities.Player;
using Game.Items;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Inventories
{
    public class MaterialInventoryUIController : InventoryUIController
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

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
            if (ActiveSlot.Item != null)
            {
                int slotIndex = _slots.FindIndex(slot => slot == ActiveSlot);
                int itemId = ActiveSlot.Item.Id;

                if (InventoryController.Instance.RemoveItemFromSlot(Type, slotIndex))
                {
                    Vector2 spawnPosition = FindFirstObjectByType<PlayerEntity>().transform.position +
                        (Vector3)(new Vector2(
                            UnityEngine.Random.Range(-1, 1),
                            UnityEngine.Random.Range(-1, 1)).normalized * 2);

                    ItemSpawnerController.Instance.SpawnItem(
                        itemId,
                        spawnPosition
                    );
                }
            }
        }

        public override void ActivateInput()
        {
            base.ActivateInput();

            var inputController = InputController.Instance;

            inputController.Controls.Player.Drop.performed += OnDropPressed;
        }

        public override void DeactivateInput()
        {
            base.DeactivateInput();

            var inputController = InputController.Instance;

            inputController.Controls.Player.Drop.performed -= OnDropPressed;
        }
    }
}