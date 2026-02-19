using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public event Action OnStorageAvaliable;
    [SerializeField] private int capacity;
    [SerializeField] private List<StorageSlot> debugView = new List<StorageSlot>();
    [SerializeField] private Materials tempMaterial;
    public Dictionary<Materials, int> storage = new Dictionary<Materials, int>();
    public bool IsFull { get; set; }

    public bool Add(Materials material, int amount = 1)
    {
        if (GetTotalCount(amount) > capacity)
        {
            IsFull = true;
            return false;
        }
        IsFull = false;
        if (storage.ContainsKey(material))
            storage[material] += amount;
        else
            storage[material] = amount;

        UpdateDebugView();
        return true;
    }
    public bool Remove(Materials material, int amount = 1)
    {
        if (!storage.ContainsKey(material) || storage[material] < amount)
            return false;

        storage[material] -= amount;
        if (storage[material] <= 0)
            storage.Remove(material);

        if (IsFull)
        {
            IsFull = false;
            OnStorageAvaliable?.Invoke();
        }

        UpdateDebugView();
        return true;
    }

    [ContextMenu("Delete Item")]
    public void TempRemove()
    {
        Remove(tempMaterial, 5);
    }
    public int GetTotalCount(int amount)
    {
        int timer = amount;
        foreach (var a in storage)
        {
            timer += a.Value;
        }
        Debug.Log(timer);
        return timer;
    }
    private void UpdateDebugView()
    {
        debugView.Clear();
        foreach (var pair in storage)
        {
            debugView.Add(new StorageSlot { material = pair.Key, amount = pair.Value });
        }
    }


}
[System.Serializable]
public struct StorageSlot
{
    public Materials material;
    public int amount;
}
