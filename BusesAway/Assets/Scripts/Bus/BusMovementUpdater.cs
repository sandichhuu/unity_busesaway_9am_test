using BA.Map;
using UnityEngine;

namespace BA.Bus
{
    public class BusMovementUpdater : MonoBehaviour
    {
        public MapBehaviour map;
        public float movementSpeed;
        public float rotationSpeed;
        public float stopPointT;

        [Space]
        [Header("DEBUG")]
        public Vector3 movementTargetPoint;
        public float normalizedT;

        public void Setup()
        {
            this.map = FindAnyObjectByType<MapBehaviour>();
            this.movementTargetPoint = this.map.GetSplinePath().FindNearestPointTo(this.transform.position, out this.normalizedT);
        }

        public bool IsReachStartupPoint()
        {
            return Vector3.Distance(this.transform.position, this.movementTargetPoint) <= 0.1f;
        }

        public void UpdateMovementToTarget(float dt)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                              this.movementTargetPoint,
                                                              this.movementSpeed * Config.BUS_START_MOVE_SPEED * dt);

            var lookPos = this.movementTargetPoint - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, dt * this.rotationSpeed);
        }

        public void UpdateMovementSpine(float dt)
        {
            var targetPoint = this.map.GetSplinePath().MoveAlongSpline(ref this.normalizedT, -this.movementSpeed);
            var lookPos = targetPoint - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            this.transform.position = targetPoint;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, dt * this.rotationSpeed);
        }

        public float GetNormalizedT()
        {
            var t = this.normalizedT;
            t %= -1;

            if (t < 0)
                t += 1;

            return t;
        }
    }
}
