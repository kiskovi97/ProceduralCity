
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Road
    {
        Vector3 x1, x2;
        Vector3 y1, y2;
        public Road(Vector3 x1_,Vector3 x2_,Vector3 y1_, Vector3 y2_)
        {
            x1 = x1_;
            x2 = x2_;
            y1 = y1_;
            y2 = y2_;
        }
        public void Draw()
        {
            Debug.DrawLine(x1, y1, Color.black, 1000, false);
            Debug.DrawLine(x2, y2, Color.black, 1000, false);
        }
    }
}
