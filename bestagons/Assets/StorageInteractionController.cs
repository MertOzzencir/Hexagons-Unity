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
        baseStorage.OnPicked(false);
    }
    public override void Drop()
    {
        base.Drop();
        baseStorage.CarryChild(true);
    }
   
}
