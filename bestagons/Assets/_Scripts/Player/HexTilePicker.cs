using System;
using UnityEngine;

public class HexTilePicker : MonoBehaviour
{
    [SerializeField] private LayerMask hexGridLayerMask;
    [SerializeField] private LayerMask playerOffSetLayerMask;
    [SerializeField] private GameObject tempIndicator;
    [SerializeField] private ExtractorBlueprint extractor;
    [SerializeField] private Drill drill;
    [SerializeField] private Feeder feeder;

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
                ExtractorBlueprint ex = Instantiate(extractor);
                rsTile.PlacedObject = ex;
                ex.transform.position = rsTile.Center;
                ex.SetResourceTile(rsTile);
            }
            else
            {
                if (currenTile.PlacedObject is ExtractorBlueprint extractor)
                {
                    Debug.Log("sa?");
                    extractor.SetTools(drill, feeder);
                }
            }
        }
    }


}
