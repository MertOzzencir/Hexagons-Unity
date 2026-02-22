
using System;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public event Action<Placeable, GameObject> OnPlaced;
    public void OnPlace(GameObject placedObject)
    {
        Debug.Log("Storage Slot Shooted Fire");
        OnPlaced?.Invoke(GetComponent<Placeable>(), placedObject);
    }
}
