
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class MainCityGenerator: MonoBehaviour
    {
        // ------- Generating Objects -----------
        public GraphGenerator graphGen;
        GameObjectGenerator gameObjectGenerator;
        RoadandCrossingGenerator objGen;
        // ------- Instantinate Objects
        public GameObject cameracar;
        public Vehicles car;
        public GameObject blockgenerator;

        public bool VisualGraph = false;
        public bool VisualRoads = false;
        public bool makecars = true;
        public bool helplines_draw = true;
        public int cars_number = 100;
        public bool depthtest = false;
        public bool makeblocks = true;

        private List<Crossing> crossings = null;
        void Start()
        {
            Debug.Log("Start Generating");
            // ------------ Generate Graph -----------
            List<GraphPoint> points = graphGen.GenerateGraph();
            if (VisualGraph)
                graphGen.Visualization01(depthtest);

            
            GameObject instant = Instantiate(blockgenerator);
            gameObjectGenerator = instant.GetComponent<GameObjectGenerator>();

            objGen = new RoadandCrossingGenerator();
            if (gameObjectGenerator != null)
            {
                crossings = objGen.GenerateObjects(gameObjectGenerator, points);
                if (VisualRoads)
                    objGen.DrawRoads(helplines_draw, depthtest);
                if (makeblocks)
                    gameObjectGenerator.GenerateBlocks(crossings);
                if (makecars)
                    GenerateCars();
            }
        }

        //--- Ez majd a KRESZ osztaly feladata lesz ----------
        void GenerateCars()
        {
            List<GameObject> cars = new List<GameObject>();
            if (cameracar != null) cars.Add(Instantiate(cameracar));
            for (int i = 0; i < cars_number; i++)
            {
                
                cars.Add(car.Car);
            }
            objGen.SetCarsStartingPosition(cars.ToArray());
            
        }
        
        int i = 1;
        void Update()
        {
            if (crossings == null) return;
            i++;
            if (i % 100 == 0)
            {
                foreach (Crossing cros in crossings)
                {
                    cros.Valt();
                }
                i = 1;
                Debug.Log("Valtott");
            }
        }


    }
}
