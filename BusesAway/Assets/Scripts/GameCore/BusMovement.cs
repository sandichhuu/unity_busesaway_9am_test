using BezierSolution;
using UnityEngine;

public class BusMovement : MonoBehaviour
{
    public enum BusMovementPhase
    {
        SettingUp,
        MoveSpline
    }

    public BezierSpline spline;
    public float movementSpeed;
    public float rotationSpeed;

    private Vector3 startPoint;
    private float normalizedT;

    private BusMovementPhase currentState = BusMovementPhase.SettingUp;

    void Start()
    {
        this.startPoint = this.spline.FindNearestPointTo(this.transform.position,out this.normalizedT);
        this.currentState = BusMovementPhase.SettingUp;
    }

    void Update()
    {
        if (this.currentState == BusMovementPhase.SettingUp) {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,
                                                                         this.startPoint,
                                                                         this.movementSpeed * 100 * Time.deltaTime);

            var lookPos = this.startPoint - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * this.rotationSpeed);

            if (Vector3.Distance(this.transform.position, this.startPoint) <= 0.1f)
                this.currentState = BusMovementPhase.MoveSpline;
        }
        else
        {
            var targetPoint = this.spline.MoveAlongSpline(ref this.normalizedT, -this.movementSpeed);
            var lookPos = targetPoint - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            this.transform.position = targetPoint;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * this.rotationSpeed);
        }
    }
}
