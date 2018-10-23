using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BuildingObject))]
public class BuildingEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildingObject obj = (BuildingObject)target;

        if (GUILayout.Button("ReGenerate"))
        {
            obj.ReGenerate();
        }
    }
}
