// (c) 2015, Case-o-Matic
// PlantIt Unity3D Plugin

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

public sealed class PlantItUnit : MonoBehaviour
{
    // All PlantIt-Units instantiated or placed in the scene
    public static List<PlantItUnit> plantItUnits { get; private set; }
    private static System.Random random = new System.Random(DateTime.Now.Millisecond);

    // Permanent-ID indicating different PlantIt-Units with the same type of plants
    public readonly string permanentId = Guid.NewGuid().ToString();

    [Tooltip("All the plants that get instantiated randomly.")]
    public PlantItBehaviour behaviour;
    [Tooltip("The range of this PlantIt-Unit in that plants shall get instantiated")]
    public float plantRangeRadius;
    [HideInInspector]
    public bool isInitialized;
    [Tooltip("If this string is not empty, PlantIt-Objects only get planted on GameObjects with this tag")]
    public string plantSpecificationTag;

    // All instantiated plants (Usable after Awake() because all plants get placed randomly at the start of the game)
    //[HideInInspector]
    public List<GameObject> instantiatedPlantGameObjects;

    private Ray plantRay;
    private RaycastHit hit;

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
        // Updates all plants and their GameObject models in the game (instantiatedPlantObjects)
        // For example its growth per minute
        if (isInitialized)
        {
            foreach (var plant in behaviour.plantObjectSlots)
            {
                plant.plantItObject.Update();
            }
        }
    }
    private void OnDestroy()
    {
        if (plantItUnits.Contains(this))
            plantItUnits.Remove(this);
    }

    private void OnDrawGizmos()
    {

        // Draws a sphere with the radius of this PlantIt-Unit area of effect
        if (PlantItSettings.Current.drawPlantRangeSpheresGizmos)
        {
            // TODO: Try to draw a PlantIt icon (Assets/Gizmos/PlantIt-Icon.psd)
            //Gizmos.DrawIcon(transform.position, "PlantIt-Icon.psd", false);
            Gizmos.DrawWireSphere(transform.position, plantRangeRadius);
        }
    }
    #endregion

    public void PlantAll()
    {
        if (isInitialized)
        {
            Debug.Log("You cannot initialize a PlantIt-Unit twice");
            return;
        }

        instantiatedPlantGameObjects = new List<GameObject>();
        List<Vector3> plantedPositions = new List<Vector3>();

        for(int i = 0; i < behaviour.plantInvokationCalls; i++)
        {
            int plantChance = random.Next(0, 100); // Does this really generate integers from 0 to 100?
            foreach (var plantObject in behaviour.plantObjectSlots)
            {
                if(plantChance <= plantObject.plantChance)
                {
                    Vector3 position = transform.position;
                    position.x += UnityEngine.Random.Range(-plantRangeRadius, plantRangeRadius);
                    position.y += transform.position.y;
                    position.z += UnityEngine.Random.Range(-plantRangeRadius, plantRangeRadius);

                    while(Vector3.Distance(position, transform.position) > plantRangeRadius)
                    {
                        position = transform.position;
                        position.x += UnityEngine.Random.Range(-plantRangeRadius, plantRangeRadius);
                        position.y += transform.position.y;
                        position.z += UnityEngine.Random.Range(-plantRangeRadius, plantRangeRadius);
                    }

                    //for (int j = 0; j < behaviour.plantInvokationCalls; j++)
                    //{
                    //    bool renewPos = false;
                    //    foreach (var plantedPosition in plantedPositions)
                    //    {
                    //        if (Vector3.Distance(plantedPosition, position) <= behaviour.plantTargetStrength)
                    //        {
                    //            renewPos = true;
                    //            break;
                    //        }
                    //    }

                    //    if (renewPos)
                    //    {
                    //        position = transform.position;
                    //        position.x += UnityEngine.Random.Range(-plantRangeRadius, plantRangeRadius);
                    //        position.y += transform.position.y;
                    //        position.z += UnityEngine.Random.Range(-plantRangeRadius, plantRangeRadius);
                    //    }
                    //    else
                    //        break;
                    //}

                    Quaternion rotation = Quaternion.identity;

                    #region Raycasting
                    plantRay = new Ray(new Vector3(position.x, position.y + plantRangeRadius, position.z), Vector3.down);
                    if (Physics.Raycast(plantRay, out hit, plantRangeRadius * 2))
                    {
                        if (plantSpecificationTag != "")
                        {
                            if (!hit.collider.CompareTag(plantSpecificationTag))
                                continue;
                        }

                        position.y = hit.point.y;
                    }
                    //else
                    //{
                    //    plantRay = new Ray(new Vector3(position.x, -(position.y + plantRangeRadius), position.z), Vector3.up);
                    //    if (Physics.Raycast(plantRay, out hit, 100))
                    //    {
                    //        if (plantSpecificationTag != "")
                    //        {
                    //            if (!hit.collider.CompareTag(plantSpecificationTag))
                    //                continue;
                    //        }
                    //        else
                    //            position.y = hit.point.y;
                    //    }
                    //}
                    #endregion

                    if (position.y > plantObject.maxPlantHeight)
                        continue; // Really just dont plant this plant?
                    if (plantObject.plantItObject.randomYRotationOnPlanting)
                        rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-360, 360), 0);
                    if (plantObject.plantItObject.adjustRotationOnPlanting)
                        rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                    GameObject plantGo = plantObject.plantItObject.PlantNew(transform, position, rotation);

                    instantiatedPlantGameObjects.Add(plantGo);
                    plantedPositions.Add(new Vector3(position.x, position.y, position.z));
                }
            }

            AssetDatabase.SaveAssets();
            isInitialized = true;
        }
    }
    public void Clear()
    {
        if(!isInitialized)
        {
            Debug.Log("You cannot clear a PlantIt-Unit twice");
            return;
        }

        foreach (var instantiatedPlant in instantiatedPlantGameObjects)
        {
            DestroyImmediate(instantiatedPlant);
        }
        instantiatedPlantGameObjects.Clear();

        EditorUtility.UnloadUnusedAssets(); // Is this really needed?
        isInitialized = false;
    }
}
