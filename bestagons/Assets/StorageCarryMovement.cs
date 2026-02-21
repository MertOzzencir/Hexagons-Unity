using UnityEngine;

public class StorageCarryMovement : CarryableMovement
{
    Storage baseStorage;
    protected override void Awake()
    {
        base.Awake();
        baseStorage = GetComponent<Storage>();
    }
    public override void OnPicked()
    {
        base.OnPicked();
        baseStorage.OnPicked(false);
    }
    public override void Drop()
    {
        base.Drop();
        baseStorage.CarryChild(true);
    }
    public override void TryToPlaceOn()
    {
        base.TryToPlaceOn();
    }
}
