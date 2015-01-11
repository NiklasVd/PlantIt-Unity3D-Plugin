// (c) 2015, Case-o-Matic
// PlantIt Unity3D Plugin

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public sealed class PlantItUnit : MonoBehaviour
{
    public static List<PlantItUnit> plantItUnits { get; private set; }

    [HideInInspector]
    public string permanentId = Guid.NewGuid().ToString();
    [Tooltip("All the plants that get instantiated randomly.")]
    public PlantItObject[] plants;
    [Tooltip("The range of this PlantIt-Unit in that plants shall get instantiated")]
    public float plantRangeRadius;
    [Tooltip("The amount of plants that shall get instantiated on initialization")]
    public float plantTargetStrength;

    private GameObject[] instantiatedPlantObjects;

    #region Unity Methods
    private void Awake()
    {
        if (plantItUnits == null)
            plantItUnits = new List<PlantItUnit>();
        if(!plantItUnits.Contains(this))
            plantItUnits.Add(this);
    }
    private void Update()
    {
        foreach (var plant in plants)
        {
            plant.Update();
        }
    }
    private void OnDestroy()
    {
        if (plantItUnits.Contains(this))
            plantItUnits.Remove(this);
    }

    private void OnDrawGizmos()
    {
        if (PlantItSettings.Current.drawPlantRangeSpheresGizmos)
        {
            // TODO: Try to draw a PlantIt icon (Assets/Gizmos/PlantIt-Icon.psd)
            //Gizmos.DrawIcon(transform.position, "PlantIt-Icon.psd", false);
            Gizmos.DrawWireSphere(transform.position, plantRangeRadius);
        }
    }
    #endregion

    private void Initialize()
    {

    }
}
