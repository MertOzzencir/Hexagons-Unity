using System;
using UnityEngine;

public class ExtractorBase : HexPlaceable
{
    public Transform DrillPlacement;
    public Transform FeederPlacement;
    public Transform StoragePlacement;

    public Drill BaseDrill { get; private set; }
    public Feeder BaseFeeder { get; private set; }
    public Storage BaseStorage { get; private set; }
    public ResourceHexTile CurrentTile { get; private set; }

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Placeable placeable))
                placeable.OnPlaced += OnToolPlaced;
        }
    }

    public void SetResourceTile(ResourceHexTile next)
    {
        PlaceableTile = next;
    }

    private void OnToolPlaced(Placeable placeable, GameObject obj)
    {
        if (obj.TryGetComponent(out Drill drill) && BaseDrill == null)
        {
            BaseDrill = drill;
            BaseDrill.InitilizeTools(this);
        }
        else if (obj.TryGetComponent(out Feeder feeder) && BaseFeeder == null)
        {
            BaseFeeder = feeder;
            BaseFeeder.InitilizeTools(this);
        }
        else if (obj.TryGetComponent(out Storage storage) && BaseStorage == null)
        {
            BaseStorage = storage;
            BaseStorage.InitilizeTools(this);
            BaseStorage.OnStorageAvaliable += ToolsActiveMode;
        }

        SendInformationToAroundTiles();
        TryStartWorking();
    }

    public void TryStartWorking()
    {
        if (BaseDrill != null && BaseFeeder != null && BaseStorage != null)
        {
            if (PlaceableTile != null)
            {
                if (!BaseDrill.enabled)
                    BaseDrill.StartTool();
                if (!BaseFeeder.enabled)
                    BaseFeeder.StartTool();
            }
        }
    }

    private void ToolsActiveMode(bool available)
    {
        if (available)
        {
            TryStartWorking();
        }
        else
        {
            if (BaseDrill != null)
                BaseDrill.CloseTool();
            if (BaseFeeder != null)
                BaseFeeder.CloseTool();
        }
    }

    public void OnStoragePicked()
    {
        BaseStorage.OnStorageAvaliable -= ToolsActiveMode;
        BaseStorage = null;

        ToolsActiveMode(false);
    }

    public void RemoveTool(ITools tool)
    {
        if (tool is Drill) BaseDrill = null;
        else if (tool is Feeder) BaseFeeder = null;
        SendInformationToAroundTiles();
        ToolsActiveMode(false);
    }
    public override void OnPickedFromTile()
    {
        base.OnPickedFromTile();
        ToolsActiveMode(false);

    }
    public override void OnPlacedTile(HexTile tile)
    {
        base.OnPlacedTile(tile);
        CurrentTile = (ResourceHexTile)tile;
        SetResourceTile(CurrentTile);
        TryStartWorking();
    }

    public override void OnAroundTilesChanged()
    {
        Debug.Log("Extractor Listening Tiles");
    }
}