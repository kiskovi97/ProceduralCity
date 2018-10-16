using UnityEngine;
using UnityEditor;
using Assets.Scripts.AdvancedCity;
using System.Collections.Generic;

public class CityGenerator : EditorWindow
{
    string myString = "Hello World";
    public RoadGeneratingValues values;
    public GameObjectGenerator gameObjectGenerator;
    private GraphGenerator graphGen = new GraphGenerator();
    private RoadandCrossingGenerator crossingGenerator = new RoadandCrossingGenerator();
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
        gameObjectGenerator = (GameObjectGenerator)EditorGUILayout.ObjectField(gameObjectGenerator, typeof(GameObjectGenerator), true);
        if (GUILayout.Button("Press Me"))
        {
            myString = "Pressed";
        }

        if (GUILayout.Button("GenerateGraph"))
        {
            myString = "GenerateGraph start";
            Clear();
            GenerateGraph();
        }
    }

    private void GenerateGraph()
    {
        Handles.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
        if (graphGen != null && values != null)
        {
            points = graphGen.GenerateGraph(values);
            int max = 10;
            while (max > 0 && points.Count <= 0)
            {
                Debug.Log("The " + (10 - max + 1) + ". try was an empty graph");
                max--;
                points = graphGen.GenerateGraph(values);
            }
            crossings = crossingGenerator.GenerateObjects(gameObjectGenerator, points, values.RoadSize);
            crossingGenerator.DrawRoads(false, false, true, true);
            gameObjectGenerator.CreatRoadMesh();
            //gameObjectGenerator.GenerateBlocks(crossings, delegateStep, values);
        }
        myString = "GenerateGraph done";
    }

    private void Clear()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Generated");
        AllDestroy(list);
    }
    void AllDestroy(GameObject[] list)
    {
        float step = 1.0f / list.Length;
        int refresh = 0;
        foreach (GameObject obj in list)
        {
            SafeDestroy(obj);
            refresh++;
            if (refresh > 100)
            {
                refresh = 0;
            }
        }
    }

    public static T SafeDestroy<T>(T obj) where T : Object
    {
        if (Application.isEditor)
            Object.DestroyImmediate(obj);
        else
            Object.Destroy(obj);

        return null;
    }
    public static T SafeDestroyGameObject<T>(T component) where T : Component
    {
        if (component != null)
            SafeDestroy(component.gameObject);
        return null;
    }
}
