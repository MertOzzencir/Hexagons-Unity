using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private VerticalMovementController verticalMovement;
    private HorizontalMovementController horizontalMovement;
    private RotationController rotationController;

    private Ray ray;
    private Rigidbody rb;

    private Vector3 lookDirection;
    private Quaternion upRight;
    private Camera cam;
    void Awake()
    {
        verticalMovement = GetComponent<VerticalMovementController>();
        horizontalMovement = GetComponent<HorizontalMovementController>();
        rotationController = GetComponent<RotationController>();

        rb = GetComponent<Rigidbody>();
        Quaternion deneme = Quaternion.Euler(30, 30, 30);
        Debug.Log(Quaternion.Inverse(deneme));
        cam = Camera.main;
    }
    void Update()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        Vector3 right = cam.transform.right;
        right.y = 0;
        lookDirection = right * InputManager.Instance.MovementVector().x + forward * InputManager.Instance.MovementVector().y;
        lookDirection.y = 0;
        lookDirection.Normalize();
        if (InputManager.Instance.MovementVector().magnitude > 0.001f)
        {
            upRight = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
        else
            upRight = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    void FixedUpdate()
    {
        verticalMovement.VerticalMovement(rb);
        horizontalMovement.HorizontalMovement(lookDirection, rb);
        rotationController.UpdateUprightForce(upRight);
        
        verticalMovement.ApplyLogic(rb);
        horizontalMovement.ApplyLogic(rb);
        rotationController.ApplyLogic(rb);
    }



}
