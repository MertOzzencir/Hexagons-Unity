using System;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private LayerMask hexLayerMask;
    private IInteractable currentObject;
    void OnEnable()
    {
        InputManager.OnLeftClick += CarryObject;
        InputManager.OnRightClick += PlaceObject;
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
            if (hit.transform.TryGetComponent(out IInteractable carryable))
            {
                currentObject = carryable;
                currentObject.OnPicked();
            }
        }
    }
    private void PlaceObject(bool obj)
    {
        if (!obj || currentObject == null) return;
        Debug.Log("Found Placeable Object");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var a in hits)
        {
            if (a.collider.TryGetComponent(out Placeable pickedPlaceable))
            {
                MonoBehaviour carryObject = (MonoBehaviour)currentObject;
                Debug.Log(carryObject.name + "" + a.transform.name);
                //if (a.transform.gameObject != carryObject.gameObject)
                //{
                Debug.Log("Trying to Place the Object");
                currentObject.Drop();
                pickedPlaceable.OnPlace(carryObject.gameObject);
                currentObject = null;
                break;
                //}
            }
            else
                Debug.Log("Pass");
        }
    }
}
