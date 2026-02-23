using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Feeder", menuName = "Create Feeder/New Feeder")]
public class FeederDataSO : ScriptableObject
{
    public float BaseTimer;
    public List<DepthMultiplier> DepthMultiplierList;
    
}

[Serializable]
public class DepthMultiplier
{
    public Depth Depth;
    public float Multiplier;
}
