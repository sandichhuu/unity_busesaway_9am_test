using System;
using UnityEngine;

namespace BA.Lane
{
    [Serializable]
    public class LaneManager
    {
        [SerializeField] private LaneBehaviour[] lanes;

        public LaneBehaviour GetLane(int index)
        {
            return lanes[index];
        }
    }
}