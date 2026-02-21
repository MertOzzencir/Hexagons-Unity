using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public event Action<bool> OnStorageAvaliable;
    public event Action OnStoragePicked;
    [SerializeField] private Transform Slots;
    public bool IsFull { get; set; }
    public List<StorageSlot> StorageList { get; private set; } = new List<StorageSlot>();
    void Awake()
    {
        for (int i = 0; i < Slots.childCount; i++)
        {
            StorageSlot a = Slots.GetChild(i).GetComponent<StorageSlot>();
            StorageList.Add(a);
        }
    }
    public bool Add(Materials material)
    {
        StorageSlot currentSlot = IsAvaliable();
        if (currentSlot == null)
        {
            OnStorageAvaliable?.Invoke(false);
            IsFull = true;
            return false;
        }

        currentSlot.AddOnSlot(material, this);
        StorageSlot avaliableSlot = IsAvaliable();
        if (avaliableSlot == null)
            OnStorageAvaliable?.Invoke(false);
        return true;
    }
    public bool Remove(Materials material)
    {
        Debug.Log("Remove try");
        foreach (var a in StorageList)
        {
            if (a.SlotMaterial == material)
            {
                Debug.Log("Removed?");
                a.RemoveOnSlot();
                OnStorageAvaliable?.Invoke(true);
                return true;
            }
        }
        return false;
    }
    public StorageSlot IsAvaliable()
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
    public void CarryChild(bool carryState)
    {
        foreach (var a in StorageList)
        {
            if (a.SlotMaterial == null)
                continue;

            Collider c = a.SlotMaterial.GetComponent<Collider>();
            if (c != null)
            {
                c.enabled = carryState;
            }
        }
    }
    public void OnPicked(bool state)
    {
        OnStoragePicked?.Invoke();
        transform.parent = null;
        CarryChild(state);
    }




}
