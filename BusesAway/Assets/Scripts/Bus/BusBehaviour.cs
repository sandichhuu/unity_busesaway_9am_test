using BA.Passenger;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private TextMeshPro quantityText;

        private int seatCount;
        public int availableSeats => this.capacity - this.seatCount;

        private PassengerManager passengerManager;
        private List<PassengerBehaviour> passengers = new();

        public void Setup(ObjectPool<BusBehaviour> pool, PassengerColor passengerColor, Material material, Vector3 initialPosition)
        {
            var gm = FindAnyObjectByType<GameManager>();
            this.passengerManager = gm.GetPassengerManager();

            this.pool = pool;
            this.meshRenderer.materials = new Material[] { material };
            this.passengerColor = passengerColor;
            this.transform.position = initialPosition;
            this.currentBusState = BusState.Idle;
            this.seatCount = 0;
            this.quantityText.text = "0/32";
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
            this.quantityText.text = $"{this.seatCount}/32";
            this.quantityText.transform.DOPunchScale(Vector3.one * .08f, .1f, 0, 1).OnComplete(() =>
            {
                this.quantityText.transform.localScale = Vector3.one;
            });

            this.passengers.Add(passenger);
            passenger.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1, 1);
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