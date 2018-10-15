using UnityEngine;
using UnityEditor;
using Assets.Scripts.AdvancedCity;
using System.Collections.Generic;

public class CityGenerator : EditorWindow
{

    string myString = "Hello World";

    private RoadGeneratingValues values = null;
    private GraphGenerator graphGen = null;
    private RoadandCrossingGenerator objGen = null;
    private GameObjectGenerator gameObjectGenerator = null;
    private List<Crossing> crossings = null;
    private List<GraphPoint> points = null;

    [MenuItem("Window/CityGenerator")]
    public static void ShowWindow()
    {
        GetWindow<CityGenerator>("CityGenerator");
    }

    private void OnGUI()
    {
        GUILayout.Label("THis is a Label", EditorStyles.boldLabel);
        GUILayout.Space(20);
        myString = EditorGUILayout.TextField("Name", myString);
        values = (RoadGeneratingValues)EditorGUILayout.ObjectField(values, typeof(RoadGeneratingValues), true);
        graphGen = (GraphGenerator)EditorGUILayout.ObjectField(graphGen, typeof(GraphGenerator), true);
        gameObjectGenerator = (GameObjectGenerator)EditorGUILayout.ObjectField(gameObjectGenerator, typeof(GameObjectGenerator), true);
        if (GUILayout.Button("Press Me"))
        {
            myString = "Pressed";
        }

        if (GUILayout.Button("GenerateGraph"))
        {
            myString = "GenerateGraph start";
            GenerateGraph();
        }
    }

    private void GenerateGraph()
    {
        Handles.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
        if (graphGen != null && values != null && gameObjectGenerator != null)
        {
            graphGen.SetValues(values);
            gameObjectGenerator.SetValues(values);
            myString = "GenerateGraph done";
            myString = values.CollapseMainRoad + "";
            points = graphGen.GenerateGraph();
            int max = 10;
            while (max > 0 && points.Count <= 0)
            {
                Debug.Log("The " + (10 - max + 1) + ". try was an empty graph");
                max--;
                points = graphGen.GenerateGraph();
            }
            objGen = new RoadandCrossingGenerator();
            crossings = objGen.GenerateObjects(gameObjectGenerator, points, values.RoadSize);
            objGen.DrawRoads(false, false, false, false);
            gameObjectGenerator.CreatRoadMesh();
        }
    }
}
