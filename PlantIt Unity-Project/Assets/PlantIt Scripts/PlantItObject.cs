// (c) 2015, Case-o-Matic
// PlantIt Unity3D Plugin

using UnityEngine;
using System.Collections;
using System;

public class PlantItObject : ScriptableObject
{
    private static System.Random random = new System.Random(DateTime.Now.Millisecond);

    [HideInInspector]
    public string permanentId = Guid.NewGuid().ToString();

    [Tooltip("The GameObject that gets planted")]
    public GameObject plantObject;
    [HideInInspector]
    public GameObject instantiatedPlantObject;
    [HideInInspector]
    public bool isPlanted;
    [Tooltip("Should the startGrowthValue get affected randomly by the startGrowthValueRandomRange?")]
    public bool isRandomRangeActive;
    public float growthPerMinute = 0.05f, startGrowthValue = 1, maxGrowthValue = 2;
    public float startGrowthValueRandomRange = 0.35f;

    [HideInInspector]
    public float currentMinuteDeltaTime;

    public void Initialize(Transform plantunittransform, Vector3 plantposition, float plantrotation)
    {
        if(!isPlanted)
        {
            instantiatedPlantObject = (GameObject)Instantiate(plantObject, plantposition, Quaternion.identity);
            instantiatedPlantObject.transform.parent = plantunittransform;
            instantiatedPlantObject.transform.rotation = Quaternion.Euler(
                instantiatedPlantObject.transform.rotation.x,
                plantrotation,
                instantiatedPlantObject.transform.rotation.z);

            float newScaleX = startGrowthValue, newScaleY = startGrowthValue, newScaleZ = startGrowthValue;
            if (isRandomRangeActive)
            {
                newScaleX += float.Parse((random.NextDouble() * startGrowthValueRandomRange).ToString());
                newScaleY += float.Parse((random.NextDouble() * startGrowthValueRandomRange).ToString());
                newScaleZ += float.Parse((random.NextDouble() * startGrowthValueRandomRange).ToString());
            }
            instantiatedPlantObject.transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);

            isPlanted = true;
        }
    }
    public void Update()
    {
        if (isPlanted)
        {
            if (plantObject.transform.localScale.x < maxGrowthValue)
            {
                currentMinuteDeltaTime += Time.deltaTime;
                if (currentMinuteDeltaTime >= 60)
                {
                    Vector3 currentGrowthVector = instantiatedPlantObject.transform.localScale;
                    plantObject.transform.localScale = new Vector3(
                        currentGrowthVector.x + growthPerMinute,
                        currentGrowthVector.y + growthPerMinute,
                        currentGrowthVector.z + growthPerMinute);
                }
            }
        }
    }
}
