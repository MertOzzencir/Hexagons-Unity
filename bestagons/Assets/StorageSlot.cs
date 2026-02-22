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
        SlotMaterial.transform.SetParent(transform);
        SlotMaterial.transform.localPosition = Vector3.zero;
        SlotMaterial.GetComponent<Rigidbody>().isKinematic = true;
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
