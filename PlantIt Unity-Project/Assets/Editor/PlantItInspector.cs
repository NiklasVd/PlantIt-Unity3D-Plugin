using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlantItUnit), true)] // editorForChildClasses is only set "true" because if PlantItUnit has no "sealed" modificator anymore its most likely forgotten that this option is turned on
public class PlantItInspector : Editor
{
    public override void OnInspectorGUI()
    {
        PlantItUnit plantItUnitTarget = base.target as PlantItUnit;
        GUILayout.Label("ID: " + plantItUnitTarget.permanentId.ToString());

        GUI.enabled = !plantItUnitTarget.isInitialized;
        if(GUILayout.Button("Plant All"))
        {
            plantItUnitTarget.PlantAll();
        }
        GUI.enabled = plantItUnitTarget.isInitialized;
        if(GUILayout.Button("Clear"))
        {
            plantItUnitTarget.Clear();
        }
        GUI.enabled = true;

        GUI.enabled = false;
        GUILayout.Toggle(plantItUnitTarget.isInitialized, "Is Initialized");
        GUI.enabled = true;

        GUILayout.Space(5);
        base.OnInspectorGUI(); // Also draws the default inspector ("DrawDefaultInspector()")
    }
}
