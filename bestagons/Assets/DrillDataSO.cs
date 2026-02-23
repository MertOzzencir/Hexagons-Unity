using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drill", menuName = "Create New Drill/New Drill")]
public class DrillDataSO : ScriptableObject
{
    public float BaseTimer;
    public List<HardnessMultiplier> DrillMultiplierList;
}

[Serializable]
public class HardnessMultiplier
{
    public Hardness TileHardness;
    public float Multiplier;
}
