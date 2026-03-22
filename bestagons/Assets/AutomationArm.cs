using System;
using System.Collections;
using UnityEngine;

public class AutomationArm : HexPlaceable
{
    [SerializeField] private Transform outputTransform;
    [SerializeField] private Transform inputTransform;

    private AutomationArmController movementController;
    public Storage OutputStorage { get; private set; }
    public Storage InputStorage { get; private set; }
    public Materials CurrentCarryingMaterial { get; set; }
    void Start()
    {
        movementController = GetComponent<AutomationArmController>();
    }

    public void FindOutputStorage()
    {
        if (OutputStorage == null)
        {
            if (CurrentCarryingMaterial == null)
                OutputStorage = FindStorage(outputTransform.position);

            if (OutputStorage != null)
            {
                OutputStorage.OnAdded += OnOutputHasMaterial;
                TryToTakeFromOutput();
            }
        }
        else
        {
            TryToTakeFromOutput();
        }
    }
    public void FindInputStorage()
    {
        if (InputStorage == null)
        {
            InputStorage = FindStorage(inputTransform.position);
            if (InputStorage != null && CurrentCarryingMaterial != null)
            {
                InputStorage.OnStorageAvaliable += OnInputIsAvaliable;
                TryToGiveToInput();
            }
        }
        else
        {
            TryToGiveToInput();
        }
    }

    public void OutputInputMode(bool obj)
    {
        if (obj)
        {
            FindInputStorage();
        }
        else
        {
            Debug.Log("OutputInput mode false");
            CurrentCarryingMaterial = null;
            FindOutputStorage();
        }
    }
    public void TakeOutMaterial()
    {
        OutputStorage.Remove(CurrentCarryingMaterial);

    }
    public void GiveMaterial()
    {
        InputStorage.Add(CurrentCarryingMaterial);
    }


    private void OnInputIsAvaliable(bool obj)
    {
        if (obj)
            TryToGiveToInput();
    }
    private void OnOutputHasMaterial()
    {
        TryToTakeFromOutput();
    }

    public void TryToTakeFromOutput()
    {
        if (CurrentCarryingMaterial != null) return;

        Materials currentGoal = OutputStorage.GetFirstAvaliableMaterial();
        if (currentGoal != null)
        {
            CurrentCarryingMaterial = currentGoal;
            movementController.StartCoroutine(movementController.RotateToPort(CurrentCarryingMaterial.transform, true));
        }
    }
    public void TryToGiveToInput()
    {
        if (CurrentCarryingMaterial == null) return;
        StorageSlot currentGoal = InputStorage.GetAvaliableSlot();
        Debug.Log("Avaliable Slot?");
        if (currentGoal != null)
        {
            Debug.Log("Found Avaliable Slot");
            movementController.StartCoroutine(movementController.RotateToPort(currentGoal.transform, false));

            //movementController.Input(currentGoal.transform.position, InputStorage);
        }
    }
    private Storage FindStorage(Vector3 position)
    {
        HexTile hex = HexGridManager.Instance.GetHexGridFromWorldPosition(position);

        if (hex == null || hex.PlacedObject == null)
            return null;

        return hex.PlacedObject.GetComponentInChildren<Storage>(); ;
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
        movementController.StopAllCoroutines();
        Storage outputCheck = FindStorage(outputTransform.position);
        if (outputCheck == null)
            OutputStorage = null;

        Storage inputCheck = FindStorage(outputTransform.position);
        if (inputCheck == null)
            InputStorage = null;
        if (CurrentCarryingMaterial != null)
        {
            CurrentCarryingMaterial.transform.parent = null;
            CurrentCarryingMaterial = null;
        }
        movementController.StartCoroutine(movementController.SetNeutral(false));
    }


}

