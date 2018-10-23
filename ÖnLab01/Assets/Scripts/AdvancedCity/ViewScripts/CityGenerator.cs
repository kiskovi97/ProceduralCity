using UnityEngine;
using UnityEditor;
using Assets.Scripts.AdvancedCity;
using System.Collections.Generic;

public class CityGenerator : EditorWindow
{
    private EditorObjectGenerator gameObjectGenerator = new EditorObjectGenerator();
    private GameObjects gameObjects = new GameObjects();
    private GameObject[] buildings = new GameObject[3];
    private GameObject[] places = new GameObject[1];
    private GameObject[] people = new GameObject[1];
    private GraphGenerator graphGen = new GraphGenerator();
    private RoadandCrossingGenerator crossingGenerator = new RoadandCrossingGenerator();
    private RoadGeneratingValues values = new RoadGeneratingValues();
    private List<Crossing> crossings = null;
    private List<GraphPoint> points = null;
    private VehiclesObject vehicles = new VehiclesObject();
    private int carsNumber = 20;
    private int peopleNumber = 4;
    private int size = 5;
    private bool graphGenerated = false;
    private bool isCamera = false;
    private bool building = false;

    [MenuItem("Window/CityGenerator")]
    public static void ShowWindow()
    {
        GetWindow<CityGenerator>("CityGenerator");
    }

    Vector2 scrollPoint;
    private void OnGUI()
    {
        values.SetSize(new float[] { -size, -size, size, size });
        scrollPoint = GUILayout.BeginScrollView(scrollPoint);
        if (GUILayout.Button("Clear"))
        {
            Clear();
        }
        GUILayout.Space(5);
        GraphAndRoadGUI();
        GUILayout.Space(5);

        if (crossings != null && crossings.Count > 0)
        {
            if (!building)
            {
                GUILayout.Space(5);
                BuildingGUI();
                GUILayout.Space(5);
            }
            GUILayout.Space(5);
            CarGUI();
            GUILayout.Space(5);
        }
        GUILayout.EndScrollView();
    }

    private bool roadFoldout = false;
    private void GraphAndRoadGUI()
    {
        roadFoldout = EditorGUILayout.Foldout(roadFoldout, "Road and Graph Settings");
        if (roadFoldout)
        {
            GUILayout.Label("City Generating Objects:", EditorStyles.boldLabel);
            GUILayout.Space(5);
            size = EditorGUILayout.IntField("Size", size);
            values.sizeRatio = EditorGUILayout.FloatField("Ratio", values.sizeRatio);
            gameObjects.CrossLamp = (GameObject)EditorGUILayout.ObjectField("Cross Lamp", gameObjects.CrossLamp, typeof(GameObject), true);
            gameObjects.RailObject = (GameObject)EditorGUILayout.ObjectField("Rail Object", gameObjects.RailObject, typeof(GameObject), true);
            gameObjects.RoadObject = (GameObject)EditorGUILayout.ObjectField("Road Object", gameObjects.RoadObject, typeof(GameObject), true);
            gameObjects.SideLamp = (GameObject)EditorGUILayout.ObjectField("Side Lamp", gameObjects.SideLamp, typeof(GameObject), true);
            gameObjects.StoppingObj = (GameObject)EditorGUILayout.ObjectField("Stopping Obj", gameObjects.StoppingObj, typeof(GameObject), true);
            gameObjects.WireObject = (GameObject)EditorGUILayout.ObjectField("Wire Object", gameObjects.WireObject, typeof(GameObject), true);

        }
        if (GUILayout.Button("Create Roads"))
        {
            Clear();
            GenerateGraph();
        }
    }

    private bool buildingFoldout = false;
    private void BuildingGUI()
    {
        buildingFoldout = EditorGUILayout.Foldout(buildingFoldout, "Building Settings");
        if (buildingFoldout)
        {
            GUILayout.Label("City Generating Values:", EditorStyles.boldLabel);
            GUILayout.Space(5);
            values.HouseUpmax = EditorGUILayout.FloatField("Max House", values.HouseUpmax);
            values.HouseUpmin = EditorGUILayout.FloatField("Min House", values.HouseUpmin);
            values.map = (Texture2D)EditorGUILayout.ObjectField("Map", values.map, typeof(Texture2D), true);
            buildings = ArrayInput("Building", buildings);
            places = ArrayInput("Places", places);
        }
        if (GUILayout.Button("Create Buildings"))
        {
            GenerateBuildings();
        }
    }

    private bool carFoldout = false;
    private void CarGUI()
    {
        carFoldout = EditorGUILayout.Foldout(carFoldout, "Car Settings");
        if (carFoldout)
        {
            GUILayout.Label("Cars:", EditorStyles.boldLabel);
            GUILayout.Space(5);
            vehicles.cameraCar = (GameObject)EditorGUILayout.ObjectField("Camera Car", vehicles.cameraCar, typeof(GameObject), true);
            vehicles.tram = (GameObject)EditorGUILayout.ObjectField("Tram", vehicles.tram, typeof(GameObject), true);
            vehicles.cars = ArrayInput("Cars", vehicles.cars);
            vehicles.people = ArrayInput("Cars", vehicles.people);
            carsNumber = EditorGUILayout.IntField("Displayable Cars Number:", carsNumber);
            peopleNumber = EditorGUILayout.IntField("Displayable Parson Number:", peopleNumber);
        }
        if (GUILayout.Button("Create Cars"))
        {
            GenerateCars();
        }
    }

    private GameObject[] ArrayInput(string name, GameObject[] input)
    {
        GUILayout.Label(name, EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (input == null) input = new GameObject[1];
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
        building = true;
    }

    private void Clear()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Generated");
        AllDestroy(list);
        graphGenerated = false;
        isCamera = false;
        building = false;
        crossings = null;
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
        GameObject player = Instantiate(vehicles.CameraCar);
        if (isCamera) cars.Add(player);
        for (int i = 0; i < carsNumber; i++)
            cars.Add(Instantiate(vehicles.Car));
        GameObject movementPointContainer = crossingGenerator.SaveMovementPoints();
        movementPointContainer.tag = "Generated";
        crossingGenerator.SetCarsStartingPosition(cars.ToArray());


        GameObject tram1 = Instantiate(vehicles.Tram);
        GameObject tram2 = Instantiate(vehicles.Tram);
        crossingGenerator.SetTram(tram1, tram2);

        List<GameObject> people = new List<GameObject>();
        for (int i = 0; i < peopleNumber; i++)
        {
            people.Add(Instantiate(vehicles.Person));
        }
        crossingGenerator.SetPeopleStartingPosition(people.ToArray());

        GameObject crossingManager = crossingGenerator.CreateCrossingManager();
        crossingManager.tag = "Generated";
        isCamera = true;
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
