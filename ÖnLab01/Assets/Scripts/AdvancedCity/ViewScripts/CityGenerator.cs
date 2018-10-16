using UnityEngine;
using UnityEditor;
using Assets.Scripts.AdvancedCity;
using System.Collections.Generic;

public class CityGenerator : EditorWindow
{
    public RoadGeneratingValues values;
    public GameObjectGenerator gameObjectGenerator;
    private GraphGenerator graphGen = new GraphGenerator();
    private RoadandCrossingGenerator crossingGenerator = new RoadandCrossingGenerator();
    private List<Crossing> crossings = null;
    private List<GraphPoint> points = null;
    private Texture2D texture;
    public FollowPlayer camera;
    public VehiclesObject vehicles = new VehiclesObject();

    [MenuItem("Window/CityGenerator")]
    public static void ShowWindow()
    {
        GetWindow<CityGenerator>("CityGenerator");
    }

    private void OnGUI()
    {
        values.SetSize(new float[] { -5, -5, 5, 5 });
        GUILayout.Label("THis is a Label", EditorStyles.boldLabel);
        GUILayout.Space(20);
        gameObjectGenerator = (GameObjectGenerator)EditorGUILayout.ObjectField("Game Object Generator", gameObjectGenerator, typeof(GameObjectGenerator), true);
        camera = (FollowPlayer)EditorGUILayout.ObjectField("Camera", camera, typeof(FollowPlayer), true);
        vehicles.cameraCar = (GameObject)EditorGUILayout.ObjectField("Camera Car", vehicles.cameraCar, typeof(GameObject), true);
        if (GUILayout.Button("GenerateGraph"))
        {
            Clear();
            GenerateGraph();
        }
        values.HouseUpmax = EditorGUILayout.FloatField("Max House", values.HouseUpmax);
        values.HouseUpmin = EditorGUILayout.FloatField("Min House", values.HouseUpmin);
        values.map = (Texture2D)EditorGUILayout.ObjectField("Map", values.map, typeof(Texture2D), true);
        if (GUILayout.Button("GenerateBuildings"))
        {
            GenerateBuildings();
        }

        if (GUILayout.Button("Clear"))
        {
            Clear();
        }

        if (GUILayout.Button("Generate Cars"))
        {
            GenerateCars();
        }
    }

    private void GenerateGraph()
    {
        points = graphGen.GenerateGraph(values);
        int max = 10;
        while (max > 0 && points.Count <= 0)
        {
            Debug.Log("The " + (10 - max + 1) + ". try was an empty graph");
            max--;
            points = graphGen.GenerateGraph(values);
        }
        gameObjectGenerator.SetValues(values);
        crossings = crossingGenerator.GenerateObjects(gameObjectGenerator, points, values.RoadSize);
        crossingGenerator.DrawRoads(false, false, true, true);
        gameObjectGenerator.CreatRoadMesh();
    }

    private void GenerateBuildings()
    {
        gameObjectGenerator.GenerateBlocks(crossings, values);
    }

    private void Clear()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Generated");
        AllDestroy(list);
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
