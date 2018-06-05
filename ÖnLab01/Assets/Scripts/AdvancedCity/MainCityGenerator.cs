
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class MainCityGenerator: MonoBehaviour
    {
        public GraphGenerator graphGen;
        public GameObject cameracar;
        public GameObject car;
        public GameObject blockgenerator;
        public bool VisualGraph = false;
        public bool VisualRoads = false;
        public bool makecars = true;
        public bool helplines_draw = true;
        public int cars_number = 100;
        public bool depthtest = false;
        public bool makeblocks = true;
        ObjectGenerator objGen;
        void Start()
        {
            Debug.Log("Start Generating");
            List<GraphPoint> points = graphGen.GenerateGraph();
            objGen = new ObjectGenerator(points);
            GameObject instant = Instantiate(blockgenerator);
            objGen.GenerateObjects(instant.GetComponent<GameObjectGenerator>());
            objGen.GenerateRoadandCros();
            if (VisualGraph)
                graphGen.Visualization01(depthtest);
            if (VisualRoads)
                objGen.DrawRoads(helplines_draw, depthtest);
            if (makecars)
                GenerateCars();
            
            if (makeblocks)
            {
                objGen.MakeBlocks(instant.GetComponent<GameObjectGenerator>());
            }
            
        }
        void GenerateCars()
        {
            List<GameObject> cars = new List<GameObject>();
            if (cameracar != null) cars.Add(Instantiate(cameracar));
            for (int i = 0; i < cars_number; i++)
            {
                GameObject realcar = Instantiate(car);
                cars.Add(realcar);
            }
            objGen.CreateCars(cars.ToArray());
            
        }


    }
}
