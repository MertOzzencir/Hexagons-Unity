using System;
using UnityEngine;

public class HexTilePicker : MonoBehaviour
{
    [SerializeField] private LayerMask hexGridLayerMask;
    [SerializeField] private LayerMask playerOffSetLayerMask;
    [SerializeField] private GameObject tempIndicator;
    [SerializeField] private ExtractorBase extractor;
    [SerializeField] private Drill drillPrefab;
    [SerializeField] private Feeder feederPrefab;
    [SerializeField] private Storage storagePrefab;

    private HexGridManager gridManager;
    private RaycastHit outerHit;
    private RaycastHit innerHit;

    HexTile currenTile;
    void Start()
    {
        gridManager = FindAnyObjectByType<HexGridManager>();
        InputManager.OnLeftClick += PlacementTest;
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out outerHit, Mathf.Infinity, playerOffSetLayerMask))
        {
            if (Physics.Raycast(outerHit.point, Vector3.down, out innerHit, Mathf.Infinity, hexGridLayerMask))
            {
                tempIndicator.SetActive(true);
                currenTile = gridManager.GetHexGridFromWorldPosition(innerHit.point);
                tempIndicator.transform.position = currenTile.Center;
            }
        }
        else
        {
            currenTile = null;
            tempIndicator.SetActive(false);
        }
    }
    private void PlacementTest(bool obj)
    {
        if (currenTile == null)
            return;

        if (obj && currenTile is ResourceHexTile rsTile)
        {
            //if(extractorSO.
            if (!rsTile.Occoupied)
            {
                rsTile.Occoupied = true;
                ExtractorBase ex = Instantiate(extractor);
                rsTile.PlacedObject = ex;
                ex.transform.position = rsTile.Center;
                ex.SetResourceTile(rsTile);
            }
            else
            {
                if (currenTile.PlacedObject is ExtractorBase extractor)
                {
                    if (extractor.Initialized)
                        return;
                    Debug.Log("sa?");
                    Drill currentDrill = Instantiate(drillPrefab);
                    Feeder currentFeeder = Instantiate(feederPrefab);
                    Storage currentStorage = Instantiate(storagePrefab);
                    extractor.SetTools(currentDrill, currentFeeder,currentStorage);
                }
            }
        }
    }


}
