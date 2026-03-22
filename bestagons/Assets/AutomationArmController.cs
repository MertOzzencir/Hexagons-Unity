using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class AutomationArmController : MonoBehaviour
{

    [Header("Editor Referances")]
    [SerializeField] private ArmParts[] armParts;
    [SerializeField] private Transform outputPort;

    private Vector3[] neutralRotations;
    private AutomationArm baseArm;
    void Awake()
    {
        neutralRotations = new Vector3[armParts.Length];
        for (int i = 0; i < neutralRotations.Length; i++)
        {
            neutralRotations[i] = armParts[i].ArmObject.localRotation.eulerAngles;
        }
        baseArm = GetComponent<AutomationArm>();
    }

    public IEnumerator RotateToPort(Transform targetGoal, bool sendInput)
    {
        Transform rootPart = armParts[0].ArmObject;

        Vector3 dir = targetGoal.position - rootPart.position;
        dir.y = 0;
        dir.Normalize();
        Quaternion startRot = rootPart.rotation;
        Quaternion targetRot = Quaternion.FromToRotation(rootPart.right, dir) * startRot;

        float timer = 0;
        while (timer < armParts[0].AnimationDuration)
        {
            timer += Time.deltaTime;
            float t = timer / armParts[0].AnimationDuration;
            float curveTimer = armParts[0].AnimationStyle.Evaluate(t);
            rootPart.rotation = Quaternion.LerpUnclamped(startRot, targetRot, curveTimer);
            yield return null;
        }
        rootPart.rotation = targetRot;
        StartCoroutine(RotateMiddleArm(targetGoal, sendInput));
    }
    public IEnumerator RotateMiddleArm(Transform targetGoal, bool sendInput)
    {
        Transform middleArm = armParts[1].ArmObject;
        Vector3 dir = targetGoal.position - middleArm.position;

        Vector3 localDir = middleArm.parent.InverseTransformDirection(dir);
        float targetAngle = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;

        Quaternion startRot = middleArm.localRotation;
        Vector3 startEuler = middleArm.localEulerAngles;
        int randomAngle = UnityEngine.Random.Range(-10, 25);
        Quaternion targetRot = Quaternion.Euler(startEuler.x, targetAngle - randomAngle, startEuler.z);
        float timer = 0;
        while (timer < armParts[1].AnimationDuration)
        {
            timer += Time.deltaTime;
            float t = timer / armParts[1].AnimationDuration;
            float tLerp = armParts[1].AnimationStyle.Evaluate(t);
            middleArm.localRotation = Quaternion.Lerp(startRot, targetRot, tLerp);
            yield return null;
        }
        middleArm.localRotation = targetRot;
        StartCoroutine(RotateGrabArm(targetGoal, sendInput));
    }
    public IEnumerator RotateGrabArm(Transform targetGoal, bool sendInput)
    {
        Transform grabArm = armParts[2].ArmObject;
        Vector3 dir = targetGoal.position - grabArm.position;
        dir.Normalize();

        Debug.Log(dir);
        Vector3 localDir = grabArm.parent.InverseTransformDirection(dir);
        Debug.Log(localDir);
        float targetAngle = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;

        Quaternion startRot = grabArm.localRotation;
        Vector3 startEuler = grabArm.localEulerAngles;
        Quaternion targetRot = Quaternion.Euler(startEuler.x, targetAngle, startEuler.z);

        float timer = 0;
        while (timer < armParts[2].AnimationDuration)
        {
            timer += Time.deltaTime;
            float t = timer / armParts[2].AnimationDuration;
            float tLerp = armParts[2].AnimationStyle.Evaluate(t);
            grabArm.localRotation = Quaternion.Lerp(startRot, targetRot, tLerp);
            yield return null;
        }
        grabArm.localRotation = targetRot;

        if (sendInput)
        {
            baseArm.TakeOutMaterial();
            baseArm.CurrentCarryingMaterial.transform.parent = armParts[2].ArmObject;
            baseArm.CurrentCarryingMaterial.transform.localPosition = Vector3.zero;
            baseArm.CurrentCarryingMaterial.transform.parent = outputPort;
            baseArm.CurrentCarryingMaterial.transform.localPosition = Vector3.zero;
        }
        else
        {
            baseArm.GiveMaterial();
        }

        StartCoroutine(SetNeutral(sendInput));
    }

    public IEnumerator SetNeutral(bool sendInput)
    {
        for (int i = 0; i < armParts.Length; i++)
        {
            Vector3 currentEuler = neutralRotations[i];
            Quaternion startRot = armParts[i].ArmObject.localRotation;
            Quaternion targetRot = Quaternion.Euler(currentEuler.x, currentEuler.y, currentEuler.z);
            float timer = 0;
            while (timer < armParts[i].AnimationDuration)
            {
                timer += Time.deltaTime;
                float t = timer / armParts[i].AnimationDuration;
                float tLerp = armParts[i].AnimationStyle.Evaluate(t);
                armParts[i].ArmObject.localRotation = Quaternion.Lerp(startRot, targetRot, tLerp);
                yield return null;
            }
        }
        baseArm.OutputInputMode(sendInput);
    }
}

[Serializable]
public class ArmParts
{
    public Transform ArmObject;
    public float AnimationDuration;
    public AnimationCurve AnimationStyle;

}

