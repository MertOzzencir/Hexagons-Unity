using UnityEngine;

public class Feeder : MonoBehaviour, ICarryable
{
    private ExtractorBase currentBase;

    public void InitilizeFeeder(ExtractorBase currentBase)
    {
        this.currentBase = currentBase;
        transform.parent = currentBase.FeederPlacement.transform;
        transform.position = currentBase.FeederPlacement.transform.position;
    }
    public void StoreMaterial(Materials material)
    {
        Debug.Log("Material Spawned from" + currentBase.CurrentTile());
        currentBase.BaseStorage.Add(material);
    }
   
}
public enum FeederType { Fast, Slow, Heavy }

