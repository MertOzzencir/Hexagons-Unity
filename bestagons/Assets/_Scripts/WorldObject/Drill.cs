using System.Collections;
using UnityEngine;

public class Drill : MonoBehaviour, ITools
{
    [SerializeField] private float drillTimer = 2f;

    public ExtractorBase CurrentBase{get;set;}
    private ResourceHexTile currentTile;
    private float timer;
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (CurrentBase.BaseStorage != null)
        {
            if (CurrentBase.BaseStorage.IsFull)
                return;
        }
        timer += Time.deltaTime;
        if (timer > drillTimer)
        {
            currentTile.Dig(out Materials currentDiggingMaterial);
            CurrentBase.BaseFeeder.StoreMaterial(currentDiggingMaterial);
            timer = 0f;
        }
    }

    public void InitilizeTools(ExtractorBase extractorBase)
    {
        this.CurrentBase = extractorBase;
        currentTile = CurrentBase.CurrentTile;
        rb.isKinematic = true;
        transform.parent = CurrentBase.DrillPlacement.transform;
        transform.position = CurrentBase.DrillPlacement.transform.position;
    }

    public void Detach()
    {
        enabled = false;
        transform.SetParent(null);
        if (CurrentBase == null) return;
        CurrentBase.RemoveTool(this);
        CurrentBase = null;
    }
    void OnDisable()
    {
        timer = 0;
    }

}
public enum DrillType { Normal, Crusher, Heated }
