
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Crossing
    {
        public Crossing(Vector3 center_)
        {
            center = center_;
        }
        public Vector3 center;
        public class RoadEdge
        {
            public Vector3 a, b;
            public Vector3 c, d;
            public Vector3 towards;
            public RoadEdge(Vector3 _a,Vector3 _b, Vector3 _tow, Vector3 center)
            {
                a = _a;
                b = _b;
                towards = _tow;
            }
        }
        public List<RoadEdge> szomszedok = new List<RoadEdge>();
        public void AddSzomszed(Vector3 a,Vector3 b,GraphPoint towards)
        {
            szomszedok.Add(new RoadEdge(a, b, towards.position,center));
        }
        public void Draw()
        {
            foreach (RoadEdge road in szomszedok)
            {
                Debug.DrawLine(road.a, road.b, Color.black, 1000, false);
            }
        }
    }
}
