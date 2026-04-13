using UnityEngine;

namespace BA.GameStates
{
    public class LevelWinState : IGameState
    {
        void IGameState.OnEnter()
        {
            var winCanvas = Object.FindAnyObjectByType<UI_Win>();
            winCanvas.Show();
        }

        void IGameState.OnExit()
        {
        }

        void IGameState.OnFixedUpdate(float fdt)
        {
        }

        void IGameState.OnUpdate(float dt)
        {
        }
    }
}