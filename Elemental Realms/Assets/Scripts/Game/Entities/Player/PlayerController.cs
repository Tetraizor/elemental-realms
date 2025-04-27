using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Game.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        private MainControls _controls;
        private PlayerInput _input;

        [SerializeField] private PlayerEntity _player;

        private Vector2 _moveDirectionInput = Vector2.zero;
        private Vector2 _lookDirectionInput = Vector2.zero;

        public Vector2 CursorPosition { get; private set; } = Vector2.right;
        public Vector2 CursorPositionNormalized { get; private set; } = Vector2.right;

        public InputType CurrentInputType { get; private set; }

        protected void Start()
        {
            _controls = new MainControls();
            _controls.Enable();

            _controls.Player.Move.performed += (ctx) => _moveDirectionInput = ctx.ReadValue<Vector2>();
            _controls.Player.Move.canceled += (ctx) => _moveDirectionInput = Vector2.zero;

            _controls.Player.Look.performed += (ctx) => _lookDirectionInput = ctx.ReadValue<Vector2>();
            _controls.Player.Look.canceled += (ctx) => _lookDirectionInput = Vector2.zero;

            _controls.Player.Attack.started += (ctx) => _player.StartInteraction();
            _controls.Player.Attack.canceled += (ctx) => _player.EndInteraction();

            _input = GetComponent<PlayerInput>();
            _input.onControlsChanged += OnControlsChanged;
        }

        private void Update()
        {
            switch (CurrentInputType)
            {
                case InputType.KeyboardMouse:
                    CursorPositionNormalized = (new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) * 2) - Vector2.one;
                    CursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    _player.MovementDirection = _moveDirectionInput;
                    _player.LookDirection = CursorPositionNormalized;
                    break;
                case InputType.Gamepad:
                    CursorPositionNormalized = (
                        _lookDirectionInput.magnitude > 0.1f ?
                            _lookDirectionInput :
                            (_moveDirectionInput.magnitude > 0.1f ?
                                _moveDirectionInput :
                                CursorPositionNormalized)
                        ).normalized;
                    CursorPosition = _player.transform.position + (Vector3)(CursorPositionNormalized * 5);

                    _player.MovementDirection = _moveDirectionInput;
                    _player.LookDirection = CursorPositionNormalized;
                    break;
            }
        }

        private void OnControlsChanged(PlayerInput input)
        {
            string currentScheme = input.currentControlScheme;

            switch (currentScheme)
            {
                case "Keyboard&Mouse":
                    CurrentInputType = InputType.KeyboardMouse;
                    break;
                case "Gamepad":
                    CurrentInputType = InputType.Gamepad;
                    break;
                default:
                    break;
            }
        }


        private void OnDestroy()
        {
            _input.onControlsChanged -= OnControlsChanged;
        }
    }

    public enum InputType
    {
        KeyboardMouse,
        Gamepad
    }
}