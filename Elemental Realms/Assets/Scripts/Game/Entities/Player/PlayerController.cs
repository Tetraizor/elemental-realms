using Game.Controllers;
using Game.Controllers.UI;
using Game.Enum;
using Game.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entities.Player
{
    public class PlayerController : MonoBehaviour, IInputConsumer
    {
        [SerializeField] private PlayerEntity _player;

        private Vector2 _moveDirectionInput = Vector2.zero;
        private Vector2 _lookDirectionInput = Vector2.zero;

        public Vector2 CursorPosition { get; private set; } = Vector2.right;
        public Vector2 CursorPositionNormalized { get; private set; } = Vector2.right;

        private InputController _inputController;

        protected void Start()
        {
            _inputController = FindFirstObjectByType<InputController>();
            _inputController.ActivateConsumer(this);
        }

        private void Update()
        {
            switch (_inputController.ActiveInputType)
            {
                case InputType.KeyboardMouse:
                    CursorPositionNormalized = (new Vector2(UnityEngine.Input.mousePosition.x / Screen.width, UnityEngine.Input.mousePosition.y / Screen.height) * 2) - Vector2.one;
                    CursorPosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

                    _player.Moveable.MovementDirection = _moveDirectionInput.normalized;
                    _player.SetLookDirection(CursorPositionNormalized.normalized);
                    break;

                case InputType.Gamepad:
                    CursorPositionNormalized = (
                        _lookDirectionInput.magnitude > 0.1f ? _lookDirectionInput :
                        (_moveDirectionInput.magnitude > 0.1f ? _moveDirectionInput :
                        CursorPositionNormalized)
                    ).normalized;

                    CursorPosition = _player.transform.position + (Vector3)(CursorPositionNormalized * 5);

                    _player.Moveable.MovementDirection = _moveDirectionInput.normalized;
                    _player.SetLookDirection(CursorPositionNormalized.normalized);
                    break;
            }
        }

        public void ActivateInput()
        {
            var controls = _inputController.Controls.Player;

            controls.Move.performed += OnMovePerformed;
            controls.Move.canceled += OnMoveCanceled;

            controls.Look.performed += OnLookPerformed;
            controls.Look.canceled += OnLookCanceled;

            controls.Attack.performed += OnAttackPerformed;
            controls.Attack.canceled += OnAttackCanceled;

            controls.Dash.performed += OnDashPerformed;

            controls.Interact.performed += OnInteractPerformed;

            controls.Inventory.performed += OnInventoryPerformed;
        }

        public void DeactivateInput()
        {
            var controls = _inputController.Controls.Player;

            controls.Move.performed -= OnMovePerformed;
            controls.Move.canceled -= OnMoveCanceled;

            controls.Look.performed -= OnLookPerformed;
            controls.Look.canceled -= OnLookCanceled;

            controls.Attack.performed -= OnAttackPerformed;
            controls.Attack.canceled -= OnAttackCanceled;

            controls.Dash.performed -= OnDashPerformed;

            controls.Interact.performed -= OnInteractPerformed;

            controls.Inventory.performed -= OnInventoryPerformed;

            _moveDirectionInput = Vector2.zero;
            _player.InteractionSource?.Deactivate();
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            _moveDirectionInput = ctx.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            _moveDirectionInput = Vector2.zero;
        }

        private void OnLookPerformed(InputAction.CallbackContext ctx)
        {
            _lookDirectionInput = ctx.ReadValue<Vector2>();
        }

        private void OnLookCanceled(InputAction.CallbackContext ctx)
        {
            _lookDirectionInput = Vector2.zero;
        }

        private void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            _player.InteractionSource?.Activate();
        }

        private void OnAttackCanceled(InputAction.CallbackContext ctx)
        {
            _player.InteractionSource?.Deactivate();
        }

        private void OnDashPerformed(InputAction.CallbackContext ctx)
        {
            _player.Dash();
        }

        private void OnInteractPerformed(InputAction.CallbackContext ctx)
        {
            _player.ItemPicker.Interact();
        }

        private void OnInventoryPerformed(InputAction.CallbackContext context)
        {
            var playerUiController = FindFirstObjectByType<PlayerUIController>();
            playerUiController.Toggle(true);
            playerUiController.StateChanged.AddListener(OnInventoryStateChanged);

            _inputController.DeactivateConsumer(this);
        }

        private void OnInventoryStateChanged(bool state)
        {
            if (!state)
            {
                var playerUiController = FindFirstObjectByType<PlayerUIController>();
                playerUiController.StateChanged.RemoveListener(OnInventoryStateChanged);

                _inputController.ActivateConsumer(this);
            }
        }
    }
}
