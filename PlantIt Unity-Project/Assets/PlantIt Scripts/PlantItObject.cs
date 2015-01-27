// (c) 2015, Case-o-Matic
// PlantIt Unity3D Plugin

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlantItObject : ScriptableObject
{
    private static System.Random random = new System.Random(DateTime.Now.Millisecond);

    [HideInInspector]
    public readonly string permanentId = Guid.NewGuid().ToString();

    [Tooltip("The GameObject that gets planted")]
    public GameObject plantObject;
    public List<GameObject> instantiatedPlantObjects; // TODO: Make this uneditable
    [Tooltip("Should the startGrowthValue get affected randomly by the startGrowthValueRandomRange?")]
    public bool isRandomRangeActive;
    [Tooltip("Adjusts the rotation of this PlantIt-Object when it gets planted on inclined objects")]
    public bool adjustRotationOnPlanting;
    [Tooltip("Sets a random y-Axis rotation that lets plants look more natural")]
    public bool randomYRotationOnPlanting;
    [Tooltip("The growth of all plants controlled by this PlantIt-Object when the timer executes")]
    public float growthInTime = 0.05f;
    public float maxGrowthValue = 2;
    [Tooltip("The time for the timer that executes the growing of these plants (e.g. 60, every 60 seconds the plants of this PlantIt-Object grow)")]
    public float growthTimerTime = 60;
    public float startGrowthValueRandomRange = 0.35f;

    public PlantItUnit plantItUnit;
    [HideInInspector]
    public float currentMinuteDeltaTime;

    public void Initialize(PlantItUnit plantitunit)
    {
        plantItUnit = plantitunit;
    }
    public GameObject PlantNew(Transform plantunittransform, Vector3 plantposition, Quaternion plantrotation)
    {
        GameObject instantiatedPlantObject = (GameObject)Instantiate(plantObject, plantposition, Quaternion.identity);
        //instantiatedPlantObject.hideFlags = HideFlags.HideInHierarchy; // Is this needed? Probably only blocks additional editing of the plants...

        instantiatedPlantObject.transform.parent = plantunittransform;
        instantiatedPlantObject.transform.rotation = plantrotation;

        Vector3 newLocalScale = instantiatedPlantObject.transform.localScale;
        if (isRandomRangeActive)
        {
            float randomRange = UnityEngine.Random.Range(-startGrowthValueRandomRange, startGrowthValueRandomRange);
            newLocalScale.x += randomRange;
            newLocalScale.y += randomRange;
            newLocalScale.z += randomRange;
        }
        instantiatedPlantObject.transform.localScale = newLocalScale;
        instantiatedPlantObjects.Add(instantiatedPlantObject);

        return instantiatedPlantObject;
    }
    public void Update()
    {
        if (!Mathf.Approximately(growthInTime, 0))
        {
            currentMinuteDeltaTime += Time.deltaTime;
            if (currentMinuteDeltaTime >= growthTimerTime)
            {
                currentMinuteDeltaTime = 0;
                foreach (var instantiatedPlant in instantiatedPlantObjects)
                {
                    if (instantiatedPlant.transform.localScale.y < maxGrowthValue)
                    {
                        instantiatedPlant.transform.localScale = new Vector3(
                            instantiatedPlant.transform.localScale.x + growthInTime,
                            instantiatedPlant.transform.localScale.y + growthInTime,
                            instantiatedPlant.transform.localScale.z + growthInTime);
                    }
                }
            } 
        }
    }
}
