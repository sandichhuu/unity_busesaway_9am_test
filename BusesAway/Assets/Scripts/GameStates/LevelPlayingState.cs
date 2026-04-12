using BA.Bus;
using BA.Lane;
using BA.Level;
using BA.Passenger;
using BA.Tunnel;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BA.GameStates
{
    public class LevelPlayingState : IGameState
    {
        private LaneManager laneManager;
        private LevelManager levelManager;
        private GameStateManager gameStateManager;
        private BusManager busManager;
        private BusStationBehaviour busStationBehaviour;

        private StartTunnelBehaviour startTunnelBehaviour;
        private EndTunnelBehaviour endTunnelBehaviour;

        private List<PassengerBehaviour> passengersOnStation = new();
        private List<Vector3> busyStationPoints = new();
        private List<Vector3> stationPoints = new();

        private bool isGameover;

        private List<BusBehaviour> spawnedBuses = new();
        private float spawnBusDelayInterval = 0f;

        void IGameState.OnEnter()
        {
            var gameManager = Object.FindAnyObjectByType<GameManager>();
            this.levelManager = gameManager.levelManager;
            this.laneManager = gameManager.GetLaneManager();
            this.gameStateManager = gameManager.stateManager;
            this.busManager = gameManager.GetBusManager();
            this.busStationBehaviour = Object.FindAnyObjectByType<BusStationBehaviour>();
            this.startTunnelBehaviour = Object.FindAnyObjectByType<StartTunnelBehaviour>();
            this.endTunnelBehaviour = Object.FindAnyObjectByType<EndTunnelBehaviour>();
            this.stationPoints = this.busStationBehaviour.GetGrid().GetShuffled();

            GenerateBus();
        }

        void IGameState.OnExit()
        {
        }

        void IGameState.OnFixedUpdate(float fdt)
        {
            if (this.isGameover)
                return;

            UpdateInteraction();
        }

        void IGameState.OnUpdate(float dt)
        {
            if (this.isGameover)
                return;

            UpdateSpawnBus(dt);
            UpdateBuses(dt);
            UpdatePassengerMovement(dt);
        }

        private void UpdateSpawnBus(float dt)
        {
            if (this.spawnedBuses.Count >= this.levelManager.GetCurrentLevelConfig().maxBusAtSameTime)
                return;

            if (this.spawnedBuses.Any(bus => bus.GetCurrentState() == BusState.Idle))
                return;

            this.spawnBusDelayInterval += dt;
            if (this.spawnBusDelayInterval >= Config.DELAY_SPAWN_BUS_DURATION)
            {
                this.spawnBusDelayInterval = 0f;
                GenerateBus();
            }
        }

        private void UpdateBuses(float dt)
        {
            for (var i = this.spawnedBuses.Count - 1; i >= 0; i--)
            {
                var bus = this.spawnedBuses[i];
                var busMovement = bus.GetBusMovementUpdater();
                var state = bus.GetCurrentState();
                switch (state)
                {
                    case BusState.Idle:
                        if (IsAllowBusMove(bus))
                            bus.SetCurrentState(BusState.Starting);
                        break;
                    case BusState.Starting:
                        busMovement.UpdateMovementStartup(dt);
                        if (busMovement.IsReachStartupPoint())
                            bus.SetCurrentState(BusState.Moving);
                        break;
                    case BusState.Moving:
                        busMovement.UpdateMovementSpine(dt);
                        break;
                    default:
                        break;
                }
            }
        }

        private bool IsAllowBusMove(BusBehaviour bus)
        {
            var allowMove = true;

            for (var i = this.spawnedBuses.Count - 1; i >= 0; i--)
            {
                var otherBus = this.spawnedBuses[i];
                if (bus.GetUUID() == otherBus.GetUUID())
                    continue;

                var busPosition = bus.transform.position;
                var otherBusPosition = otherBus.transform.position;
                var distance = Vector3.Distance(busPosition, otherBusPosition);

                if (distance < Config.MOVABLE_BUS_DISTANCE)
                {
                    allowMove = false;
                    break;
                }
            }

            return allowMove;
        }

        private void UpdatePassengerMovement(float dt)
        {
            for (var i = this.passengersOnStation.Count - 1; i >= 0; i--)
            {
                var passenger = this.passengersOnStation[i];
                if (passenger != null)
                {
                    passenger.transform.position = Vector3.MoveTowards(passenger.transform.position,
                                                                       this.busyStationPoints[i],
                                                                       Config.DEFAULT_MOVEMENT_SPEED * Time.deltaTime);
                }
            }
        }

        private void UpdateInteraction()
        {
            if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            {
                Vector2 screenPos = Pointer.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<LaneBehaviour>(out var lane))
                    {
                        OnLaneSelected(lane);
                    }
                }
            }
        }

        private void OnLaneSelected(LaneBehaviour lane)
        {
            var passengerBlocks = lane.GetPassengerBlocks();
            var grid = this.busStationBehaviour.GetGrid();
            if (passengerBlocks.Count > 0)
            {
                var firstBlock = passengerBlocks.Dequeue();
                this.passengersOnStation.AddRange(firstBlock.passengers);

                for (var i = 0; i < firstBlock.passengers.Count; i++)
                {
                    if (this.stationPoints.Count > 0)
                    {
                        var stationPoint = this.stationPoints[0];
                        this.busyStationPoints.Add(stationPoint);
                        this.stationPoints.RemoveAt(0);
                    }
                    else
                    {
                        // Move to random point and change state to lose state.
                        this.busyStationPoints.Add(
                            this.busyStationPoints[Random.Range(0, this.busyStationPoints.Count)]);

                        this.isGameover = true;

                        DelayGameover().Forget();
                    }
                }
            }
        }

        private async UniTask DelayGameover()
        {
            await UniTask.Delay(2000);
            Debug.Log("You Lose");
            this.gameStateManager.ChangeState(new LevelGameoverState());
        }

        private void GenerateBus()
        {
            // Make sure do not spawn too much bus at same time
            if (this.spawnedBuses.Count >= this.levelManager.GetCurrentLevelConfig().maxBusAtSameTime)
                return;

            var allLanes = this.laneManager.AllLanes();
            var randomColorList = new List<PassengerColor>();
            for (var i = 0; i < allLanes.Length; i++)
            {
                var randomIndex = Random.Range(1, 4);
                var passengerBlocks = allLanes[i].GetPassengerBlocks();
                PassengerBlock randomPassengerBlock = null;
                if (passengerBlocks.Count > randomIndex)
                    randomPassengerBlock = passengerBlocks.ElementAt(randomIndex);
                else if (passengerBlocks.Count > 0)
                    randomPassengerBlock = passengerBlocks.Peek();

                if (randomPassengerBlock != null)
                    randomColorList.Add(randomPassengerBlock.color);
            }

            Debug.Log($"RdBusList: {string.Join('-', randomColorList)}");

            if (randomColorList.Count > 0)
            {
                var randomColor = randomColorList[Random.Range(0, randomColorList.Count)];
                Debug.Log($"Bus: {randomColor}");
                this.spawnedBuses.Add(this.busManager.CreateBus(randomColor, this.startTunnelBehaviour.transform.position));
            }
        }
    }
}