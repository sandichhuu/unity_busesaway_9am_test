using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BA.Lane
{
    [Serializable]
    public class LaneManager
    {
        [SerializeField] private LaneBehaviour[] lanes;

        public LaneBehaviour GetLane(int index)
        {
            return this.lanes[index];
        }

        public ref LaneBehaviour[] AllLanes()
        {
            return ref this.lanes;
        }
    }
}