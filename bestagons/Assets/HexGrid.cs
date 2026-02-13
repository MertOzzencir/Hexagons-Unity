using System;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private int radius;
    [SerializeField] private Vector3 worldPos;
    private Dictionary<GameObject, HexCoord> hexGrids = new Dictionary<GameObject, HexCoord>();
    void Start()
    {
        for (int q = -radius; q <= radius; q++)
        {
            for (int r = -radius; r <= radius; r++)
            {
                if (Mathf.Abs(q + r) <= radius)
                {
                    HexCoord currentHex = new HexCoord(q, r);
                    GameObject name = Instantiate(gridPrefab);
                    name.transform.position = currentHex.GetWorldPosition();
                    name.name = $"HexCoard[{currentHex.q},{currentHex.r}]";
                    hexGrids.Add(name, currentHex);
                }
            }
        }
    }
    [ContextMenu("Test")]
    public void TestWorldPosition()
    {
        HexCoord result = HexCoord.FromWorldPositionToHex(worldPos);
        Debug.Log($"HexCoard[{result.q},{result.r}]");
    }

    void OnDrawGizmos()
    {
        if (hexGrids == null) return;

        foreach (var a in hexGrids)
        {
            Gizmos.DrawSphere(a.Value.GetWorldPosition(), 0.2f);
        }
        Gizmos.DrawSphere(worldPos, 0.2f);
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
    public Vector3 GetWorldPosition()
    {
        float x = Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2f * r;
        float z = 3f / 2f * r;
        return new Vector3(x, 0, z);
    }
    public HexCoord GetNeighbor(int index)
    {
        return new HexCoord(q + directions[index].q, r + directions[index].r);
    }

    public static HexCoord FromWorldPositionToHex(Vector3 worldPos)
    {
        float r = worldPos.z / 1.5f;
        float q = (worldPos.x - Mathf.Sqrt(3) / 2f * r) / Mathf.Sqrt(3);
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

}