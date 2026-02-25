using System;
using UnityEngine;

public class StorageSlot : MonoBehaviour
{
    private Storage baseStorage;
    public Materials SlotMaterial { get; private set; }


    void Awake()
    {
        baseStorage = GetComponentInParent<Storage>();
    }
    public void AddOnSlot(Materials slot)
    {
        SlotMaterial = slot;
        SlotMaterial.GetComponent<Collider>().isTrigger = true;
        SlotMaterial.transform.SetParent(transform);
        SlotMaterial.transform.localPosition = Vector3.zero;
        Rigidbody rb = SlotMaterial.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            Destroy(rb);
        }
        SlotMaterial.MaterialStorage = baseStorage;
    }
    public Materials GetMaterial()
    {
        return SlotMaterial;
    }

    public void RemoveOnSlot()
    {
        SlotMaterial.MaterialStorage = null;
        SlotMaterial.transform.parent = null;
        SlotMaterial = null;
    }

}
