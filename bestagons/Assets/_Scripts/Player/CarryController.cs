using UnityEngine;

public class CarryController : MonoBehaviour
{
    [SerializeField] private LayerMask hexLayerMask;
    private ICarryable currentObject;
    void OnEnable()
    {
        InputManager.OnLeftClick += CarryObject;
    }

    void Update()
    {
        if (currentObject == null)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hexLayerMask))
        {
            currentObject.Carry(hit.point);
        }
        else
        {
            currentObject.Drop();
            currentObject = null;
        }

    }
    void FixedUpdate()
    {
        if (currentObject == null)
            return;

        currentObject.LocomotionLogic();
    }

    private void CarryObject(bool obj)
    {
        if (!obj) return;

        if (currentObject != null)
        {
            currentObject.Drop();
            currentObject = null;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out ICarryable carryable))
            {
                currentObject = carryable;
                currentObject.OnPicked();
            }
        }
    }
}
