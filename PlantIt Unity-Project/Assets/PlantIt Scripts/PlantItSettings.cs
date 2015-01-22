// (c) 2015, Case-o-Matic
// PlantIt Unity3D Plugin

using UnityEngine;
using System.Collections;

public class PlantItSettings : ScriptableObject
{
    public const string plantItVersion = "1.00";
    public static PlantItSettings Current {
        get
        {
            if (_Current == null)
                _Current = (PlantItSettings)Resources.Load(@"PlantIt/PlantIt-Settings");
            return _Current;
        }
    }
    private static PlantItSettings _Current;

    public string behavioursResourceFolderPath = "PlantIt/Behaviours", plantsResourceFolderPath = "PlantIt/Plants";
    public bool drawPlantRangeSpheresGizmos, drawPlantsGrowthValueGizmos;
}
