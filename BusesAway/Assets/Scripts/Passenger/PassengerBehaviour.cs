using BA.Lane;
using UnityEngine;
using UnityEngine.Pool;

namespace BA.Passenger
{
    public enum PassengerState
    {
        Idle,
        MoveUp
    }

    public class PassengerBehaviour : MonoBehaviour, IHaveId
    {
        private LaneManager laneManager;
        private MeshRenderer meshRenderer;
        private ObjectPool<PassengerBehaviour> pool;

        [SerializeField] private PassengerState state;
        [SerializeField] private PassengerColor passengerColor;
        [SerializeField] private int laneIndex;
        [SerializeField] private int row;
        [SerializeField] private int col;

        public void Setup(ObjectPool<PassengerBehaviour> pool, PassengerColor passengerColor, Material material, Vector3 initialPosition)
        {
            this.laneManager = FindAnyObjectByType<GameManager>().GetLaneManager();
            this.state = PassengerState.Idle;
            this.pool = pool;
            this.meshRenderer = GetComponent<MeshRenderer>();
            this.meshRenderer.materials = new Material[] { material };
            this.passengerColor = passengerColor;
            this.transform.position = initialPosition;
        }

        public void Setup(int laneIndex, int row, int col)
        {
            this.laneIndex = laneIndex;
            this.row = row;
            this.col = col;
        }

        public void ChangeRow(int row)
        {
            this.row = row;
            ChangeState(PassengerState.MoveUp);
        }

        public void ChangeState(PassengerState state)
        {
            this.state = state;
        }

        public PassengerState GetState()
        {
            return this.state;
        }

        public void SetState(PassengerState state)
        {
            this.state = state;
        }

        public PassengerColor GetColor()
        {
            return this.passengerColor;
        }

        public void Release()
        {
            this.pool.Release(this);
        }

        public void OnUpdate(float dt)
        {
            if (this.state == PassengerState.MoveUp)
            {
                var laneGrid = this.laneManager.GetLane(this.laneIndex).GetGrid();
                var targetPoint = laneGrid.GetPoint(this.row, this.col);
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPoint, dt * 10);
            
                var distance = Vector3.Distance(targetPoint, this.transform.position);
                if (distance < 0.1f)
                {
                    ChangeState(PassengerState.Idle);
                }
            }
        }

        public void GetLRC(out int laneIndex, out int row, out int col)
        {
            laneIndex = this.laneIndex;
            row = this.row;
            col = this.col;
        }

        public int GetRow()
        {
            return this.row;
        }
    }
}