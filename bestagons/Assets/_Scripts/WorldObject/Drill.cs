using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : MonoBehaviour, ITools
{
    [SerializeField] private DrillDataSO drillData;

    public ExtractorBase CurrentBase { get; set; }
    private float timer;
    private float currentTimer;
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
            {
                enabled = false;
                return;
            }
        }
        else
            return;
        timer += Time.deltaTime;
        if (timer > currentTimer)
        {
            CurrentBase.CurrentTile.Dig(out Materials currentDiggingMaterial);
            CurrentBase.BaseFeeder.StoreMaterial(currentDiggingMaterial);
            timer = 0f;
        }
    }

    public void InitilizeTools(ExtractorBase extractorBase)
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Destroy(rb);

        GetComponent<Collider>().isTrigger = true;

        this.CurrentBase = extractorBase;
        transform.parent = CurrentBase.DrillPlacement.transform;
        transform.position = CurrentBase.DrillPlacement.transform.position;
    }
    public void CalculateBaseTimer(Hardness tileHardness)
    {
        foreach (var a in drillData.DrillMultiplierList)
        {
            if (a.TileHardness == tileHardness)
            {
                currentTimer = drillData.BaseTimer * a.Multiplier;
            }
        }
        currentTimer = drillData.BaseTimer;
    }
    public void StartTool()
    {
        enabled = true;
        CalculateBaseTimer(CurrentBase.CurrentTile.ResourceData.Hardness);
    }
    public void CloseTool()
    {
        enabled = false;
    }
    public void Detach()
    {
        GetComponent<Collider>().isTrigger = false;
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
