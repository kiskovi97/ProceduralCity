using UnityEngine;
using UnityEditor;
using Assets.Scripts.AdvancedCity;
using System.Collections.Generic;

public class CityGenerator : EditorWindow
{
    public EditorObjectGenerator gameObjectGenerator = new EditorObjectGenerator();
    public GameObjects gameObjects = new GameObjects();
    public GameObject[] buildings = new GameObject[3];
    public GameObject[] places = new GameObject[1];
    private GraphGenerator graphGen = new GraphGenerator();
    private RoadandCrossingGenerator crossingGenerator = new RoadandCrossingGenerator();
    private RoadGeneratingValues values = new RoadGeneratingValues();
    private List<Crossing> crossings = null;
    private List<GraphPoint> points = null;
    public VehiclesObject vehicles = new VehiclesObject();

    [MenuItem("Window/CityGenerator")]
    public static void ShowWindow()
    {
        GetWindow<CityGenerator>("CityGenerator");
    }

    Vector2 scrollPoint;
    bool graphGenerated = false;
    private void OnGUI()
    {
        values.SetSize(new float[] { -5, -5, 5, 5 });
        scrollPoint = GUILayout.BeginScrollView(scrollPoint);
        if (GUILayout.Button("Clear"))
        {
            Clear();
        }
        GUILayout.Space(5);

        GUILayout.Label("City Generating Objects:", EditorStyles.boldLabel);
        GUILayout.Space(5);
        gameObjects.CrossLamp = (GameObject)EditorGUILayout.ObjectField("Cross Lamp", gameObjects.CrossLamp, typeof(GameObject), true);
        gameObjects.RailObject = (GameObject)EditorGUILayout.ObjectField("Rail Object", gameObjects.RailObject, typeof(GameObject), true);
        gameObjects.RoadObject = (GameObject)EditorGUILayout.ObjectField("Road Object", gameObjects.RoadObject, typeof(GameObject), true);
        gameObjects.SideLamp = (GameObject)EditorGUILayout.ObjectField("Side Lamp", gameObjects.SideLamp, typeof(GameObject), true);
        gameObjects.StoppingObj = (GameObject)EditorGUILayout.ObjectField("Stopping Obj", gameObjects.StoppingObj, typeof(GameObject), true);
        gameObjects.WireObject = (GameObject)EditorGUILayout.ObjectField("Wire Object", gameObjects.WireObject, typeof(GameObject), true);
        if (GUILayout.Button("Create Roads"))
        {
            Clear();
            GenerateGraph();
        }
        GUILayout.Space(5);

        if (graphGenerated)
        {
            GUILayout.Label("City Generating Values:", EditorStyles.boldLabel);
            GUILayout.Space(5);
            values.HouseUpmax = EditorGUILayout.FloatField("Max House", values.HouseUpmax);
            values.HouseUpmin = EditorGUILayout.FloatField("Min House", values.HouseUpmin);
            values.map = (Texture2D)EditorGUILayout.ObjectField("Map", values.map, typeof(Texture2D), true);
            buildings = ArrayInput("Building", buildings);
            places = ArrayInput("Places", places);
            if (graphGenerated && GUILayout.Button("Create Buildings"))
            {
                GenerateBuildings();
            }
            GUILayout.Space(5);
            GUILayout.Label("Cars:", EditorStyles.boldLabel);
            GUILayout.Space(5);
            vehicles.cameraCar = (GameObject)EditorGUILayout.ObjectField("Camera Car", vehicles.cameraCar, typeof(GameObject), true);
            vehicles.cars = ArrayInput("Cars", vehicles.cars);
            if (GUILayout.Button("Create Cars"))
            {
                GenerateCars();
            }
        }
        GUILayout.EndScrollView();
    }

    private GameObject[] ArrayInput(string name, GameObject[] input)
    {
        GUILayout.Label(name, EditorStyles.boldLabel);
        GUILayout.Space(10);
        int max = input.Length;
        max = EditorGUILayout.IntField(name + " Count", max);
        GameObject[] tmp = new GameObject[max];
        for (int i = 0; i < input.Length && i < max; i++)
        {
            tmp[i] = input[i];
        }
        for (int i = 0; i < tmp.Length; i++)
            tmp[i] = (GameObject)EditorGUILayout.ObjectField(name + i, tmp[i], typeof(GameObject), true);
        GUILayout.Space(10);
        return tmp;
    }

    private void GenerateGraph()
    {
        gameObjects.buildings = buildings;
        gameObjects.greenPlace = places;
        points = graphGen.GenerateGraph(values);
        int max = 10;
        while (max > 0 && points.Count <= 0)
        {
            Debug.Log("The " + (10 - max + 1) + ". try was an empty graph");
            max--;
            points = graphGen.GenerateGraph(values);
        }
        Debug.Log("GraphGenerated");
        gameObjectGenerator.SetValues(values, gameObjects);
        crossings = crossingGenerator.GenerateObjects(gameObjectGenerator, points, values.RoadSize);
        Debug.Log("Crossings set");
        crossingGenerator.DrawRoads(false, false, true, true);
        Debug.Log("DrawRoads");
        gameObjectGenerator.CreatRoadMesh();
        Debug.Log("CreatRoadMesh");
        graphGenerated = true;
    }

    private void GenerateBuildings()
    {
        gameObjectGenerator.GenerateBlocks(crossings, values);
    }

    private void Clear()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Generated");
        AllDestroy(list);
        graphGenerated = false;
    }

    void AllDestroy(GameObject[] list)
    {
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

    public void GenerateCars()
    {
        List<GameObject> cars = new List<GameObject>();
        GameObject player = Instantiate(vehicles.cameraCar);
        cars.Add(player);
        crossingGenerator.SetCarsStartingPosition(cars.ToArray());
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
