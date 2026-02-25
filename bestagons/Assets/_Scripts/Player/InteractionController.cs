using System;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private LayerMask hexLayerMask;
    private IInteractable currentObject;
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
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var a in hits)
        {
            if (a.collider.TryGetComponent(out IInteractable carryable))
            {
                currentObject = carryable;
                currentObject.OnPicked();
                break;
            }
        }

    }
}
