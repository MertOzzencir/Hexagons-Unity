using System;
using UnityEditor.Animations;
using UnityEngine;

public class ExtractorBase : HexPlaceable
{
    public Transform DrillPlacement;
    public Transform FeederPlacement;
    public Transform StoragePlacement;
    public Drill BaseDrill { get; set; }
    public Feeder BaseFeeder { get; set; }
    public Storage BaseStorage { get; set; }
    private ResourceHexTile currentTile;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Placeable placeable))
            {
                placeable.OnPlaced += GetTools;
            }
        }
    }

    private void GetTools(Placeable placeable, GameObject obj)
    {
        Drill drill = obj.GetComponent<Drill>();
        Feeder feeder = obj.GetComponent<Feeder>();
        Storage storage = obj.GetComponent<Storage>();
        SetTools(drill, feeder, storage);
    }

    public void SetResourceTile(ResourceHexTile next)
    {
        currentTile = next;
    }
    public void SetTools(Drill drill = null, Feeder feeder = null, Storage storage = null)
    {

        if (drill != null && BaseDrill == null)
        {
            BaseDrill = drill;
            BaseDrill.enabled = false;
            BaseDrill.InitilizeDrill(currentTile, this);
        }

        if (feeder != null && BaseFeeder == null)
        {
            BaseFeeder = feeder;
            BaseFeeder.enabled = false;
            BaseFeeder.InitilizeFeeder(this);
        }

        if (storage != null && BaseStorage == null)
        {
            BaseStorage = storage;
            InitilizeStorage();
        }

        if (BaseDrill != null && BaseFeeder != null && BaseStorage != null)
            InitilizeTools();
    }

    private void InitilizeTools()
    {
        BaseDrill.enabled = true;
        BaseFeeder.enabled = true;
    }
    public void InitilizeStorage()
    {
        BaseStorage.GetComponent<Rigidbody>().isKinematic = true;
        BaseStorage.transform.parent = StoragePlacement.transform;
        BaseStorage.transform.position = StoragePlacement.transform.position;
        BaseStorage.OnStoragePicked += OnStoragePicked;
        BaseStorage.OnStorageAvaliable += OnStorageAvaliable;
    }

    private void OnStorageAvaliable(bool obj)
    {
        BaseDrill.enabled = obj;
    }

    public void OnStoragePicked()
    {
        BaseStorage.OnStoragePicked -= OnStoragePicked;
        BaseStorage.OnStorageAvaliable -= OnStorageAvaliable;
        BaseDrill.enabled = false;
        BaseStorage = null;
    }

    public Vector3 CurrentTile()
    {
        return currentTile.Center;
    }

}



