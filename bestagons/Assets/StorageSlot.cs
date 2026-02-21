using UnityEngine;

public class StorageSlot : MonoBehaviour
{
    public Materials SlotMaterial { get; private set; }

    public void AddOnSlot(Materials slot, Storage baseStorage)
    {
        Materials m = Instantiate(slot);
        SlotMaterial = m;
        SlotMaterial.transform.SetParent(transform);
        SlotMaterial.transform.localPosition = Vector3.zero;
        SlotMaterial.GetComponent<Rigidbody>().isKinematic = true;
        SlotMaterial.MaterialStorage = baseStorage;
    }
    public Materials GetMaterial()
    {
        return SlotMaterial;
    }
    public void RemoveOnSlot()
    {
        SlotMaterial.MaterialStorage = null;
        SlotMaterial.transform.parent = null;
        SlotMaterial = null;
    }

}
