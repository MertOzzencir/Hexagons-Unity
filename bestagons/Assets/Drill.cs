using System;
using System.Collections;
using UnityEngine;

public class Drill : MonoBehaviour, ICarryable
{
    private ExtractorBase currentBase;
    private ResourceHexTile currentTile;
    private Coroutine diggingCoroutine;
    public void InitilizeDrill(ResourceHexTile current, ExtractorBase currentBase)
    {
        currentTile = current;
        this.currentBase = currentBase;
        transform.parent = currentBase.DrillPlacement.transform;
        transform.position = currentBase.DrillPlacement.transform.position;
        currentBase.BaseStorage.OnStorageAvaliable += OpenDrill;
        OpenDrill();
    }
    public void OpenDrill()
    {
        if (diggingCoroutine != null) return;
        diggingCoroutine = StartCoroutine(StartDigging());
    }
    IEnumerator StartDigging()
    {
        while (true)
        {
            if (!currentBase.BaseStorage.IsFull)
            {
                currentTile.Dig(out Materials currentDiggingMaterial);
                yield return new WaitForSeconds(1f);
                currentBase.BaseFeeder.StoreMaterial(currentDiggingMaterial);
            }
            else
                break;
        }
        diggingCoroutine = null;
    }

}
public enum DrillType { Normal, Crusher, Heated }
