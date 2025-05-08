using Game.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace Game.Controllers.UI
{
    public class InventoryUIController : MonoBehaviour, IInputConsumer
    {
        public bool IsOn { get; private set; }

        public UnityEvent<bool> StateChanged;

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

        public void Activate()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed += OnInventoryButtonPressed;
        }

        public void Deactivate()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed -= OnInventoryButtonPressed;
        }

        private void OnInventoryButtonPressed(CallbackContext context)
        {
            Toggle(false);
        }
    }
}