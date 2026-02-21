using System.Data;
using UnityEngine;

public class Feeder : MonoBehaviour
{

    private ExtractorBase currentBase;

    public void InitilizeFeeder(ExtractorBase currentBase)
    {
        enabled = true;
        this.currentBase = currentBase;
        transform.parent = currentBase.FeederPlacement.transform;
        transform.position = currentBase.FeederPlacement.transform.position;
    }
    public void StoreMaterial(Materials material)
    {
        currentBase.BaseStorage.Add(material);
    }


}
public enum FeederType { Fast, Slow, Heavy }

