using System;
using UnityEngine;

[Serializable]
public class HexTile
{

    public HexTileDataSO TileData;
    public Vector3 Center;
    public bool Occoupied;
    public HexPlaceable PlacedObject;
    public HexTile(Vector3 center, HexTileDataSO data)
    {
        this.Center = center;
        TileData = data;
    }
    public void SetObjectOnTile(HexPlaceable currentObject)
    {
        PlacedObject = currentObject;
        Occoupied = true;
        FireEventToOtherTiles();
    }
    public void OnPickedFromTile()
    {
        PlacedObject = null;
        Occoupied = false;
        FireEventToOtherTiles();
    }
    public void FireEventToOtherTiles()
    {
        HexGridManager.Instance.FindNeighborhoodsToCommunicate(Center);
    }
}

