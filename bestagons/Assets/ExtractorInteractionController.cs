using UnityEngine;

public class ExtractorInteractionController : InteractionBase
{
    ExtractorBase baseExtractor;

    void Awake()
    {
        baseExtractor = GetComponent<ExtractorBase>();
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPicked()
    {
        base.OnPicked();
        baseExtractor.OnPickedFromTile();
    }
    public override void Drop()
    {
        base.Drop();
        HexTile pointedTile = HexTileController.Instance.TileInCursor;
        if (pointedTile == null) return;

        if (pointedTile is ResourceHexTile rsTile)
        {
            if (!rsTile.Occoupied)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                baseExtractor.OnPlacedTile(rsTile);
            }
        }
    }
}
