using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AutomationArmController : MonoBehaviour
{
    AutomationArm baseArm;
    [SerializeField] private Transform target;
    [SerializeField] private Transform hint;
    [SerializeField] private Transform targetGoal;
    [SerializeField] private Rig rig;

    Materials currentMaterial;
    bool transformMode;
    void Awake()
    {
        baseArm = GetComponent<AutomationArm>();
    }
    void Update()
    {
        if (currentMaterial == null) return;

        Vector3 pivot = transform.position;

        Vector3 currentDir = target.position - pivot;
        Vector3 targetDir = targetGoal.position - pivot;
        currentDir.y = 0;
        targetDir.y = 0;

        float radius = targetDir.magnitude;
        currentDir = currentDir.normalized * radius;

        Vector3 slerpedDir = Vector3.Slerp(currentDir, targetDir, 2f * Time.deltaTime);

        float targetY = targetGoal.position.y;
        float currentY = Mathf.Lerp(target.position.y, targetY, 4f * Time.deltaTime);

        target.position = pivot + slerpedDir + Vector3.up * currentY;

        Vector3 lookDirection = (targetGoal.transform.position - transform.position).normalized;
        Vector3 lookDirection2 = Vector3.Cross(Vector3.up, lookDirection);
        Quaternion lookQuaternion = Quaternion.LookRotation(lookDirection2);
        target.transform.rotation = Quaternion.Lerp(target.transform.rotation, lookQuaternion, 10f * Time.deltaTime);

        float xDistance = target.transform.position.x - transform.position.x;
        float zDistance = target.transform.position.z - transform.position.z;
        Vector3 hintSmooth = new Vector3(transform.position.x - xDistance, target.position.y, transform.position.z - zDistance);
        hint.position = Vector3.MoveTowards(hint.position, hintSmooth, 10f * Time.deltaTime);

        if (Vector3.Distance(target.position, targetGoal.position) < 0.1f)
        {
            if (transformMode)
            {
                if (baseArm.OutputStorage != null)
                {
                    baseArm.OutputSuccess();
                    currentMaterial.transform.parent = target;
                    baseArm.TryToGiveToInput();
                }
            }
            else
            {
                if (baseArm.InputStorage != null)
                {
                    currentMaterial = null;
                    baseArm.InputSuccess();
                    baseArm.TryToTakeFromOutput();
                }
            }
        }
    }
    public void Output(Vector3 destPosition, Materials pickedMaterials, Storage outputStorage)
    {
        enabled = true;
        transformMode = true;
        targetGoal.position = destPosition;
        currentMaterial = pickedMaterials;
    }
    public void Input(Vector3 position, Storage inputStorage)
    {
        transformMode = false;
        targetGoal.position = position;
    }
}
