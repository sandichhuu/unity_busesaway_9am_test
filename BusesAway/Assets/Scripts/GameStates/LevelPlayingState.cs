using BA.Lane;
using BA.Level;
using BA.Passenger;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BA.GameStates
{
    public class LevelPlayingState : IGameState
    {
        private LaneManager laneManager;
        private LevelManager levelManager;
        private BusStationBehaviour busStationBehaviour;
        private UpdateSystem updateSystem;

        private List<PassengerBehaviour> passengersOnStation = new();
        private List<Vector3> busyStationPoints = new();
        private List<Vector3> stationPoints = new();
        private int currentStationPointIndex = 0;

        private bool isGameover;

        void IGameState.OnEnter()
        {
            var gameManager = Object.FindAnyObjectByType<GameManager>();
            this.updateSystem = gameManager.updateSystem;
            this.levelManager = gameManager.levelManager;
            this.laneManager = gameManager.GetLaneManager();
            this.busStationBehaviour = Object.FindAnyObjectByType<BusStationBehaviour>();
            this.stationPoints = this.busStationBehaviour.GetGrid().GetShuffled();
        }

        void IGameState.OnExit()
        {
        }

        void IGameState.OnFixedUpdate(float fdt)
        {
        }

        void IGameState.OnUpdate(float dt)
        {
            if (this.isGameover)
                return;

            UpdateInteraction();
            UpdatePassengerMovement(dt);
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
        }
    }
}