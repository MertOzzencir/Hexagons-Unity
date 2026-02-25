using UnityEngine;

public class AutomationArmController : MonoBehaviour
{
    AutomationArm baseArm;
    [SerializeField] private Transform target;
    [SerializeField] private Transform hint;
    [SerializeField] private Transform targetGoal;

    Storage outputStorage;
    Storage inputStorage;
    Materials currentMaterial;
    bool transformMode;
    void Awake()
    {
        baseArm = GetComponent<AutomationArm>();
    }
    void Update()
    {
        if (currentMaterial == null) return;

        Vector3 lookDirection = (targetGoal.transform.position - transform.position).normalized;
        Vector3 lookDirection2 = Vector3.Cross(Vector3.up, lookDirection);  //Fix the forward issue
        Quaternion lookQuaternion = Quaternion.LookRotation(lookDirection2);
        target.position = Vector3.MoveTowards(target.position, targetGoal.position, 2f * Time.deltaTime);
        target.transform.rotation = Quaternion.Lerp(target.transform.rotation, lookQuaternion, 10f * Time.deltaTime);

        float xDistance = target.transform.position.x - transform.position.x;
        float zDistance = target.transform.position.z - transform.position.z;
        Vector3 hintSmooth = new Vector3(transform.position.x - xDistance, target.position.y, transform.position.z - zDistance);
        hint.position = Vector3.MoveTowards(hint.position, hintSmooth, 10f * Time.deltaTime);

        if (Vector3.Distance(target.position, targetGoal.position) < 0.1f)
        {
            if (transformMode)
            {
                outputStorage.Remove(currentMaterial);
                currentMaterial.transform.parent = target;
                baseArm.TryToGiveToInput();
            }
            else
            {
                if (inputStorage != null)
                {
                    inputStorage.Add(currentMaterial);
                    currentMaterial = null;
                    inputStorage = null;
                }
            }
        }

    }
    public void Output(Vector3 destPosition, Materials pickedMaterials, Storage outputStorage)
    {
        transformMode = true;
        this.outputStorage = outputStorage;
        currentMaterial = pickedMaterials;
        enabled = true;
        targetGoal.position = destPosition;
    }
    public void Input(Vector3 position, Storage inputStorage)
    {
        this.inputStorage = inputStorage;
        transformMode = false;
        targetGoal.position = position;
    }
}
