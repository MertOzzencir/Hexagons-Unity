using UnityEngine;

public class ResourceHexTile : HexTile
{
    ResourceHexTileDataSO ResourceData => (ResourceHexTileDataSO)TileData;
    public ResourceHexTile(Vector3 center, HexTileDataSO tileData) : base(center, tileData)
    {
        TileData = tileData;
    }

    public void Dig(out Materials digMaterial)
    {
        digMaterial = ResourceData.TileMaterial;
    }
}
public enum Hardness { Soft, Hard, Crystal }
public enum Depth { Shallow, Medium, Deep }
