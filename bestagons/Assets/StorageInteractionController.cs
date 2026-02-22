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
        base.OnPicked();
        baseStorage.Detach();
    }
    public override void Drop()
    {
        base.Drop();
        baseStorage.ChildrenCollision(true);
    }

}
