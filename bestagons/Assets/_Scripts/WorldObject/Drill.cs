using System.Collections;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField] private float drillTimer = 2f;

    private ExtractorBase currentBase;
    private ResourceHexTile currentTile;
    private float timer;
    public void InitilizeDrill(ResourceHexTile current, ExtractorBase currentBase)
    {
        enabled = true;
        currentTile = current;
        this.currentBase = currentBase;
        transform.parent = currentBase.DrillPlacement.transform;
        transform.position = currentBase.DrillPlacement.transform.position;
    }
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > drillTimer)
        {
            currentTile.Dig(out Materials currentDiggingMaterial);
            currentBase.BaseFeeder.StoreMaterial(currentDiggingMaterial);
            timer = 0f;
        }
    }

    void OnDisable()
    {
        timer = 0;
    }
}
public enum DrillType { Normal, Crusher, Heated }
