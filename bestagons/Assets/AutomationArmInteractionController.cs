using UnityEngine;

public class AutomationArmInteractionController : InteractionBase
{
    AutomationArm automationArm;
    void Awake()
    {
        automationArm = GetComponent<AutomationArm>();
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPicked()
    {
        base.OnPicked();
        automationArm.OnPickedFromTile();
    }
    public override void Drop()
    {
        base.Drop();
        HexTile pointedTile = HexTileController.Instance.TileInCursor;
        if (pointedTile == null) return;

        if (pointedTile is HexTile rsTile)
        {
            //if(extractorSO.
            if (!rsTile.Occoupied)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                automationArm.OnPlacedTile(rsTile);
            }
        }
    }
}
