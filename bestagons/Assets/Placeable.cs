
using System;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public event Action<Placeable, GameObject> OnPlaced;
  
    public void OnPlace(GameObject placedObject)
    {
        OnPlaced?.Invoke(this, placedObject);
    }
}
