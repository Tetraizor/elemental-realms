using Game.Enum;
using Game.Input;
using Tetraizor.MonoSingleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Controllers
{
    public class InputController : MonoSingleton<InputController>
    {
        public MainControls Controls { get; private set; }
        public PlayerInput Input { get; private set; }
        public InputType ActiveInputType { get; private set; }

        private void Start()
        {
            Controls = new MainControls();
            Controls.Enable();

            Input = GetComponent<PlayerInput>();
            Input.onControlsChanged += OnControlsChanged;
        }

        public void ActivateConsumer(IInputConsumer consumer)
        {
            consumer.ActivateInput();
        }

        public void DeactivateConsumer(IInputConsumer consumer)
        {
            if (consumer != null) consumer.DeactivateInput();
        }

        private void OnControlsChanged(PlayerInput input)
        {
            string currentScheme = input.currentControlScheme;

            switch (currentScheme)
            {
                case "Keyboard&Mouse":
                    ActiveInputType = InputType.KeyboardMouse;
                    break;
                case "Gamepad":
                    ActiveInputType = InputType.Gamepad;
                    break;
                default:
                    break;
            }
        }

        private void OnDestroy()
        {
            if (Input)
                Input.onControlsChanged -= OnControlsChanged;
        }
    }
}