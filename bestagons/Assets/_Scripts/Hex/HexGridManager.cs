using System;
using System.Collections.Generic;
using UnityEngine;

public class HexGridManager : MonoBehaviour
{
    [Range(0, 30)]
    [SerializeField] private int hexCircleAmount;

    [Range(0, 100)]
    [SerializeField] private float innerSize;

    [Range(0, 100)]
    [SerializeField] private float outerSize;
    [SerializeField] private float height;
    [SerializeField] private Vector3 worldPos;
    [SerializeField] private HexTileDataSO[] hexDatas;
    private Dictionary<HexCoord, HexTile> hexTiles = new Dictionary<HexCoord, HexTile>();
    private HexRenderer hexRenderer;
    void Start()
    {
        hexRenderer = FindAnyObjectByType<HexRenderer>();
    }

    private void HexGeneration()
    {
        if (hexRenderer == null)
            return;
        Debug.Log("Hex Mesh Generation Started");
        hexRenderer.ClearMeshData();
        ClearDict();
        for (int q = -hexCircleAmount; q <= hexCircleAmount; q++)
        {
            for (int r = -hexCircleAmount; r <= hexCircleAmount; r++)
            {
                if (Mathf.Abs(q + r) <= hexCircleAmount)
                {
                    HexCoord coord = new HexCoord(q, r);
                    int random = UnityEngine.Random.Range(0, 2);
                    HexTile tile = new HexTile(coord.GetWorldPosition(outerSize), hexDatas[0]);
                    hexTiles.Add(coord, tile);

                }
            }
        }
        ResourceHexTile normalTile = new ResourceHexTile(new HexCoord(0, 0).GetWorldPosition(outerSize), hexDatas[1]);
        ResourceHexTile normalTile2 = new ResourceHexTile(new HexCoord(1, 2).GetWorldPosition(outerSize), hexDatas[2]);
        hexTiles[new HexCoord(0, 0)] = normalTile;
        hexTiles[new HexCoord(1, 2)] = normalTile2;
        int i = 0;
        foreach (var a in hexTiles)
        {
            hexRenderer.SetVerticesAndTriangles(a.Key.GetWorldPosition(outerSize), i, outerSize, innerSize, height, a.Value.TileData.tileColor);
            i++;
        }
        hexRenderer.GenerateMesh();
    }
    private void ClearDict()
    {
        hexTiles.Clear();
    }

    public HexTile GetHexGridFromWorldPosition(Vector3 position)
    {
        HexCoord currentHex = HexCoord.FromWorldPositionToHex(position, outerSize);
        return hexTiles[currentHex];
    }


    void OnDrawGizmos()
    {
        if (hexTiles == null) return;

        foreach (var a in hexTiles)
        {
            Gizmos.DrawSphere(a.Key.GetWorldPosition(outerSize), 0.2f);
        }
        Gizmos.DrawSphere(worldPos, 0.2f);
    }
    private void OnValidate()
    {
        if (outerSize < innerSize)
            outerSize = innerSize;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this != null)
                HexGeneration();
        };
#endif
    }
}


[Serializable]
public struct HexCoord
{
    public int q;
    public int r;
    private static readonly HexCoord[] directions =
    {
        new HexCoord(1,0),
        new HexCoord(-1,0),
        new HexCoord(0,1),
        new HexCoord(0,-1),
        new HexCoord(1,-1),
        new HexCoord(-1,1)
    };
    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }
    public Vector3 GetWorldPosition(float size)
    {
        float x = Mathf.Sqrt(3) * q * size + Mathf.Sqrt(3) * size / 2f * r;
        float z = 3f * size / 2f * r;
        return new Vector3(x, 0, z);
    }
    public HexCoord GetNeighbor(int index)
    {
        return new HexCoord(q + directions[index].q, r + directions[index].r);
    }

    public static HexCoord FromWorldPositionToHex(Vector3 worldPos, float size)
    {
        float r = worldPos.z / (1.5f * size);
        float q = (worldPos.x - Mathf.Sqrt(3) / 2f * size * r) / (Mathf.Sqrt(3) * size);
        float s = -q - r;

        int roundQ = Mathf.RoundToInt(q);
        int roundR = Mathf.RoundToInt(r);
        int roundS = Mathf.RoundToInt(s);

        float diffQ = Mathf.Abs(roundQ - q);
        float diffR = Mathf.Abs(roundR - r);
        float diffS = Mathf.Abs(roundS - s);

        if (diffQ > diffR && diffQ > diffS)
            roundQ = -roundR - roundS;
        else if (diffR > diffS)
            roundR = -roundQ - roundS;

        return new HexCoord(roundQ, roundR);
    }

    public override bool Equals(object obj)
    {
        if (obj is HexCoord other)
            return q == other.q && r == other.r;
        return false;
    }
    public override int GetHashCode()
    {
        return q * 397 ^ r;
    }

}