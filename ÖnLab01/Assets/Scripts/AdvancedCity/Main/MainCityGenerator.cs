
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
        public Vehicles vehicles;
        public bool VisualGraph = false;
        public bool VisualRoads = false;
        public bool makecars = true;
        public bool helplines_draw = true;
        public int cars_number = 100;
        public bool depthtest = false;
        public bool makeblocks = true;
        public bool trams = true;

        private List<Crossing> crossings = null;
        void Start()
        {
            RoadGeneratingValues values = GetComponent<RoadGeneratingValues>();
            if (values == null) return;
            if (!values.isActiveAndEnabled) return;

            GraphGenerator graphGen = GetComponent<GraphGenerator>();
            if (graphGen == null) return;
            if (!graphGen.isActiveAndEnabled) return;
            
            List<GraphPoint> points = graphGen.GenerateGraph(VisualGraph,depthtest);
            int max = 10;
            while (max > 0 && points.Count <= 0)
            {
                int i = 10-max+1;
                Debug.Log("The " + i + ". try was an empty graph");
                max--;
                points = graphGen.GenerateGraph(VisualGraph, depthtest);
            }
            GameObjectGenerator gameObjectGenerator = GetComponent<GameObjectGenerator>();
            if (gameObjectGenerator == null) return;
            if (!gameObjectGenerator.isActiveAndEnabled) return;

            objGen = new RoadandCrossingGenerator();
            if (gameObjectGenerator != null)
            {
                crossings = objGen.GenerateObjects(gameObjectGenerator, points, values.sizeRatio);
                if (VisualRoads)
                    objGen.DrawRoads(helplines_draw, depthtest);
                if (makeblocks)
                    gameObjectGenerator.GenerateBlocks(crossings);
                if (makecars)
                    GenerateCars();
            }
            foreach (Crossing cros in crossings)
            {
                cros.Valt();
            }
        }

        //--- Ez majd a KRESZ osztaly feladata lesz ----------
        void GenerateCars()
        {
            List<GameObject> cars = new List<GameObject>();
            
            for (int i = 0; i < cars_number; i++)
            {
                if ((i == 5 && cars_number > 50)   || (i==0 && cars_number <= 50))
                {
                    if (vehicles.cameraCar != null)
                    {
                        cars.Add(Instantiate(vehicles.cameraCar));
                    }
                }
                cars.Add(vehicles.Car);
            }
            
            objGen.SetCarsStartingPosition(cars.ToArray());
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
            if (crossings == null) return;
            i++;
            if (i % 300 == 0)
            {
                foreach (Crossing cros in crossings)
                {
                    cros.Valt();
                }
                if (i == 300) i = 500;
                else i = 1;
                Debug.Log("Valtott");
            }
        }


    }
}
