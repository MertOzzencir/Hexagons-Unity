using UnityEngine;

public class AutomationArm : HexPlaceable
{
    [SerializeField] private Transform outputTransform;
    [SerializeField] private Transform inputTransform;
    private AutomationArmController movementController;
    private Storage outputStorage;
    private Storage inputStorage;
    void Start()
    {
        movementController = GetComponent<AutomationArmController>();
    }

    public void FindOutputStorage()
    {
        outputStorage = FindStorageAtPosition(outputTransform.position);
        inputStorage = FindStorageAtPosition(inputTransform.position);
        TryToTakeFromOutput();
    }
    private Storage FindStorageAtPosition(Vector3 position)
    {
        HexTile hex = HexGridManager.Instance.GetHexGridFromWorldPosition(position);

        if (hex == null || hex.PlacedObject == null)
            return null;

        return hex.PlacedObject.GetComponentInChildren<Storage>();
    }
    private void TryToTakeFromOutput()
    {
        if (outputStorage != null)
        {
            Materials currentGoal = outputStorage.GetFirstAvaliableMaterial();
            if (currentGoal != null)
            {
                movementController.Output(currentGoal.transform.position, currentGoal, outputStorage);
            }
        }
    }
    public void TryToGiveToInput()
    {
        if (inputStorage != null)
        {
            StorageSlot currentGoal = inputStorage.GetAvaliableSlot();
            Debug.Log("Avaliable Slot?");
            if (currentGoal != null)
            {
                Debug.Log("Found Avaliable Slot");
                movementController.Input(currentGoal.transform.position, inputStorage);
            }
        }
    }
    public override void OnPickedFromTile()
    {
        base.OnPickedFromTile();
    }
    public override void OnPlacedTile(HexTile tile)
    {
        base.OnPlacedTile(tile);
        FindOutputStorage();
    }

    public override void OnAroundTilesChanged()
    {
        FindOutputStorage();
    }
}
