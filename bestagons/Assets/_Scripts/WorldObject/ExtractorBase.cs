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
        CurrentTile = next;
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
            SubscribeStorage();
        }

        TryStartWorking();
    }

    private void TryStartWorking()
    {
        if (BaseDrill != null && BaseFeeder != null && BaseStorage != null)
        {
            BaseDrill.enabled = true;
            BaseFeeder.enabled = true;
            BaseStorage.enabled = true;
        }
    }

    private void SubscribeStorage()
    {
        BaseStorage.OnStorageAvaliable += OnStorageAvaliable;
    }

    private void OnStorageAvaliable(bool available)
    {
        if (BaseDrill != null)
            BaseDrill.enabled = available;
    }

    public void OnStoragePicked()
    {
        BaseStorage.OnStorageAvaliable -= OnStorageAvaliable;
        BaseStorage = null;

        StopWorking();
    }

    public void RemoveTool(ITools tool)
    {
        if (tool is Drill) BaseDrill = null;
        else if (tool is Feeder) BaseFeeder = null;
        StopWorking();
    }
    private void StopWorking()
    {
        if (BaseDrill != null)
            BaseDrill.enabled = false;
        if (BaseFeeder != null)
            BaseFeeder.enabled = false;
    }
}