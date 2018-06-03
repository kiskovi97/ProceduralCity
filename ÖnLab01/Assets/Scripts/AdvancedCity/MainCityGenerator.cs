
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class MainCityGenerator: MonoBehaviour
    {
        public GraphGenerator graphGen;
        public GameObject car;
        ObjectGenerator objGen;
        void Start()
        {
            Debug.Log("Start");

            List<GraphPoint> points = graphGen.GenerateGraph();
            //graphGen.Visualization01();
            Debug.Log("Graph Generated");
            objGen = new ObjectGenerator(points);
            objGen.GenerateObjects();
            objGen.GenerateRoadMesh();
            objGen.DrawRoads();
            List<GameObject> cars = new List<GameObject>();
            for (int i=0; i<50; i++)
            {
                
                GameObject realcar = Instantiate(car);
                cars.Add(realcar);
            }
            
            objGen.CreateCars(cars.ToArray());
        }


    }
}
