using UnityEngine;

namespace BA.GameStates
{
    public class LevelGameoverState : IGameState
    {
        void IGameState.OnEnter()
        {
            var winCanvas = Object.FindAnyObjectByType<UI_Lose>();
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