using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class AutoKinematicManager : MonoBehaviour
{
    public static AutoKinematicManager Instance;
    private List<AutoKinematicData> sceneList = new List<AutoKinematicData>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void FixedUpdate()
    {
        foreach (var a in sceneList)
        {
            if (a.rb == null || a.rb.isKinematic)
                continue;
            if (a.rb.linearVelocity.magnitude < 0.1f && a.rb.angularVelocity.magnitude < 0.1f)
            {
                a.timer += Time.fixedDeltaTime;
                if (a.timer > 2f)
                {
                    a.timer = 0;
                    a.rb.isKinematic = true;
                }
            }
        }
    }
    public void WakeUp(Rigidbody rb)
    {
        foreach (var a in sceneList)
        {
            if (a.rb == rb)
            {
                a.rb.isKinematic = false;
                a.timer = 0;
                break;
            }
        }
    }
    public void Register(Rigidbody rb)
    {
        sceneList.Add(new AutoKinematicData(rb));
    }


}
[Serializable]
public class AutoKinematicData
{
    public Rigidbody rb;
    public float timer;

    public AutoKinematicData(Rigidbody rb)
    {
        this.rb = rb;
        timer = 0f;
    }
}

