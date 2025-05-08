using UnityEngine;

namespace Game.Interactions
{
    public interface IInteractable
    {
        public void Interact();

        public void Activate();
        public void Deactivate();

        public Vector2 InteractablePosition { get; }
    }
}