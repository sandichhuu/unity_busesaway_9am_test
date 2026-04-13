using BA.Bus;
using BA.Lane;
using BA.Level;
using BA.Map;
using BA.Passenger;
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
        private MapBehaviour map;

        private List<PassengerBehaviour> passengersOnStation = new();
        private List<Vector3> busyStationPoints = new();
        private List<Vector3> stationPoints = new();

        private bool isGameover;

        private List<BusBehaviour> spawnedBuses = new();
        private float spawnBusDelayInterval = 0f;
        private List<PassengerBehaviour> readyEnterBusBuffer = new();

        private int score;
        private int maxScore;

        private List<PassengerColor> busesSpawnConfig = new();

        void IGameState.OnEnter()
        {
            var gameManager = Object.FindAnyObjectByType<GameManager>();
            this.levelManager = gameManager.levelManager;
            this.laneManager = gameManager.GetLaneManager();
            this.gameStateManager = gameManager.stateManager;
            this.busManager = gameManager.GetBusManager();
            this.busStationBehaviour = Object.FindAnyObjectByType<BusStationBehaviour>();
            this.map = Object.FindAnyObjectByType<MapBehaviour>();
            this.stationPoints = this.busStationBehaviour.GetGrid().GetShuffled();
            this.score = 0;
            InitSpawnBusQueue();
            //GenerateBusCommon();
        }

        private void InitSpawnBusQueue()
        {
            foreach (var kv in this.levelManager.GetCurrentLevelConfig().lane1Config)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    this.busesSpawnConfig.Add(kv.Key);
                    this.maxScore += 32;
                }
            }

            foreach (var kv in this.levelManager.GetCurrentLevelConfig().lane2Config)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    this.busesSpawnConfig.Add(kv.Key);
                    this.maxScore += 32;
                }
            }

            foreach (var kv in this.levelManager.GetCurrentLevelConfig().lane3Config)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    this.busesSpawnConfig.Add(kv.Key);
                    this.maxScore += 32;
                }
            }

            this.busesSpawnConfig.Shuffle();
        }

        void IGameState.OnExit()
        {
        }

        void IGameState.OnFixedUpdate(float fdt)
        {
            if (this.isGameover)
                return;
        }

        void IGameState.OnUpdate(float dt)
        {
            if (this.isGameover)
                return;

            UpdateInteraction();
            UpdateSpawnBus(dt);
            UpdateBuses(dt);
            UpdatePassengers(dt);
            UpdateMissingRows(dt);
            UpdatePassengerMoveToBus(dt);
        }

        private void UpdatePassengers(float dt)
        {
            UpdatePassengerMoveUp(dt);
        }

        private void UpdatePassengerMoveUp(float dt)
        {
            var allLanes = this.laneManager.AllLanes();
            for (var i = 0; i < allLanes.Length; i++)
            {
                var lane = allLanes[i];
                var passengerBlocks = lane.GetPassengerBlocks().ToList();
                for (var j = 0; j < passengerBlocks.Count; j++)
                {
                    var passengerBlock = passengerBlocks[j];
                    for (var k = 0; k < passengerBlock.passengers.Count; k++)
                    {
                        var passenger = passengerBlock.passengers[k];
                        passenger.OnUpdate(dt);
                    }
                }
            }
        }

        private void UpdateMissingRows(float dt)
        {
            var allLanes = this.laneManager.AllLanes();
            for (var i = 0; i < allLanes.Length; i++)
            {
                var lane = allLanes[i];
                var missingRow = lane.GetMissingRow();
                if (missingRow > 0)
                {
                    var passengerBlocks = lane.GetPassengerBlocks().ToList();
                    for (var j = 0; j < passengerBlocks.Count; j++)
                    {
                        var passengerBlock = passengerBlocks[j];
                        for (var k = 0; k < passengerBlock.passengers.Count; k++)
                        {
                            var passenger = passengerBlock.passengers[k];
                            var r = passenger.GetRow();
                            passenger.ChangeRow(r - missingRow);
                        }
                    }

                    lane.SetMissingRow(0);
                }
            }
        }

        private void UpdateSpawnBus(float dt)
        {
            if (this.spawnedBuses.Count >= this.levelManager.GetCurrentLevelConfig().maxBusAtSameTime)
                return;

            if (this.spawnedBuses.Any(bus => bus.GetCurrentState() == BusState.Idle))
                return;

            if (this.busesSpawnConfig.Count <= 0)
                return;

            this.spawnBusDelayInterval += dt;
            if (this.spawnBusDelayInterval >= Config.DELAY_SPAWN_BUS_DURATION)
            {
                this.spawnBusDelayInterval = 0f;

                var spawnColor = this.busesSpawnConfig[0];
                this.busesSpawnConfig.RemoveAt(0);
                this.spawnedBuses.Add(this.busManager.CreateBus(spawnColor, this.map.GetSpawnPoint()));

                //if (this.passengersOnStation.Count < this.busStationBehaviour.GetCapacity() * Config.SPAWN_BUS_SURE_WIN_THRESHOLE)
                //    GenerateBusCommon();
                //else
                //    GenerateBusSureWin();
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
                        if (IsAllowBusMove(bus, Config.START_BUS_DISTANCE))
                            bus.SetCurrentState(BusState.Starting);
                        break;
                    case BusState.Starting:
                        busMovement.UpdateMovementToTarget(dt);
                        if (busMovement.IsReachStartupPoint())
                            bus.SetCurrentState(BusState.Moving);
                        break;
                    case BusState.Moving:
                        if (IsAllowBusMove(bus, Config.BUS_DISTANCE_MOVABLE))
                        {
                            busMovement.UpdateMovementSpine(dt);
                            UpdateSwitchToParkingState(bus);
                        }
                        break;
                    case BusState.Parking:
                        UpdateParkingState(bus);
                        break;
                    case BusState.MoveToPreDespawn:
                        UpdateMoveToPreDespawnPoint(dt, bus);
                        break;
                    case BusState.MoveToDespawn:
                        UpdateMoveToDespawn(dt, bus);
                        break;
                    default:
                        break;
                }
            }
        }

        private void UpdateMoveToDespawn(float dt, BusBehaviour bus)
        {
            bus.GetBusMovementUpdater().UpdateMovementToTarget(dt);

            var despawnPoint = this.map.GetDespawnPoint();
            var busPosition = bus.transform.position;
            var distance = Vector3.Distance(busPosition, despawnPoint);
            if (distance < 0.1f)
            {
                bus.Release();
                this.spawnedBuses.Remove(bus);
                this.score += 32;

                // 160 score is max per lane, 3 lane is 480
                if (this.score >= this.maxScore)
                {
                    this.gameStateManager.ChangeState(new LevelWinState());
                }
            }
        }

        private void UpdateMoveToPreDespawnPoint(float dt, BusBehaviour bus)
        {
            var busMovement = bus.GetBusMovementUpdater();
            busMovement.UpdateMovementSpine(dt);
            var distance = Mathf.Abs(this.map.GetPreDespawnNormalizedT() - busMovement.GetNormalizedT());
            if (distance < 0.01f)
            {
                bus.SetCurrentState(BusState.MoveToDespawn);
                busMovement.movementTargetPoint = this.map.GetDespawnPoint();
            }
        }

        private void UpdateParkingState(BusBehaviour bus)
        {
            bus.GetGrid().CalculatePoints();
            var busEntryPoint = bus.GetBusEntryPoint();
            for (var i = this.readyEnterBusBuffer.Count - 1; i >= 0; i--)
            {
                var passenger = this.readyEnterBusBuffer[i];
                if (passenger != null)
                {
                    passenger.transform.position = Vector3.MoveTowards(passenger.transform.position,
                                                                       busEntryPoint,
                                                                       10 * Time.deltaTime);

                    if (Vector3.Distance(passenger.transform.position, busEntryPoint) <= 0.1f)
                    {
                        passenger.Release();
                        bus.AddPassenger();
                        this.readyEnterBusBuffer.RemoveAt(i);
                    }
                }
            }

            if (this.readyEnterBusBuffer.Count <= 0)
            {
                if (bus.availableSeats <= 0)
                {
                    bus.SetCurrentState(BusState.MoveToPreDespawn);
                }
                else
                {
                    bus.SetCurrentState(BusState.Moving);
                }
            }
        }

        private void UpdateSwitchToParkingState(BusBehaviour bus)
        {
            if (this.passengersOnStation.Any(p => p.GetColor() == bus.GetColor()) && IsReadyToParking(bus))
            {
                bus.SetCurrentState(BusState.Parking);

                var availableSeats = bus.availableSeats;
                for (var i = this.passengersOnStation.Count - 1; i >= 0; i--)
                {
                    var passenger = this.passengersOnStation[i];
                    if (passenger.GetColor() == bus.GetColor())
                    {
                        this.readyEnterBusBuffer.Add(passenger);
                        this.passengersOnStation.RemoveAt(i);
                        this.stationPoints.Add(this.busyStationPoints[i]);
                        this.busyStationPoints.RemoveAt(i);

                        if (this.readyEnterBusBuffer.Count == availableSeats)
                            break;
                    }
                }
            }
        }

        private bool IsReadyToParking(BusBehaviour bus)
        {
            var parkingPoint = this.map.GetParkingPoint();
            var distance = Vector3.Distance(bus.transform.position, parkingPoint);

            return distance < 0.1f;
        }

        private bool IsAllowBusMove(BusBehaviour bus, float dst)
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
                var isBothMoving = bus.GetCurrentState() == BusState.Moving &&
                                   otherBus.GetCurrentState() == BusState.Moving;
                var isOtherStopping = otherBus.GetCurrentState() == BusState.MoveToDespawn;
                if (distance < dst && (!isBothMoving || isOtherStopping))
                {
                    allowMove = false;
                    break;
                }
            }

            return allowMove;
        }

        private void UpdatePassengerMoveToBus(float dt)
        {
            for (var i = this.passengersOnStation.Count - 1; i >= 0; i--)
            {
                var passenger = this.passengersOnStation[i];
                if (passenger != null)
                {
                    passenger.transform.position = Vector3.MoveTowards(passenger.transform.position,
                                                                       this.busyStationPoints[i],
                                                                       Config.DEFAULT_BUS_MOVEMENT_SPEED * Time.deltaTime);
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
                lane.SetMissingRow(firstBlock.passengers.Count / 4);
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
            await UniTask.Delay(1000);
            this.gameStateManager.ChangeState(new LevelGameoverState());
        }

        private void GenerateBusSureWin()
        {
            // Make sure do not spawn too much bus at same time
            if (this.spawnedBuses.Count >= this.levelManager.GetCurrentLevelConfig().maxBusAtSameTime)
                return;

            this.passengersOnStation.Max(p => p.GetColor());

            var mostFrequent = this.passengersOnStation
                .GroupBy(obj => obj.GetColor()) // Group by color
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();

            this.spawnedBuses.Add(this.busManager.CreateBus(mostFrequent, this.map.GetSpawnPoint()));
        }

        private void GenerateBusCommon()
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
                this.spawnedBuses.Add(this.busManager.CreateBus(randomColor, this.map.GetSpawnPoint()));
            }
        }
    }
}