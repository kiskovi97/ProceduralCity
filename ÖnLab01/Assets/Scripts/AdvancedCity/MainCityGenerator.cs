
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
            if (graphGen != null)
                graphGen = new GraphGenerator();
            List<GraphPoint> points = graphGen.GenerateGraph();
            objGen = new ObjectGenerator(points);

        }
    }
}
