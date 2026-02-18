using UnityEditor.Search;
using UnityEngine;

public class ExtractorBlueprint : HexPlaceable
{
    [SerializeField] private ExtractorSO data;
    [SerializeField] private Transform drillPlacement;
    [SerializeField] private Transform feederPlacement;
    public Drill Drill { get; set; }
    public Feeder Feeder { get; set; }
    private ResourceHexTile currentTile;

    public void SetResourceTile(ResourceHexTile next)
    {
        currentTile = next;
    }
    public void SetTools(Drill drill = null, Feeder feeder = null)
    {
        if (Drill == null)
        {
            Drill = drill;
            Drill.transform.position = drillPlacement.position;
            Drill.transform.parent= drillPlacement;
        }
        if (Feeder == null)
        {
            Feeder = feeder;
            Feeder.transform.position = feederPlacement.position;
            Feeder.transform.parent= feederPlacement;
        }
    }
}



