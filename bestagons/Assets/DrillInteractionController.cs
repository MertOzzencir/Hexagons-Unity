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
        base.OnPicked();
        if (baseDrill.CurrentBase != null)
        {
            baseDrill.Detach();
        }
    }
    public override void Drop()
    {
        base.Drop();
    }

}
