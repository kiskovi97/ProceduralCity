
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class MainCityGenerator: MonoBehaviour
    {
        public GraphGenerator graphGen;
        ObjectGenerator objGen;
        void Start()
        {
            Debug.Log("Start");

            List<GraphPoint> points = graphGen.GenerateGraph();
            graphGen.Visualization01();
            Debug.Log("Graph Generated");
            objGen = new ObjectGenerator(points);
            List<Crossing> crossings = objGen.GenerateRoadMesh();
            foreach (Crossing cros in crossings)
            {
                cros.Draw();
            }
        }


    }
}
