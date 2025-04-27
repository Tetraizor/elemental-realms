using Game.Data;
using Game.Entities.Common;
using Game.Interactables;
using Game.Interactors;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Entities.Player
{
    public class PlayerEntity : DynamicEntityBase, IInteractorEntity
    {
        public UnityEvent<InteractionData> InteractionStarted = new();
        public UnityEvent<InteractionData> InteractionCancelled = new();
        public UnityEvent<InteractionData> InteractionEnded = new();

        public bool IsInteracting { get; private set; } = false;
        public bool CanInteract { get; private set; } = true;

        public InteractorBase Interactor { get; private set; }

        [SerializeField] private Fist _fistPrefab;

        protected override void Awake()
        {
            base.Awake();

            var fist = Instantiate(_fistPrefab, transform.position, Quaternion.identity);
            fist.transform.parent = transform;
            SetInteractor(fist);

            StateManager.SetState(new PlayerIdleState(this));
        }

        public void StartInteraction()
        {
            if (!CanInteract) return;

            if (IsInteracting) return;
            else IsInteracting = true;

            InteractionStarted?.Invoke(new InteractionData());
        }

        public void EndInteraction()
        {
            if (!IsInteracting) return;
            else IsInteracting = false;

            InteractionEnded?.Invoke(new InteractionData());
        }

        public void CancelInteraction()
        {
            if (!IsInteracting) return;
            else IsInteracting = false;

            InteractionCancelled?.Invoke(new InteractionData());
        }

        public void SetInteractor(InteractorBase interactor)
        {
            if (Interactor != null)
            {
                InteractionStarted.RemoveListener(Interactor.StartInteraction);
                InteractionEnded.RemoveListener(Interactor.EndInteraction);
                InteractionCancelled.RemoveListener(Interactor.CancelInteraction);

                Interactor.PutDown();
            }

            Interactor = interactor;

            if (Interactor != null)
            {
                InteractionStarted.AddListener(Interactor.StartInteraction);
                InteractionEnded.AddListener(Interactor.EndInteraction);
                InteractionCancelled.AddListener(Interactor.CancelInteraction);

                Interactor.PullUp(this);
            }

            // TODO: Show tool on player.
        }

        public override float GetFinalSpeedMultiplier()
        {
            return base.GetFinalSpeedMultiplier() * Interactor.SpeedMultiplier;
        }
    }
}