// (c) 2015, Case-o-Matic
// PlantIt Unity3D Plugin

using UnityEngine;
using System.Collections;
using UnityEditor;

// Is this really needed?
public class PlantItEditorWindow : EditorWindow
{
    [MenuItem("PlantIt/PlantIt-Editor")]
    private static void Init()
    {
        PlantItEditorWindow window = EditorWindow.GetWindow<PlantItEditorWindow>();
        window.title = "PlantIt-Editor";
    }

    private void Awake()
    {

    }

    private void OnGUI()
    {
        GUILayout.Space(5);
        GUILayout.Label("Create a new settings file", EditorStyles.boldLabel);
        GUILayout.Label("The settings file can be created as often as you want, but one must exist as Resources/PlantIt/PlantIt-Settings.asset.", EditorStyles.miniBoldLabel);
        if (GUILayout.Button("Create PlantIt-Settings file"))
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(typeof(PlantItSettings));
            AssetDatabase.CreateAsset(asset, "Assets/Resources/PlantIt/PlantIt-Settings.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        GUILayout.Space(5);
        GUILayout.Label("Create a new PlantIt-Object thats usable in PlantIt-Units.", EditorStyles.boldLabel);
        if (GUILayout.Button("Create PlantIt-Object"))
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(typeof(PlantItObject));
            AssetDatabase.CreateAsset(asset, "Assets/Resources/PlantIt/Plants/New PlantIt-Object.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        GUILayout.Space(5);
        GUILayout.Label("Create a new PlantIt-Unit that defines a new area containing PlantIt-Objects.", EditorStyles.boldLabel);
        if (GUILayout.Button("Create PlantIt-Object"))
        {
            GameObject newPlantItUnitGo = (GameObject)Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
            newPlantItUnitGo.name = "New PlantIt-Unit";
            newPlantItUnitGo.AddComponent<PlantItUnit>();
        }
    }
}
