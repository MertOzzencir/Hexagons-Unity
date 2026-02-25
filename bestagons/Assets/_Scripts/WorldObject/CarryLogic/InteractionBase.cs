using UnityEngine;

[RequireComponent(typeof(VerticalMovementController), typeof(HorizontalMovementController), typeof(RotationController))]
public class InteractionBase : MonoBehaviour, IInteractable
{
    public VerticalMovementController VerticalController { get; set; }
    public RotationController RotationController { get; set; }
    public HorizontalMovementController HorizontalController { get; set; }
    public Vector3 direction { get; set; }
    public Rigidbody rb { get; set; }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        VerticalController = GetComponent<VerticalMovementController>();
        RotationController = GetComponent<RotationController>();
        HorizontalController = GetComponent<HorizontalMovementController>();
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
    public virtual void OnPicked()
    {
        if (rb == null)
        {
            Debug.Log("Is Null");
            rb = gameObject.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.freezeRotation = true;
            rb.mass = 5f;
        }

        rb.isKinematic = false;
    }
}
