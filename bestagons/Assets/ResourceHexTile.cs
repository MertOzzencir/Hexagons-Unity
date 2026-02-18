using UnityEngine;

public class ResourceHexTile : HexTile
{
    public Hardness Hardness;
    public Depth Depth;
    public ResourceHexTile(Vector3 center, Hardness hardness, Depth depth) : base(center)
    {
        Hardness = hardness;
        Depth = depth;
    }

    public void Dig()
    {
        Debug.Log("Digging");
    }
}
public enum Hardness { Soft, Hard, Crystal }
public enum Depth { Shallow, Medium, Deep }
