using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSO", menuName = "Create Tile Data/Resource")]
public class ResourceHexTileDataSO : HexTileDataSO
{
    public Hardness Hardness;
    public Depth Depth;
    public Materials TileMaterial;
}
