public class GameStateManager : ILoop, IFixedLoop
{
    private IGameState currentState;

    public void ChangeState(IGameState newState)
    {
        this.currentState?.OnExit();
        this.currentState = newState;
        this.currentState?.OnEnter();
    }

    void ILoop.Invoke(float dt)
    {
        this.currentState?.OnUpdate(dt);
    }

    void IFixedLoop.Invoke(float fdt)
    {
        this.currentState?.OnFixedUpdate(fdt);
    }
}