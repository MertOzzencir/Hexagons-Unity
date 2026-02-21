using UnityEngine;

public class MaterialsCarryMovement : CarryableMovement
{
    Materials baseMaterial;
    protected override void Awake()
    {
        base.Awake();
        baseMaterial = GetComponent<Materials>();
    }
    public override void OnPicked()
    {
        base.OnPicked();
        Debug.Log("Material Picked?");
        if (baseMaterial.MaterialStorage != null)
            baseMaterial.MaterialStorage.Remove(baseMaterial);
    }
    public override void Drop()
    {
        base.Drop();
    }
    public override void TryToPlaceOn()
    {
        base.TryToPlaceOn();
    }
}
