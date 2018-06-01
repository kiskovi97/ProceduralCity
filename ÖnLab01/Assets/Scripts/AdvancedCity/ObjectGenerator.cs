using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class ObjectGenerator
    {
        List<GraphPoint> controlPoints;
        public ObjectGenerator(List<GraphPoint> points)
        {
            controlPoints = points;
        }
    }
}
