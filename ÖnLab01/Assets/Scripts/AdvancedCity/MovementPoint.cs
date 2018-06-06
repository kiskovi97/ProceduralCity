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
        public void Draw(bool depthtest)
        {
            
            if (outPoints == null)
            {
                Debug.DrawLine(center, center + new Vector3(0, 1, 0), Color.red, 1000, depthtest);
                return;
            }
                
            for (int i=0; i<outPoints.Count; i++)
                Debug.DrawLine(center, (outPoints[i].center*4 + center*0)/4, Color.green, 1000, depthtest);
            
        }
        public static void Connect(MovementPoint[] be, MovementPoint[] ki)
        {
            int kulonbseg = System.Math.Abs(be.Length - ki.Length);
            if (be.Length < ki.Length)
            {
                for (int j = 0; j < kulonbseg; j++)
                    be[0].ConnectPoint(ki[j]);
                for (int j = 0; j < be.Length; j++)
                    be[j].ConnectPoint(ki[j + kulonbseg]);
            }
            else
            {
                for (int j = 0; j < ki.Length; j++)
                    be[j].ConnectPoint(ki[j]);
                for (int j = ki.Length; j < be.Length; j++)
                    be[j].ConnectPoint(ki[ki.Length - 1]);
            }
        }
    }
}
