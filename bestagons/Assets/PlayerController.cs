using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movmenet")]
    [SerializeField] private float maxForce;
    [SerializeField] private float acceleration;
    [SerializeField] private AnimationCurve accelerationFactorFromDot;
    [SerializeField] private float maxAccelForce;
    [SerializeField] private AnimationCurve MaxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 forceScale;

    [Header("Vertical Movement")]
    [SerializeField] private float rideHeight;
    [SerializeField] private float rideSpringStrength;
    [SerializeField] private float rideSpringDamper;
    [SerializeField] private Transform downDirection;
    [SerializeField] private float uprightCorrectionStrength;
    [SerializeField] private float uprightCorrectionDamper;
    private Ray ray;
    private Vector3 offSetDirection;
    private float offSetForce;
    private Rigidbody rb;
    private Vector3 rotAxis;
    private float rotRadians;
    private Vector3 mGoalVel;
    private Vector3 locoMovement;
    private Vector3 lookDirection;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Quaternion deneme = Quaternion.Euler(30, 30, 30);
        Debug.Log(Quaternion.Inverse(deneme));
    }
    void Update()
    {
        ray = new Ray(transform.position, downDirection.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 vel = rb.linearVelocity;
            Vector3 rayDir = downDirection.forward;

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;

            if (hitBody != null)
                otherVel = hitBody.linearVelocity;

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - rideHeight;
            //Debug.Log("Distance: " + x);
            //Debug.Log("relVel: " + relVel);
            //Debug.Log("otherDirVel: " + otherDirVel);
            offSetForce = x * rideSpringStrength - relVel * rideSpringDamper;
            offSetDirection = rayDir;
            //Debug.Log(offSetForce);

        }
        Quaternion upright;
        lookDirection = Camera.main.transform.right * InputManager.Instance.MovementNormalized().x + Camera.main.transform.forward * InputManager.Instance.MovementNormalized().y;
        lookDirection.y = 0;
        if (InputManager.Instance.MovementNormalized().magnitude > 0.001f)
        {
            upright = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
        else
            upright = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        UpdateUprightForce(upright);
    }
    public void Movement(Vector3 directionMovement)
    {
        Vector3 currentVelocity = mGoalVel.normalized;

        float velDot = Vector3.Dot(directionMovement, currentVelocity);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velDot);

        Vector3 goalVel = directionMovement * maxForce;
        mGoalVel = Vector3.MoveTowards(mGoalVel, goalVel, accel * Time.deltaTime);

        locoMovement = (mGoalVel - rb.linearVelocity) / Time.fixedDeltaTime;

        float maxAccel = maxAccelForce * MaxAccelerationForceFactorFromDot.Evaluate(velDot);
        locoMovement = Vector3.ClampMagnitude(locoMovement, maxAccel);
    }
    void FixedUpdate()
    {
        Movement(lookDirection);
        rb.AddForce(offSetDirection * offSetForce);
        rb.AddForce(Vector3.Scale(locoMovement * rb.mass, forceScale));
        rb.AddTorque((rotAxis * (rotRadians * uprightCorrectionStrength)) - (rb.angularVelocity * uprightCorrectionDamper));
    }
    public void UpdateUprightForce(Quaternion uprightsCorrection)
    {
        Quaternion characterRotation = transform.rotation;
        Quaternion toGoal = uprightsCorrection * Quaternion.Inverse(characterRotation);
        if (toGoal.w < 0)
        {
            toGoal.x = -toGoal.x;
            toGoal.y = -toGoal.y;
            toGoal.z = -toGoal.z;
            toGoal.w = -toGoal.w;
        }

        float rogDegress;
        toGoal.ToAngleAxis(out rogDegress, out rotAxis);
        rotAxis.Normalize();
        rotRadians = rogDegress * Mathf.Deg2Rad;


    }

}
