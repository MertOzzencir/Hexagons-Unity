using UnityEngine;

public class StorageInteractionController : InteractionBase
{
    Storage baseStorage;
    protected override void Start()
    {
        base.Start();
        baseStorage = GetComponent<Storage>();
    }
    public override void OnPicked()
    {
        baseStorage.Detach();
        base.OnPicked();
    }
    public override void Drop()
    {
        base.Drop();
    }

}
