using UnityEngine;

public class HorizontalMovementController : MonoBehaviour
{
    [Header("Locomotion")]
    [SerializeField] private float maxForce;
    [SerializeField] private float acceleration;
    [SerializeField] private AnimationCurve accelerationFactorFromDot;
    [SerializeField] private float maxAccelForce;
    [SerializeField] private AnimationCurve MaxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 forceScale;
    private Vector3 mGoalVel;
    private Vector3 LocoMovement;
   
    public void HorizontalMovement(Vector3 directionMovement,Rigidbody rb)
    {
        Vector3 currentVelocity = mGoalVel.normalized;

        float velDot = Vector3.Dot(directionMovement, currentVelocity);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velDot);

        Vector3 goalVel = directionMovement * maxForce;
        mGoalVel = Vector3.MoveTowards(mGoalVel, goalVel, accel * Time.fixedDeltaTime);

        LocoMovement = (mGoalVel - rb.linearVelocity) / Time.fixedDeltaTime;

        float maxAccel = maxAccelForce * MaxAccelerationForceFactorFromDot.Evaluate(velDot);
        LocoMovement = Vector3.ClampMagnitude(LocoMovement, maxAccel);
    }
    public void ApplyLogic(Rigidbody rb)
    {
        rb.AddForce(Vector3.Scale(LocoMovement * rb.mass, forceScale));
    }
   
}
