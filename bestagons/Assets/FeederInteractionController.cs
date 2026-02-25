using UnityEngine;

public class FeederInteractionController : InteractionBase
{
    Feeder baseFeeder;
    void Awake()
    {
        baseFeeder = GetComponent<Feeder>();
    }
    public override void OnPicked()
    {
        if (baseFeeder.CurrentBase != null)
        {
            baseFeeder.Detach();
        }
        base.OnPicked();
    }
    public override void Drop()
    {
        base.Drop();
    }

}
