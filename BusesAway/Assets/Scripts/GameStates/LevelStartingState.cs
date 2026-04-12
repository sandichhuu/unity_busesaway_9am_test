using BA.Lane;
using BA.Level;
using BA.Passenger;

namespace BA.GameStates
{
    public class LevelStartingState : IGameState
    {
        private readonly GameManager gameManager;
        private readonly PassengerManager passengerManager;
        private readonly LaneManager laneManager;
        private readonly LevelManager levelManager;

        public LevelStartingState()
        {
            this.gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();
            this.passengerManager = this.gameManager.GetPassengerManager();
            this.laneManager = this.gameManager.GetLaneManager();
            this.levelManager = this.gameManager.levelManager;
        }

        void IGameState.OnEnter()
        {
            var levelConfig = this.levelManager.GetCurrentLevelConfig();
            var spawnQueues = levelConfig.spawnQueues;

            this.passengerManager.SpawnPassengerBlock(this.laneManager.GetLane(0), spawnQueues[0]);
            this.passengerManager.SpawnPassengerBlock(this.laneManager.GetLane(1), spawnQueues[1]);
            this.passengerManager.SpawnPassengerBlock(this.laneManager.GetLane(2), spawnQueues[2]);
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