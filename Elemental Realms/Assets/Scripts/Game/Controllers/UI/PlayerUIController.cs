using System.Linq;
using DG.Tweening;
using Game.Enum;
using Game.Input;
using Tetraizor.MonoSingleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Controllers.UI
{
    public class PlayerUIController : MonoSingleton<PlayerUIController>, IInputConsumer
    {
        public bool IsOn { get; private set; }
        [HideInInspector] public UnityEvent<bool> StateChanged;

        private readonly InventoryType[] _inventoryCycle = new[]
        {
            InventoryType.MaterialInventory,
            InventoryType.ToolsInventory,
            InventoryType.GearInventory
        };

        [Header("UI References")]
        [SerializeField] private RectTransform _materialsRoot;
        [SerializeField] private RectTransform _toolsRoot;
        [SerializeField] private RectTransform _gearRoot;

        [SerializeField] private Button _materialsButton;
        [SerializeField] private Button _toolsButton;
        [SerializeField] private Button _gearButton;

        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;

        [SerializeField] private Sprite _activeLabelSprite;
        [SerializeField] private Sprite _inactiveLabelSprite;

        private const float INVENTORY_SLIDE_TIME = .5f;
        private const float INVENTORY_BUTTON_ANIMATION_TIME = .1f;

        private bool _isAnimationPlaying = false;

        private InventoryType _activeInventoryType = InventoryType.MaterialInventory;

        private void Start()
        {
            _materialsButton.onClick.AddListener(() => ChangeInventory(InventoryType.MaterialInventory));
            _toolsButton.onClick.AddListener(() => ChangeInventory(InventoryType.ToolsInventory));
            _gearButton.onClick.AddListener(() => ChangeInventory(InventoryType.GearInventory));

            _previousButton.onClick.AddListener(SelectPreviousTab);
            _nextButton.onClick.AddListener(SelectNextTab);

            _activeInventoryType = InventoryType.ToolsInventory;
            ChangeInventory(InventoryType.MaterialInventory);
        }

        public void ChangeInventory(InventoryType inventoryType)
        {
            var previousInventoryType = _activeInventoryType;
            if (_activeInventoryType == inventoryType || _isAnimationPlaying) return;

            _activeInventoryType = inventoryType;

            (RectTransform, Button) GetReferencesFrom(InventoryType type)
            {
                return type switch
                {
                    InventoryType.MaterialInventory => (_materialsRoot, _materialsButton),
                    InventoryType.ToolsInventory => (_toolsRoot, _toolsButton),
                    InventoryType.GearInventory => (_gearRoot, _gearButton),
                    _ => (null, null)
                };
            }

            (RectTransform previousRoot, Button previousButton) = GetReferencesFrom(previousInventoryType);
            (RectTransform currentRoot, Button currentButton) = GetReferencesFrom(_activeInventoryType);

            previousRoot.GetComponent<InventoryUIController>().DeactivateInput();
            currentRoot.GetComponent<InventoryUIController>().ActivateInput();

            // Animate buttons
            previousButton.GetComponent<Image>().sprite = _inactiveLabelSprite;
            previousButton.transform.DOLocalMoveY(-4.0f, .1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                previousButton.transform.DOLocalMoveY(0, .1f).SetEase(Ease.OutCubic);
            });

            currentButton.GetComponent<Image>().sprite = _activeLabelSprite;
            currentButton.transform.DOLocalMoveY(12.0f, .1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                currentButton.transform.DOLocalMoveY(8, .1f).SetEase(Ease.OutCubic);
            });

            // Handle wrap-around left/right
            int count = _inventoryCycle.Length;
            int prevIndex = System.Array.IndexOf(_inventoryCycle, previousInventoryType);
            int currIndex = System.Array.IndexOf(_inventoryCycle, _activeInventoryType);

            // Normalize the distance
            int diff = (currIndex - prevIndex + count) % count;

            bool goingRight = diff > 0 && diff <= count / 2;
            bool goingLeft = diff > count / 2;

            Vector2 offscreenLeft = new Vector2(-1200f, 0f);
            Vector2 offscreenRight = new Vector2(1200f, 0f);
            Vector2 center = Vector2.zero;

            _isAnimationPlaying = true;

            if (goingLeft)
            {
                currentRoot.anchoredPosition = offscreenLeft;
                currentRoot.DOAnchorPos(center, INVENTORY_SLIDE_TIME).SetEase(Ease.OutCubic);
                previousRoot.DOAnchorPos(offscreenRight, INVENTORY_SLIDE_TIME).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    _isAnimationPlaying = false;
                });
            }
            else if (goingRight)
            {
                currentRoot.anchoredPosition = offscreenRight;
                currentRoot.DOAnchorPos(center, INVENTORY_SLIDE_TIME).SetEase(Ease.OutCubic);
                previousRoot.DOAnchorPos(offscreenLeft, INVENTORY_SLIDE_TIME).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    _isAnimationPlaying = false;
                });
            }
        }

        public void SelectNextTab()
        {
            _nextButton.transform.DOKill();
            _nextButton.transform
                .DOScale(new Vector3(-1, 1, 1) * 1.1f, INVENTORY_BUTTON_ANIMATION_TIME)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _nextButton.transform
                        .DOScale(new Vector3(-1, 1, 1), INVENTORY_BUTTON_ANIMATION_TIME)
                        .SetEase(Ease.OutCubic);
                });

            int currentIndex = System.Array.IndexOf(_inventoryCycle, _activeInventoryType);
            int nextIndex = (currentIndex + 1) % _inventoryCycle.Length;
            ChangeInventory(_inventoryCycle[nextIndex]);
        }

        public void SelectPreviousTab()
        {
            _previousButton.transform.DOKill();
            _previousButton.transform
                .DOScale(Vector3.one * 1.1f, INVENTORY_BUTTON_ANIMATION_TIME)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _previousButton.transform
                        .DOScale(Vector3.one, INVENTORY_BUTTON_ANIMATION_TIME)
                        .SetEase(Ease.OutCubic);
                });

            int currentIndex = System.Array.IndexOf(_inventoryCycle, _activeInventoryType);
            int prevIndex = (currentIndex - 1 + _inventoryCycle.Length) % _inventoryCycle.Length;
            ChangeInventory(_inventoryCycle[prevIndex]);
        }

        public void Toggle()
        {
            Toggle(!IsOn);
        }

        public void Toggle(bool state)
        {
            if (state == IsOn) return;
            IsOn = state;

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

        #region Input

        private void OnSelectLeftButtonPressed(InputAction.CallbackContext _) => SelectPreviousTab();
        private void OnSelectRightButtonPressed(InputAction.CallbackContext _) => SelectNextTab();
        private void OnInventoryButtonPressed(InputAction.CallbackContext _) => Toggle(false);

        public void ActivateInput()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed += OnInventoryButtonPressed;

            inputController.Controls.Player.SelectLeft.performed += OnSelectLeftButtonPressed;
            inputController.Controls.Player.SelectRight.performed += OnSelectRightButtonPressed;

            var inventoryControllers = GetComponentsInChildren<InventoryUIController>().ToList();

            switch (_activeInventoryType)
            {
                case InventoryType.MaterialInventory:
                    _materialsRoot.GetComponent<InventoryUIController>().ActivateInput();
                    break;
                case InventoryType.ToolsInventory:
                    _toolsRoot.GetComponent<InventoryUIController>().ActivateInput();
                    break;
                case InventoryType.GearInventory:
                    _gearRoot.GetComponent<InventoryUIController>().ActivateInput();
                    break;
            }
        }

        public void DeactivateInput()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed -= OnInventoryButtonPressed;

            inputController.Controls.Player.SelectLeft.performed -= OnSelectLeftButtonPressed;
            inputController.Controls.Player.SelectRight.performed -= OnSelectRightButtonPressed;

            var inventoryControllers = GetComponentsInChildren<InventoryUIController>().ToList();
            inventoryControllers.ForEach(inventory => inventory.DeactivateInput());
        }

        #endregion
    }
}