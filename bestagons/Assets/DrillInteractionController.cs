using UnityEngine;

public class DrillInteractionController : InteractionBase
{
    Drill baseDrill;
    void Awake()
    {
        baseDrill = GetComponent<Drill>();
    }
    public override void OnPicked()
    {
        if (baseDrill.CurrentBase != null)
        {
            baseDrill.Detach();
        }
        base.OnPicked();
    }
    public override void Drop()
    {
        base.Drop();
    }

}
