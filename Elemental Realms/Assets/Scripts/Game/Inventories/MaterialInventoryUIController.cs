using DG.Tweening;
using Game.Controllers;
using Game.Controllers.UI;
using Game.Items;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Inventories
{
    public class MaterialInventoryUIController : InventoryUIController
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private RectTransform _consumablePrompt;

        [SerializeField] private RectTransform _promptPrefab;
        [SerializeField] private RectTransform _effectContainer;

        protected override void Start()
        {
            base.Start();

            SlotSelected.AddListener(OnSlotSelected);
        }

        private void OnSlotSelected(ItemSlot slot)
        {
            var elements = _effectContainer.GetComponentsInChildren<PromptElement>();
            foreach (var element in elements)
            {
                Destroy(element.gameObject);
            }

            if (slot.ItemInstance != null)
            {
                _titleText.SetText(slot.ItemInstance.Item.Name);
                _descriptionText.SetText(slot.ItemInstance.Item.Description);

                if (slot.ItemInstance.Item is IItemConsumable consumable)
                {
                    var consumableEffects = consumable.Consume();
                    _consumablePrompt.gameObject.SetActive(true);

                    foreach (var consumableEffect in consumableEffects)
                    {
                        var prompt = Instantiate(_promptPrefab, _effectContainer);
                        prompt.GetComponent<PromptElement>().Setup($"{consumableEffect.Type.Name} ({consumableEffect.Magnitude})", consumableEffect.Type.Icon);
                    }
                }
                else
                {
                    _consumablePrompt.gameObject.SetActive(false);
                }
            }
            else
            {
                _titleText.SetText("");
                _descriptionText.SetText("");

                _consumablePrompt.gameObject.SetActive(false);
            }
        }

        private void OnDropPressed(InputAction.CallbackContext context)
        {
            DropItem();
        }

        private void OnInteractPressed(InputAction.CallbackContext context)
        {
            if (ActiveSlot == null || ActiveSlot.ItemInstance == null) return;

            var itemInstance = ActiveSlot.ItemInstance;

            if (itemInstance.Item is IItemConsumable consumable)
            {
                ActiveSlot.transform.DOScale(Vector3.one * 1.1f, .07f).OnComplete(() =>
                {
                    ActiveSlot.transform.DOScale(Vector3.one, .5f);
                });

                int slotIndex = _slots.FindIndex(slot => slot == ActiveSlot);
                int itemId = ActiveSlot.ItemInstance.Item.Id;

                if (InventoryController.Instance.RemoveItemFromSlot(Type, slotIndex))
                {
                    var effects = consumable.Consume();
                }

                SelectSlot(ActiveSlot);
            }
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