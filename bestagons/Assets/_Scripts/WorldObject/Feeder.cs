using System.Data;
using UnityEngine;

public class Feeder : MonoBehaviour, ITools
{

    public ExtractorBase CurrentBase{get;set;}
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StoreMaterial(Materials material)
    {
        Materials sa = Instantiate(material);
        CurrentBase.BaseStorage.Add(sa);
    }

    public void InitilizeTools(ExtractorBase extractorBase)
    {
        rb.isKinematic = true;
        this.CurrentBase = extractorBase;
        transform.parent = CurrentBase.FeederPlacement.transform;
        transform.position = CurrentBase.FeederPlacement.transform.position;
    }
    public void Detach()
    {
        enabled = false;
        transform.SetParent(null);
        if(CurrentBase == null) return;
        CurrentBase.RemoveTool(this);
        CurrentBase = null;
    }
}
public enum FeederType { Fast, Slow, Heavy }

