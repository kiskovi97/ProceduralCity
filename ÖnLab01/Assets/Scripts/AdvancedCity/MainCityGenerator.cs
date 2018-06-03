
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
        ObjectGenerator objGen;
        void Start()
        {
            Debug.Log("Start");

            List<GraphPoint> points = graphGen.GenerateGraph();
            //graphGen.Visualization01();
            Debug.Log("Graph Generated");
            objGen = new ObjectGenerator(points);
            objGen.GenerateObjects();
            objGen.GenerateRoadandCros();
            objGen.DrawRoads();
            List<GameObject> cars = new List<GameObject>();
            if (cameracar != null) cars.Add(Instantiate(cameracar));
            for (int i=0; i<30; i++)
            {
                
                GameObject realcar = Instantiate(car);
                cars.Add(realcar);
            }
            objGen.CreateCars(cars.ToArray());
            for (int j=0; j<5; j++)
            {
                List<GameObject> cars2 = new List<GameObject>();
                for (int i = 0; i < 30; i++)
                {

                    GameObject realcar = Instantiate(car);
                    cars2.Add(realcar);
                }
                objGen.CreateCars(cars2.ToArray());
            }
            GameObject instant = Instantiate(blockgenerator);
            objGen.MakeBlocks(instant.GetComponent<BlockGenerator>());
            
        }


    }
}
