using BA.Tunnel;
using BezierSolution;
using UnityEngine;

namespace BA.Map
{
    public class MapBehaviour : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private BezierSpline splinePath;
        [SerializeField] private Transform parkingPoint;
        [SerializeField] private StartTunnelBehaviour startTunnelBehaviour;
        [SerializeField] private EndTunnelBehaviour endTunnelBehaviour;
        [SerializeField] private float offsetPreDespawnNormalizedT;

        [Header("Debug")]
        [SerializeField] private Vector3 preDespawnPoint;
        [SerializeField] private float preDespawnNormalizedT;

        private void Awake()
        {
            this.preDespawnPoint = this.splinePath
                .FindNearestPointTo(this.endTunnelBehaviour.transform.position, out this.preDespawnNormalizedT);
        }

        public BezierSpline GetSplinePath()
        {
            return this.splinePath;
        }

        public Vector3 GetParkingPoint()
        {
            return this.parkingPoint.position;
        }

        public Vector3 GetSpawnPoint()
        {
            return this.startTunnelBehaviour.transform.position;
        }

        public Vector3 GetDespawnPoint()
        {
            return this.endTunnelBehaviour.transform.position;
        }

        public Vector3 GetPreDespawnPoint()
        {
            return this.preDespawnPoint;
        }

        public float GetPreDespawnNormalizedT()
        {
            return this.preDespawnNormalizedT + this.offsetPreDespawnNormalizedT;
        }
    }
}