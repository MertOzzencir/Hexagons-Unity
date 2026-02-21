using UnityEngine;

[RequireComponent(typeof(VerticalMovementController), typeof(HorizontalMovementController), typeof(RotationController))]
public abstract class CarryableMovement : MonoBehaviour, ICarryable
{
    public VerticalMovementController VerticalController { get; set; }
    public RotationController RotationController { get; set; }
    public HorizontalMovementController HorizontalController { get; set; }
    public Vector3 direction { get; set; }
    public Rigidbody rb { get; set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        VerticalController = GetComponent<VerticalMovementController>();
        RotationController = GetComponent<RotationController>();
        HorizontalController = GetComponent<HorizontalMovementController>();
        AutoKinematicManager.Instance.Register(rb);
    }

    public void Carry(Vector3 toPosition)
    {
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
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ActivateMovement(false);
    }

    private void ActivateMovement(bool canCarry)
    {
        VerticalController.enabled = canCarry;
        RotationController.enabled = canCarry;
        HorizontalController.enabled = canCarry;
    }
    public void LocomotionLogic()
    {
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
            VerticalController.VerticalMovement(rb);
            VerticalController.ApplyLogic(rb);
        }
    }
    public virtual void TryToPlaceOn()
    {

    }

    public virtual void OnPicked()
    {
        AutoKinematicManager.Instance.WakeUp(rb);
    }
}
