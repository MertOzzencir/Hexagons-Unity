using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feeder : MonoBehaviour, ITools
{
    [SerializeField] private FeederDataSO feederData;
    public ExtractorBase CurrentBase { get; set; }
    private List<Materials> onWayToStorageMaterials = new List<Materials>();
    Rigidbody rb;
    private float timer;
    private float currentTimer;
    private int reservedSlots;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }
    private void Update()
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
        if (onWayToStorageMaterials.Count != 0)
        {

            timer += Time.deltaTime;
            if (timer < currentTimer)
                return;

            timer = 0;
            if (!CurrentBase.BaseStorage.IsFull)
            {
                Materials currentMaterial = onWayToStorageMaterials[0];
                Materials sa = Instantiate(currentMaterial);
                CurrentBase.BaseStorage.Add(sa);
                onWayToStorageMaterials.Remove(currentMaterial);
                Debug.Log("Sa counter");
                ReserveStock(-1);
            }
            else
                return;
        }
    }
    public void StoreMaterial(Materials material)
    {
        if (CurrentBase.BaseStorage.TotalEmptySlot() - reservedSlots > 0)
        {
            ReserveStock(+1);
            onWayToStorageMaterials.Add(material);
        }
    }

    public void InitilizeTools(ExtractorBase extractorBase)
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Destroy(rb);

        GetComponent<Collider>().isTrigger = true;
        this.CurrentBase = extractorBase;
        transform.parent = CurrentBase.FeederPlacement.transform;
        transform.position = CurrentBase.FeederPlacement.transform.position;
    }

    public float CalculateBaseTimer(Depth tileHardness)
    {
        foreach (var a in feederData.DepthMultiplierList)
        {
            if (a.Depth == tileHardness)
            {
                return feederData.BaseTimer * a.Multiplier;
            }
        }
        return feederData.BaseTimer;
    }
    public void StartTool()
    {
        enabled = true;
        currentTimer = CalculateBaseTimer(CurrentBase.CurrentTile.ResourceData.Depth);
    }
    public void CloseTool()
    {
        enabled = false;
    }
    public void ReserveStock(int i)
    {
        reservedSlots += i;
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
    void OnEnable()
    {
        timer = 0f;
        onWayToStorageMaterials.Clear();
        Debug.Log("Disabled");
        reservedSlots = 0;
    }
}

