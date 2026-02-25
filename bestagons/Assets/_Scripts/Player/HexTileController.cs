using UnityEngine;

public class HexTileController : MonoBehaviour
{
    public static HexTileController Instance;
    [SerializeField] private LayerMask hexGridLayerMask;
    [SerializeField] private LayerMask playerOffSetLayerMask;
    [SerializeField] private GameObject tempIndicator;

    private RaycastHit outerHit;
    private RaycastHit innerHit;

    public HexTile TileInCursor;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out outerHit, Mathf.Infinity, playerOffSetLayerMask))
        {
            if (Physics.Raycast(outerHit.point, Vector3.down, out innerHit, Mathf.Infinity, hexGridLayerMask))
            {
                tempIndicator.SetActive(true);
                TileInCursor = HexGridManager.Instance.GetHexGridFromWorldPosition(innerHit.point);
                //tempIndicator.transform.position = currenTile.Center;
            }
        }
        else
        {
            TileInCursor = null;
            tempIndicator.SetActive(false);
        }
    }



}
