using UnityEngine;

public class AutomationArm : HexPlaceable
{
    [SerializeField] private Transform outputTransform;
    [SerializeField] private Transform inputTransform;
    private AutomationArmController movementController;
    public Storage OutputStorage { get; private set; }
    public Storage InputStorage { get; private set; }
    public Materials CurrentCarryingMaterial;
    void Start()
    {
        movementController = GetComponent<AutomationArmController>();
    }
    void Update()
    {
        if (CurrentCarryingMaterial == null)
            TryToTakeFromOutput();
    }

    public void FindStorages()
    {
        OutputStorage = FindStorageAtPosition(outputTransform.position);
        InputStorage = FindStorageAtPosition(inputTransform.position);
    }
    private Storage FindStorageAtPosition(Vector3 position)
    {
        HexTile hex = HexGridManager.Instance.GetHexGridFromWorldPosition(position);

        if (hex == null || hex.PlacedObject == null)
            return null;

        return hex.PlacedObject.GetComponentInChildren<Storage>();
    }
    public void TryToTakeFromOutput()
    {
        if (CurrentCarryingMaterial != null) return;

        if (OutputStorage != null)
        {
            Materials currentGoal = OutputStorage.GetFirstAvaliableMaterial();
            if (currentGoal != null)
            {
                CurrentCarryingMaterial = currentGoal;
                movementController.Output(currentGoal.transform.position, currentGoal, OutputStorage);
            }
        }
        else
        {
            FindStorages();
        }
    }
    public void TryToGiveToInput()
    {
        if (InputStorage != null)
        {
            StorageSlot currentGoal = InputStorage.GetAvaliableSlot();
            Debug.Log("Avaliable Slot?");
            if (currentGoal != null)
            {
                Debug.Log("Found Avaliable Slot");
                movementController.Input(currentGoal.transform.position, InputStorage);
            }
        }
    }
    public void OutputSuccess()
    {
        OutputStorage.Remove(CurrentCarryingMaterial);
    }
    public void InputSuccess()
    {
        InputStorage.Add(CurrentCarryingMaterial);
        CurrentCarryingMaterial = null;
    }
    public override void OnPickedFromTile()
    {
        base.OnPickedFromTile();
    }
    public override void OnPlacedTile(HexTile tile)
    {
        base.OnPlacedTile(tile);
        FindStorages();
    }

    public override void OnAroundTilesChanged()
    {
        Debug.Log("Automotion Arm Listens");
        FindStorages();
    }
}
