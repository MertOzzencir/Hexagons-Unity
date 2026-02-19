using System;
using UnityEditor.Search;
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

    public bool Initialized;

    public void SetResourceTile(ResourceHexTile next)
    {
        currentTile = next;
    }
    public void SetTools(Drill drill = null, Feeder feeder = null, Storage storage = null)
    {
        if (Initialized) return;

        if (BaseDrill == null)
            BaseDrill = drill;

        if (BaseFeeder == null)
            BaseFeeder = feeder;

        if (BaseStorage == null)
            BaseStorage = storage;

        if (BaseDrill != null && BaseFeeder != null && BaseStorage != null)
            InitilizeTools();

    }


    private void InitilizeTools()
    {
        BaseDrill.InitilizeDrill(currentTile, this);
        BaseFeeder.InitilizeFeeder(this);
        InitilizeStorage();

        Initialized = true;
    }
    public void InitilizeStorage()
    {
        BaseStorage.transform.parent = StoragePlacement.transform;
        BaseStorage.transform.position = StoragePlacement.transform.position;
    }

    public Vector3 CurrentTile()
    {
        return currentTile.Center;
    }

}



