using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Storage : MonoBehaviour, ITools
{
    public event Action<bool> OnStorageAvaliable;
    [SerializeField] private Transform Slots;
    public bool IsFull { get; set; }
    private Rigidbody rb;
    private ExtractorBase CurrentBase;
    public List<StorageSlot> StorageList { get; private set; } = new List<StorageSlot>();
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < Slots.childCount; i++)
        {
            StorageSlot a = Slots.GetChild(i).GetComponent<StorageSlot>();
            a.GetComponent<Placeable>().OnPlaced += RecieveObjectFromSlot;
            StorageList.Add(a);
        }
    }

    private void RecieveObjectFromSlot(Placeable addedOnSlot, GameObject obj)
    {
        if (obj.TryGetComponent(out Materials material))
        {
            if (addedOnSlot.TryGetComponent(out StorageSlot slot))
            {
                if (slot.SlotMaterial == null)
                {
                    Add(material, slot);
                }
            }
        }
    }


    public bool Add(Materials material, StorageSlot slotFromSide = null)
    {

        if (!CheckStorageCapacity(out StorageSlot currentSlot)) return false;
        if (slotFromSide == null)
            currentSlot.AddOnSlot(material);
        else
            slotFromSide.AddOnSlot(material);
        CheckStorageCapacity(out StorageSlot no);
        return true;
    }

    private bool CheckStorageCapacity(out StorageSlot slot)
    {
        slot = null;
        StorageSlot currentSlot = GetAvaliableSlot();
        if (currentSlot == null)
        {
            OnStorageAvaliable?.Invoke(false);
            IsFull = true;
            return false;
        }
        else
        {
            slot = currentSlot;
            return true;
        }
    }
    public int TotalEmptySlot()
    {
        int i = 0;
        foreach (var a in StorageList)
        {
            if (a.SlotMaterial == null)
            {
                i++;
            }
        }
        return i;
    }


    public bool Remove(Materials material)
    {
        Debug.Log("Remove try");
        foreach (var a in StorageList)
        {
            if (a.SlotMaterial == material)
            {
                IsFull = false;
                Debug.Log("Removed?");
                a.RemoveOnSlot();
                OnStorageAvaliable?.Invoke(true);
                return true;
            }
        }
        return false;
    }
    public StorageSlot GetAvaliableSlot()
    {
        foreach (var a in StorageList)
        {
            if (a.SlotMaterial == null)
            {
                return a;
            }
        }
        return null;
    }
    public Materials GetFirstAvaliableMaterial()
    {
        foreach (var a in StorageList)
        {
            Materials avaliableMaterial = a.GetMaterial();
            if (avaliableMaterial != null)
                return avaliableMaterial;
        }
        return null;
    }
    public void ChildrenTriggerCheck(bool carryState)
    {
        foreach (var a in StorageList)
        {
            if (a.SlotMaterial == null)
                continue;

            Collider c = a.SlotMaterial.GetComponent<Collider>();
            if (c != null)
            {
                c.isTrigger = carryState;
            }
        }
    }

    public void InitilizeTools(ExtractorBase extractorBase)
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Destroy(rb);

        GetComponent<Collider>().isTrigger = true;
        CurrentBase = extractorBase;
        transform.parent = CurrentBase.StoragePlacement.transform;
        transform.position = CurrentBase.StoragePlacement.transform.position;
    }

    public void Detach()
    {
        GetComponent<Collider>().isTrigger = false;
        ChildrenTriggerCheck(true);
        transform.SetParent(null);

        if (CurrentBase == null) return;
        CurrentBase.RemoveTool(this);
        CurrentBase.OnStoragePicked();
        CurrentBase = null;
    }
}
