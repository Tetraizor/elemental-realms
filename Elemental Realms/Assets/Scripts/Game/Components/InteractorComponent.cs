using System.Collections.Generic;
using System.Linq;
using Game.Interactions;
using Game.Inventories;
using UnityEngine;

namespace Game.Components
{
    public class InteractorComponent : MonoBehaviour, IInteractorFieldParent
    {
        [SerializeField] private InteractorField _interactorField;
        private List<IInteractable> _interactables = new();

        private void Start()
        {
            _interactorField.Setup(this);
        }

        private void Update()
        {
            if (_interactables.Count > 1)
            {
                UpdateInteractables();
            }
        }

        public void Interact()
        {
            if (_interactables.Count == 0) return;

            var interactable = _interactables.First();
            interactable.Interact();
        }

        public void OnHitStarted(Collider2D collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                _interactables.Add(interactable);

                UpdateInteractables();
            }
        }

        public void OnHitStayed(Collider2D collider) { }

        public void OnHitEnded(Collider2D collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (_interactables.Contains(interactable))
                {
                    _interactables.Remove(interactable);
                    interactable.Deactivate();

                    UpdateInteractables();
                }
            }
        }

        public void UpdateInteractables()
        {
            _interactables.Sort((pickable1, pickable2) =>
            {
                float distance1 = (transform.position - (Vector3)pickable1.InteractablePosition).magnitude;
                float distance2 = (transform.position - (Vector3)pickable2.InteractablePosition).magnitude;

                return distance1.CompareTo(distance2);
            });

            for (int i = 0; i < _interactables.Count; i++)
            {
                var interactable = _interactables[i];

                if (i == 0)
                {
                    interactable.Activate();
                }
                else
                {
                    interactable.Deactivate();
                }
            }
        }
    }
}