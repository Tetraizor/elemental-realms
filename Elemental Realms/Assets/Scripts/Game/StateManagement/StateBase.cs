public abstract class StateBase
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void FixedTick(float fixedDeltaTime);
    public abstract bool Exit(StateBase newState);
}