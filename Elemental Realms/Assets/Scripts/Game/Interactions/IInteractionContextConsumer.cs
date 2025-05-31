using Game.Data;

namespace Game.Interactions
{
    public interface IInteractionContextConsumer
    {
        public void ConsumeContext(InteractionContext ctx);
    }
}