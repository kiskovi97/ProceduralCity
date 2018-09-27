
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.AdvancedCity
{
    [RequireComponent(typeof(RoadGeneratingValues))]
    class MainCityGenerator: MonoBehaviour
    {
        
        RoadandCrossingGenerator objGen;
        RoadGeneratingValues values;
        // ------- Instantinate Objects
        public FollowPlayer[] cameras;
        public Vehicles vehicles;
        public GameObject person;
        public bool VisualGraph = false;
        public bool VisualRoads = false;
        public bool MakeLamps = false;
        public bool makecars = true;
        public bool helplines_draw = true;
        public int cars_number = 100;
        public int peopleMax = 100;
        public bool depthtest = false;
        public bool makeblocks = true;
        public bool trams = true;

        private List<Crossing> crossings = null;
        void Start()
        {
            Debug.Log("Start");
            RoadGeneratingValues values = GetComponent<RoadGeneratingValues>();
            if (values == null) return;
            if (!values.isActiveAndEnabled) return;

            GraphGenerator graphGen = GetComponent<GraphGenerator>();
            if (graphGen == null) return;
            if (!graphGen.isActiveAndEnabled) return;

            List<GraphPoint> points = graphGen.GenerateGraph(VisualGraph, depthtest);
            int max = 10;
            while (max > 0 && points.Count <= 0)
            {
                int i = 10 - max + 1;
                Debug.Log("The " + i + ". try was an empty graph");
                max--;
                points = graphGen.GenerateGraph(VisualGraph, depthtest);
            }
            GameObjectGenerator gameObjectGenerator = GetComponent<GameObjectGenerator>();
            if (gameObjectGenerator == null) return;
            if (!gameObjectGenerator.isActiveAndEnabled) return;
            gameObjectGenerator.SetValues(values);

            objGen = new RoadandCrossingGenerator();
            if (gameObjectGenerator != null)
            {
                crossings = objGen.GenerateObjects(gameObjectGenerator, points, values.sizeRatio);
                if (VisualRoads)
                {
                    objGen.DrawRoads(helplines_draw, depthtest, MakeLamps, trams);
                    gameObjectGenerator.CreatRoadMesh();
                }
                if (makeblocks)
                    gameObjectGenerator.GenerateBlocks(crossings);
                if (makecars)
                    GenerateCars();
            }
            foreach (Crossing cros in crossings)
            {
                cros.Valt();
            }
            //Invoke("Export", 1f);
        }
        
        void Export()
        {
            Debug.Log("Export Start");
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            List<MeshFilter> filters = new List<MeshFilter>();
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.GetComponent<BuildingObject>() != null)
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
            AscnyFgv(filters, renderers);
            Debug.Log("Export Done");
        }

        void AscnyFgv(List<MeshFilter> filters, List<MeshRenderer> renderers)
        {
            for(int i = 0; i < filters.Count && i < 1; i++)
            {
                MeshFilter filter = filters[i];
                MeshRenderer meshRenderer = renderers[i];
                string fileName = i + "mesh";
                ObjExporter.MeshToFile(filter, meshRenderer.materials, "Assets/Export/"+fileName+".obj");
            }
        }

        //--- Ez majd a KRESZ osztaly feladata lesz ----------
        void GenerateCars()
        {
            List<GameObject> cars = new List<GameObject>();
            for (int i = 0; i < cars_number; i++)
            {
                if ((i == 5 && cars_number > 50) || (i == 0 && cars_number <= 50))
                {
                    if (vehicles.cameraCar != null)
                    {
                        GameObject player = Instantiate(vehicles.cameraCar);
                        cars.Add(player);
                        foreach (FollowPlayer camera in cameras)
                        {
                            camera.player = player.transform;
                        }
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
            }
        }
        
        int i = 1;
        void Update()
        {
            if (Input.GetButtonDown("Exit")) {
                Debug.Log("Exit");
                Application.Quit();
            }
            if (crossings == null) return;
            i++;
            if (i % 300 == 0)
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
