using UnityEngine;

[RequireComponent(typeof(VerticalMovementController), typeof(HorizontalMovementController), typeof(RotationController))]
public class InteractionBase : MonoBehaviour, IInteractable
{
    [SerializeField] private LocomotionSO movementData;
    public VerticalMovementController VerticalController { get; set; }
    public RotationController RotationController { get; set; }
    public HorizontalMovementController HorizontalController { get; set; }
    public Vector3 direction { get; set; }
    public Rigidbody rb { get; set; }
    private Vector3 Destination;
    private float verticalMultiplier;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        VerticalController = GetComponent<VerticalMovementController>();
        RotationController = GetComponent<RotationController>();
        HorizontalController = GetComponent<HorizontalMovementController>();
        SetMovementData();
    }
    private void SetMovementData()
    {
        VerticalController.rideHeight = movementData.RideHeight;
        VerticalController.rideSpringDamper = movementData.RideSpringDamper;
        VerticalController.rideSpringStrength = movementData.RideSpringStrength;
        verticalMultiplier = movementData.RideHeightMultiplier;

        HorizontalController.maxForce = movementData.MaxForce;
        HorizontalController.acceleration = movementData.Acceleration;
        HorizontalController.accelerationFactorFromDot = movementData.AccelerationFactorFromDot;
        HorizontalController.maxAccelForce = movementData.MaxAccelForce;
        HorizontalController.MaxAccelerationForceFactorFromDot = movementData.MaxAccelerationForceFactorFromDot;
        HorizontalController.forceScale = movementData.ForceScale;

        RotationController.uprightCorrectionDamper = movementData.UprightCorrectionDamper;
        RotationController.uprightCorrectionStrength = movementData.UprightCorrectionStrength;

        rb.mass = movementData.Mass;
        rb.interpolation = movementData.Interpolate;
        rb.collisionDetectionMode = movementData.CollisionDetection;
        rb.constraints = movementData.Constraints;
        rb.isKinematic = movementData.IsKinematic;
        rb.linearDamping = movementData.LinearDamping;
    }

    public void Carry(Vector3 toPosition)
    {
        Destination = toPosition;
        rb.mass = movementData.Mass;
        toPosition.y = 0;
        Vector3 yNormalized = new Vector3(transform.position.x, 0, transform.position.z);
        if (Vector3.Distance(yNormalized, toPosition) > 0.25f)
        {
            direction = (toPosition - yNormalized).normalized;
        }
        else
        {
            direction = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        ActivateMovement(true);
    }
    public virtual void Drop()
    {
        rb.mass = movementData.Mass * 100f;
        rb.linearDamping = movementData.LinearDamping;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ActivateMovement(false);
        TryToPlace();
    }
    private void TryToPlace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var a in hits)
        {
            if (a.collider.TryGetComponent(out Placeable pickedPlaceable))
            {
                pickedPlaceable.OnPlace(gameObject);
                break;
            }

        }
    }
    private void ActivateMovement(bool canCarry)
    {
        VerticalController.enabled = canCarry;
        RotationController.enabled = canCarry;
        HorizontalController.enabled = canCarry;
    }
    public void LocomotionLogic()
    {

        if ((Destination.y - transform.position.y) > -movementData.RideHeight)
        {
            if (transform.position.y < Destination.y + 1)
            {
                float goalRide = Destination.y + 1f;
                VerticalController.rideHeight = Mathf.Lerp(VerticalController.rideHeight, goalRide, 20f * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, Destination) < 0.1f)
                VerticalController.rideHeight = movementData.RideHeight;
        }
        else
        {
            VerticalController.rideHeight = movementData.RideHeight;
        }

        if (RotationController.enabled && direction != Vector3.zero)
        {
            RotationController.UpdateUprightForce(Quaternion.LookRotation(direction, Vector3.up));
            RotationController.ApplyLogic(rb);
        }
        if (HorizontalController.enabled)
        {
            HorizontalController.HorizontalMovement(direction, rb);
            HorizontalController.ApplyLogic(rb);
        }
        if (VerticalController.enabled)
        {
            VerticalController.VerticalMovement(rb, VerticalController.rideHeight);
            VerticalController.ApplyLogic(rb);
        }
    }
    public virtual void OnPicked()
    {
        if (rb == null)
        {
            Debug.Log("Is Null");
            rb = gameObject.AddComponent<Rigidbody>();
            rb.interpolation = movementData.Interpolate;
            rb.collisionDetectionMode = movementData.CollisionDetection;
            rb.constraints = movementData.Constraints;
            rb.mass = rb.mass;
        }

        rb.isKinematic = false;
    }
}
