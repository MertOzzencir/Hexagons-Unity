using UnityEngine;

public class MaterialsInteractionController : InteractionBase
{
    Materials baseMaterial;

    protected override void Start()
    {
        base.Start();
        baseMaterial = GetComponent<Materials>();
    }
    public override void OnPicked()
    {
        base.OnPicked();
        baseMaterial.GetComponent<Collider>().isTrigger = false;
        Debug.Log("Material Picked?");
        if (baseMaterial.MaterialStorage != null)
            baseMaterial.MaterialStorage.Remove(baseMaterial);
    }
    public override void Drop()
    {
        base.Drop();
    }

}
