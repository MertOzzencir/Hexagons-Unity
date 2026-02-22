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
        base.OnPicked();
        if (baseFeeder.CurrentBase != null)
        {
            baseFeeder.CurrentBase.BaseFeeder = null;
            baseFeeder.enabled = false;
        }
    }
    public override void Drop()
    {
        base.Drop();
    }

}
