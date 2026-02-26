using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private LayerMask hexLayerMask;
    private IInteractable currentObject;
    void OnEnable()
    {
        InputManager.OnLeftClick += CarryObject;
    }


    RaycastHit[] results = new RaycastHit[30];
    void Update()
    {
        if (currentObject == null)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hitCount = Physics.RaycastNonAlloc(ray, results);
        System.Array.Sort(results, 0, hitCount, Comparer<RaycastHit>.Create((a, b) => a.distance.CompareTo(b.distance)));
        MonoBehaviour mb = (MonoBehaviour)currentObject;
        for (int i = 0; i < hitCount; i++)
        {
            if (results[i].transform.gameObject != mb.gameObject)
            {
                currentObject.Carry(results[i].point);
                return;
            }
        }

        currentObject.Drop();
        currentObject = null;


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
