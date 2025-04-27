namespace Game.Interactors
{
    public interface IInteractorEntity
    {
        public void StartInteraction();
        public void EndInteraction();
        public void SetInteractor(InteractorBase interactor);
    }
}