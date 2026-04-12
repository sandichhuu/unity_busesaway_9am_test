using BezierSolution;
using UnityEngine;

namespace BA.Bus
{
    public class BusMovementUpdater : MonoBehaviour
    {
        public BezierSpline spline;
        public float movementSpeed;
        public float rotationSpeed;
        public float stopPointT;

        [Space]
        [Header("DEBUG")]
        public Vector3 startPoint;
        public float normalizedT;

        public void Setup()
        {
            this.spline = FindAnyObjectByType<BezierSpline>();
            this.startPoint = this.spline.FindNearestPointTo(this.transform.position, out this.normalizedT);
        }

        public bool IsReachStartupPoint()
        {
            return Vector3.Distance(this.transform.position, this.startPoint) <= 0.1f;
        }

        public void UpdateMovementStartup(float dt)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                              this.startPoint,
                                                              this.movementSpeed * Config.BUS_START_MOVE_SPEED * dt);

            var lookPos = this.startPoint - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, dt * this.rotationSpeed);
        }

        public void UpdateMovementSpine(float dt)
        {
            var targetPoint = this.spline.MoveAlongSpline(ref this.normalizedT, -this.movementSpeed);
            var lookPos = targetPoint - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            this.transform.position = targetPoint;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, dt * this.rotationSpeed);
        }
    }
}
