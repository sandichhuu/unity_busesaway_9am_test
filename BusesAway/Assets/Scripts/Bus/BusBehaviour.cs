using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BA.Bus
{
    public enum BusState
    {
        Idle,
        Starting,
        Moving
    }

    public class BusBehaviour : MonoBehaviour, IHaveId
    {
        private MeshRenderer meshRenderer;
        private ObjectPool<BusBehaviour> pool;

        [SerializeField] private BusMovementUpdater busMovement;
        [SerializeField] private PassengerColor passengerColor;
        [SerializeField] public BusState currentBusState;

        public int capacity;
        public List<int> onboardPassengers = new List<int>();
        public bool isFull => this.onboardPassengers.Count >= this.capacity;
        public int availableSeats => this.capacity - this.onboardPassengers.Count;

        public void Setup(ObjectPool<BusBehaviour> pool, PassengerColor passengerColor, Material material, Vector3 initialPosition)
        {
            this.pool = pool;
            this.meshRenderer = GetComponent<MeshRenderer>();
            this.meshRenderer.materials = new Material[] { material };
            this.passengerColor = passengerColor;
            this.transform.position = initialPosition;
            this.currentBusState = BusState.Idle;
        }

        public bool BoardPassengers(int count)
        {
            int toBoard = Mathf.Min(count, this.availableSeats);
            for (int i = 0; i < toBoard; i++)
            {
                this.onboardPassengers.Add(0);
            }
            return toBoard == count;
        }

        public void Clear()
        {
            this.onboardPassengers.Clear();
        }

        public void Release()
        {
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
    }
}