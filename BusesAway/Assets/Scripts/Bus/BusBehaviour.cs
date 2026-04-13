using BA.Passenger;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BA.Bus
{
    public enum BusState
    {
        Idle,
        Starting,
        Moving,
        Parking,
        MoveToPreDespawn,
        MoveToDespawn
    }

    public class BusBehaviour : MonoBehaviour, IHaveId
    {
        private ObjectPool<BusBehaviour> pool;

        [SerializeField] private Transform busEntryPoint;
        [SerializeField] private Grid grid;
        [SerializeField] private BusMovementUpdater busMovement;
        [SerializeField] private PassengerColor passengerColor;
        [SerializeField] private BusState currentBusState;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private int capacity;

        private int seatCount;
        public int availableSeats => this.capacity - this.seatCount;

        private PassengerManager passengerManager;
        private List<PassengerBehaviour> passengers = new();

        public void Setup(ObjectPool<BusBehaviour> pool, PassengerColor passengerColor, Material material, Vector3 initialPosition)
        {
            this.passengerManager = FindAnyObjectByType<GameManager>().GetPassengerManager();

            this.pool = pool;
            this.meshRenderer.materials = new Material[] { material };
            this.passengerColor = passengerColor;
            this.transform.position = initialPosition;
            this.currentBusState = BusState.Idle;
            this.seatCount = 0;
        }

        public void Release()
        {
            this.seatCount = 0;
            ResetState();
            this.pool.Release(this);
        }

        public BusMovementUpdater GetBusMovementUpdater()
        {
            return this.busMovement;
        }

        public BusState GetCurrentState()
        {
            return this.currentBusState;
        }

        public void SetCurrentState(BusState busState)
        {
            this.currentBusState = busState;
        }

        public PassengerColor GetColor()
        {
            return this.passengerColor;
        }

        public void AddPassenger()
        {
            var passenger = this.passengerManager
                .CreatePassenger(this.passengerColor, this.grid.GetPoint(this.seatCount));
            passenger.transform.SetParent(this.transform);
            this.seatCount++;

            this.passengers.Add(passenger);
        }

        public Vector3 GetBusEntryPoint()
        {
            return this.busEntryPoint.position;
        }

        public void ResetState()
        {
            for (var i = 0; i < this.passengers.Count; i++)
                this.passengers[i].Release();

            this.passengers.Clear();
            this.currentBusState = BusState.Idle;
        }

        public Grid GetGrid()
        {
            return this.grid;
        }
    }
}