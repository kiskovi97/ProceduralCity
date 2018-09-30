
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.AdvancedCity
{
    [RequireComponent(typeof(RoadGeneratingValues))]
    public class MainCityGenerator : MonoBehaviour
    {
        public FollowPlayer[] cameras;
        public Vehicles vehicles;
        public GameObject person;
        public bool MakeLamps = false;
        public bool helplines_draw = true;
        public int cars_number = 100;
        public int peopleMax = 100;
        public bool depthtest = false;
        public bool begginingTram = false;

        private List<Crossing> crossings = null;
        private List<GraphPoint> points = null;
        private GameObjectGenerator gameObjectGenerator = null;
        private RoadandCrossingGenerator objGen = null;
        private RoadGeneratingValues values = null;
        public void setSize(float size)
        {
            values.Sizes[0] = -size;
            values.Sizes[1] = -size;
            values.Sizes[2] = size;
            values.Sizes[3] = size;
        }
        public float getSize()
        {
            if (values == null) values = GetComponent<RoadGeneratingValues>();
            return values.Sizes[2];
        }
        public void GenerateOnlyGraph(bool visual)
        {
            if (values == null) values = GetComponent<RoadGeneratingValues>();
            GraphGenerator graphGen = GetComponent<GraphGenerator>();
            points = graphGen.GenerateGraph(visual, depthtest);
            int max = 10;
            while (max > 0 && points.Count <= 0)
            {
                Debug.Log("The " + (10 - max + 1) + ". try was an empty graph");
                max--;
                points = graphGen.GenerateGraph(visual, depthtest);
            }
            gameObjectGenerator = GetComponent<GameObjectGenerator>();
            gameObjectGenerator.SetValues(values);
            objGen = new RoadandCrossingGenerator();
            crossings = objGen.GenerateObjects(gameObjectGenerator, points, values.sizeRatio);
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
            gameObjectGenerator.GenerateBlocks(crossings, delegateStep);
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
                cros.Valt();
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
        public void Update()
        {
            if (Input.GetButtonDown("Exit"))
            {
                Debug.Log("Exit");
                Application.Quit();
            }
            if (crossings == null) return;
            i++;
            if (i % 300 == 0 && running)
            {
                StartCoroutine(AllValt());
                if (i == 300) i = 500;
                else i = 1;
                Debug.Log("Valtott");
            }
        }

        System.Collections.IEnumerator AllValt()
        {
            foreach (Crossing cros in crossings)
            {
                cros.Valt();
                yield return null;
            }
        }

    }
}
