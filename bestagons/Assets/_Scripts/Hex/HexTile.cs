using System;
using UnityEngine;

[Serializable]
public class HexTile
{
    public HexTileDataSO TileData;
    public Vector3 Center;
    public bool Occoupied;
    public HexPlaceable PlacedObject;
    public HexTile(Vector3 center,HexTileDataSO data)
    {
        this.Center = center;
        TileData = data;
    }
}

