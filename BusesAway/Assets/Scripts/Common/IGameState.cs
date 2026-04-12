public interface IGameState
{
    void OnEnter();
    void OnUpdate(float dt);
    void OnFixedUpdate(float fdt);
    void OnExit();
}