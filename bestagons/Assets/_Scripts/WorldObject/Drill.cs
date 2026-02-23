using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : MonoBehaviour, ITools
{
    [SerializeField] private DrillDataSO drillData;

    public ExtractorBase CurrentBase { get; set; }
    private ResourceHexTile currentTile;
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
            currentTile.Dig(out Materials currentDiggingMaterial);
            CurrentBase.BaseFeeder.StoreMaterial(currentDiggingMaterial);
            timer = 0f;
        }
    }

    public void InitilizeTools(ExtractorBase extractorBase)
    {
        this.CurrentBase = extractorBase;
        currentTile = CurrentBase.CurrentTile;
        currentTimer = CalculateBaseTimer(currentTile.ResourceData.Hardness);
        rb.isKinematic = true;
        transform.parent = CurrentBase.DrillPlacement.transform;
        transform.position = CurrentBase.DrillPlacement.transform.position;
    }
    public float CalculateBaseTimer(Hardness tileHardness)
    {
        foreach (var a in drillData.DrillMultiplierList)
        {
            if (a.TileHardness == tileHardness)
            {
                return drillData.BaseTimer * a.Multiplier;
            }
        }
        return drillData.BaseTimer;
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
