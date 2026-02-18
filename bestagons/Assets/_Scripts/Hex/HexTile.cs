using System;
using UnityEngine;

[Serializable]
public class HexTile
{
    public Vector3 Center;
    public bool Occoupied;
    public HexPlaceable PlacedObject;
    public HexTile(Vector3 center)
    {
        this.Center = center;
    }
}

