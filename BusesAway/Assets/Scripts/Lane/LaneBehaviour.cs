using System.Collections.Generic;
using UnityEngine;

namespace BA.Lane
{
    public class LaneBehaviour : MonoBehaviour, ISetup
    {
        [SerializeField] private int laneIndex;
        [SerializeField] private int missingRow;

        private Grid grid;
        private Queue<PassengerBlock> passengerBlocks = new();

        private void Awake()
        {
            this.grid = GetComponent<Grid>();
        }

        void ISetup.Invoke()
        {
        }

        public Grid GetGrid()
        {
            return this.grid;
        }

        public ref Queue<PassengerBlock> GetPassengerBlocks()
        {
            return ref this.passengerBlocks;
        }

        public int GetLaneIndex()
        {
            return this.laneIndex;
        }

        public void SetMissingRow(int row)
        {
            this.missingRow = row;
        }

        public int GetMissingRow()
        {
            return this.missingRow;
        }
    }
}