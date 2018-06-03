using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class MovementPoint
    {
        public Vector3 center;
        public MovementPoint(Vector3 mov)
        {
            center = mov;
        }
        private List<MovementPoint> outPoints;
        public void ConnectPoint(MovementPoint point)
        {
            if (outPoints == null) outPoints = new List<MovementPoint>();
            if (!outPoints.Contains(point)) outPoints.Add(point);
        }
        public void DisConnectPoint(MovementPoint point)
        {
            if (outPoints == null) return;
            outPoints.Remove(point);
        }
        public MovementPoint getNextPoint()
        {
            if (outPoints == null) return null;
            if (outPoints.Count < 1) return null;
            int i = (int)(Random.value * (outPoints.Count));
            return outPoints[i];
        }
        public void Draw()
        {
            if (outPoints == null) return;
            for (int i=0; i<outPoints.Count; i++)
                Debug.DrawLine(center, (outPoints[i].center*3 + center)/4, Color.green, 1000, false);
        }
    }
}
