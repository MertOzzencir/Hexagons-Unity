using System.Data;
using UnityEngine;

public class Feeder : MonoBehaviour
{

    public ExtractorBase CurrentBase;
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void InitilizeFeeder(ExtractorBase currentBase)
    {
        rb.isKinematic = true;
        this.CurrentBase = currentBase;
        transform.parent = currentBase.FeederPlacement.transform;
        transform.position = currentBase.FeederPlacement.transform.position;
    }
    public void StoreMaterial(Materials material)
    {
        Materials sa = Instantiate(material);
        CurrentBase.BaseStorage.Add(sa);
    }
}
public enum FeederType { Fast, Slow, Heavy }

