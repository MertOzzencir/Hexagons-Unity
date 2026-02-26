using UnityEngine;

public abstract class HexPlaceable : MonoBehaviour
{
    public HexTile PlaceableTile;
    public abstract void OnAroundTilesChanged();
    public virtual void OnPlacedTile(HexTile tile)
    {

        PlaceableTile = tile;
        PlaceableTile.SetObjectOnTile(this);
        transform.position = tile.Center;
    }
    public virtual void OnPickedFromTile()
    {
        if (PlaceableTile != null)
        {
            PlaceableTile.OnPickedFromTile();
            PlaceableTile = null;
        }
    }
    public void SendInformationToAroundTiles()
    {
        if(PlaceableTile!= null)
        {
            PlaceableTile.FireEventToOtherTiles();
        }
    }

}
