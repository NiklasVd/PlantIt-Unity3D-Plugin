using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlantItBehaviour : ScriptableObject
{
    [Tooltip("Sets the planting mode affecting the associated PlantIt-Unit")]
    public PlantItBehaviourMode plantMode;
    [Tooltip("All plants that are affected by this behaviour")]
    public List<PlantItObjectSlot> plantObjectSlots;
    [Tooltip("The minmal distance to other plants in the PlantIt-Unit")]
    public float plantTargetStrength;
    [Tooltip("The amount of invokations for the planting-routine of all PlantIt-Objects accessed by this PlantIt-Behaviour")]
    public int plantInvokationCalls;
}

[Serializable]
public enum PlantItBehaviourMode
{
    [Tooltip("Only plant at the borders of the PlantIt-Unit")]
    OnlyBorder,
    [Tooltip("Only plant in the center of the PlantIt-Unit)")]
    OnlyCenter,
    [Tooltip("Plants can get planted everywhere")]
    All
}
[Serializable]
public class PlantItObjectSlot
{
    [Tooltip("The intialization chance of this PlantIt-Object to get planted (max. value: 100)")]
    public int plantChance;
    [Tooltip("The maximum height (y-Axis) for planting this PlantIt-Object in the world")]
    public float maxPlantHeight;
    [Tooltip("The PlantIt-Object that should get planted")]
    public PlantItObject plantItObject;
    //public PlantItObject instantiatedPlantItObject; // TODO: Make this uneditable
}
