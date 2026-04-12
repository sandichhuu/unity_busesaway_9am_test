using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BA.Lane
{
    [Serializable]
    public class LaneManager : ILoop
    {
        [SerializeField] private LaneBehaviour[] lanes;

        public LaneBehaviour GetLane(int index)
        {
            return this.lanes[index];
        }

        void ILoop.Invoke(float dt)
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
                        lane.OnLaneSelected();
                    }
                }
            }
        }
    }
}