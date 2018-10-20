
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.AdvancedCity
{
    public class MainCityGenerator : MonoBehaviour
    {
        public GameObjects gameObjects;
        public RoadGeneratingValues values;
        public FollowPlayer[] cameras;
        public VehiclesMonoBehevior vehicles;
        public GameObject person;
        public bool MakeLamps = false;
        public bool helplines_draw = true;
        public int cars_number = 100;
        public int peopleMax = 100;
        public bool depthtest = false;
        public bool begginingTram = false;

        private List<Crossing> crossings = null;
        private List<GraphPoint> points = null;
        private GraphGenerator graphGen = new GraphGenerator();
        private RoadandCrossingGenerator objGen = new RoadandCrossingGenerator();
        private GameObjectGenerator gameObjectGenerator = null;
        public void SetValues(RoadGeneratingValues values) {
            this.values = values;
        }
        public void SetSize(float size)
        {
            values.SetSize(new float[4]
            {
                -size,-size,size,size
            });
        }
        public float GetSize()
        {
            return values.getSize();
        }
        public void GenerateOnlyGraph(bool visual)
        {
            points = graphGen.GenerateGraph(values, visual, depthtest);
            int max = 10;
            while (max > 0 && points.Count <= 0)
            {
                max--;
                points = graphGen.GenerateGraph(values, visual, depthtest);
            }
            gameObjectGenerator = GetComponent<GameObjectGenerator>();
            gameObjectGenerator.SetValues(values, gameObjects);
            crossings = objGen.GenerateObjects(gameObjectGenerator, points, values.RoadSize);
        }
        public void GenerateEverything(GameObjectGenerator.step delegateStep)
        {
            GenerateOnlyGraph(false);
            DrawRoads();
            DrawBlocks(delegateStep);
            GenerateCars();
        }
        public void DrawRoads()
        {
            objGen.DrawRoads(helplines_draw, depthtest, MakeLamps, trams);
            gameObjectGenerator.CreatRoadMesh();
        }
        public void DrawBlocks(GameObjectGenerator.step delegateStep)
        {
            gameObjectGenerator.GenerateBlocks(crossings, delegateStep, values);
        }

        public void Export(int max)
        {
            Debug.Log("Export Start");
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            List<MeshFilter> filters = new List<MeshFilter>();
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.GetComponent<BuildingObject>() != null || obj.GetComponent<RoadPhysicalObject>() != null)
                {
                    MeshFilter filter = obj.GetComponent<MeshFilter>();
                    MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                    if (renderer != null && renderer != null)
                    {
                        filters.Add(filter);
                        renderers.Add(renderer);
                    }
                }
            }
            StartCoroutine(AscnyFgv(filters, renderers, max));
        }

        IEnumerator AscnyFgv(List<MeshFilter> filters, List<MeshRenderer> renderers, int max)
        {
            for (int i = 0; i < filters.Count && i < max; i++)
            {
                MeshFilter filter = filters[i];
                MeshRenderer meshRenderer = renderers[i];
                string fileName = i + "mesh";
                ObjExporter.MeshToFile(filter, meshRenderer.materials, "Assets/Export/" + fileName + ".obj");
                yield return null;
            }
            Debug.Log("Export Ended");
        }

        //--- Ez majd a KRESZ osztaly feladata lesz ----------
        bool playerOK = false;
        bool running = false;
        public bool trams = true;
        public void GenerateCars()
        {
            List<GameObject> cars = new List<GameObject>();
            for (int i = 0; i < cars_number; i++)
            {
                if ((i == 5 && cars_number > 50) || (i == 0 && cars_number <= 50))
                {
                    if (vehicles.cameraCar != null && !playerOK)
                    {
                        GameObject player = Instantiate(vehicles.cameraCar);
                        cars.Add(player);
                        foreach (FollowPlayer camera in cameras)
                        {
                            camera.player = player.transform;
                        }
                        playerOK = true;
                    }
                }
                cars.Add(vehicles.Car);
            }
            objGen.SetCarsStartingPosition(cars.ToArray());
            List<GameObject> people = new List<GameObject>();
            for (int i = 0; i < peopleMax; i++)
            {
                people.Add(Instantiate(person));
            }
            objGen.SetPeopleStartingPosition(people.ToArray());
            if (trams)
            {
                GameObject obj = vehicles.Tram;
                GameObject obj2 = vehicles.Tram;
                objGen.SetTram(obj, obj2);
                trams = false;
            }
            foreach (Crossing cros in crossings)
            {
                cros.Switch(true);
            }
            running = true;
        }

        public void Clear()
        {
            running = false;
            playerOK = false;
            trams = false;
        }
        int i = 1;
        bool standingBy = false;
        public void Update()
        {
            if (Input.GetButtonDown("Exit"))
            {
                Debug.Log("Exit");
                Application.Quit();
            }
            if (crossings == null) return;
            i++;
            if (i >= 100 && running)
            {
                if (standingBy)
                {
                    i = 0;
                    standingBy = false;
                    StartCoroutine(AllValt());
                    Debug.Log("Valtott");
                }
                else if (i >= 300)
                {
                    i = 0;
                    standingBy = true;
                    StartCoroutine(AllValt());
                    Debug.Log("Valtott");
                }
            }
        }

        System.Collections.IEnumerator AllValt()
        {
            foreach (Crossing cros in crossings)
            {
                cros.Switch(standingBy);
                yield return null;
            }
        }

    }
}
