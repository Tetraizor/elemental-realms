using System.Linq;
using Game.Input;
using Tetraizor.MonoSingleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Controllers.UI
{
    public class PlayerUIController : MonoSingleton<PlayerUIController>, IInputConsumer
    {
        public bool IsOn { get; private set; }
        [HideInInspector] public UnityEvent<bool> StateChanged;

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

        public void ActivateInput()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed += OnInventoryButtonPressed;

            var inventoryControllers = GetComponentsInChildren<InventoryUIController>().ToList();
            inventoryControllers.ForEach(inventory => inventory.ActivateInput());
        }

        public void DeactivateInput()
        {
            var inputController = FindFirstObjectByType<InputController>();

            inputController.Controls.Player.Inventory.performed -= OnInventoryButtonPressed;

            var inventoryControllers = GetComponentsInChildren<InventoryUIController>().ToList();
            inventoryControllers.ForEach(inventory => inventory.DeactivateInput());
        }

        private void OnInventoryButtonPressed(InputAction.CallbackContext context)
        {
            Toggle(false);
        }
    }
}