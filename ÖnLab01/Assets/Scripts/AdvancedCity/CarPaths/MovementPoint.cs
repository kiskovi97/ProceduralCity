using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    public class MovementPoint
    {
        public Vector3 center;
        public Vector3 direction;
        public MovementPoint(Vector3 mov)
        {
            center = mov;
        }
        public void setDirection(Vector3 dir)
        {
            direction = dir;
        }
        private List<MovementPoint> outPoints;
        private bool onmaga = false;
        public void Nyitott(bool nyitott)
        {
            onmaga = !nyitott;
        }
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
        public MovementPoint getPoint()
        {
            if (outPoints == null) return null;
            if (outPoints.Count < 1) return null;
            return outPoints[0];
        }
        public MovementPoint getNextPoint()
        {
            if (outPoints == null) return null;
            if (outPoints.Count < 1) return null;
            if (!onmaga)
            {
                int i = (int)(Random.value * (outPoints.Count));
                return outPoints[i];
            }
            else return this;
               
        }
        public void Draw(bool depthtest)
        {
            Debug.DrawLine(center, center + new Vector3(0, 1, 0), Color.blue, 1000, depthtest);
            if (outPoints == null)
            {
                Debug.DrawLine(center, center + new Vector3(0, 1, 0), Color.red, 1000, depthtest);
                return;
            }

            for (int i = 0; i < outPoints.Count; i++)
            {
                Vector3 cross;
                if (Vector3.Angle(direction, outPoints[i].direction) < 30)
                {
                    cross = (center + outPoints[i].center) / 2;
                }
                else
                {
                    cross = MyMath.Intersect(center, direction, outPoints[i].center, outPoints[i].direction);
                }
                for (float time = 0; time < 1; time += 0.1f)
                {
                    Debug.DrawLine(BezierCurve(center,cross, outPoints[i].center, time), BezierCurve(center, cross, outPoints[i].center, time+0.1f), Color.green, 1000, depthtest);
                }
            } 
        }

        public static MovementPoint[] Connect(MovementPoint[] be, MovementPoint[] ki, bool beTram, bool kiTram, Vector3 beDir, Vector3 kiDir)
        {
            List<MovementPoint> output = new List<MovementPoint>();
            int kulonbseg = System.Math.Abs(be.Length - ki.Length);
            if (beTram && !kiTram)
            {
                be = be.Take(be.Count() - 1).ToArray();
                return Connect(be, ki, false, false, beDir, kiDir);
            }
            if (!beTram && kiTram)
            {
                ki = ki.Take(ki.Count() - 1).ToArray();
                return Connect(be, ki, false, false, beDir, kiDir);
            }
            if (beTram && kiTram)
            {
                output.AddRange(CurveAndConnect(be.Last(), ki.Last(), beDir, kiDir));
                be = be.Take(be.Count() - 1).ToArray();
                ki = ki.Take(ki.Count() - 1).ToArray();
                return Connect(be, ki, false, false, beDir, kiDir).Concat(output.ToArray()).ToArray();
            }

            if (be.Length < ki.Length)
            {
                int length = be.Length;
                for (int j = 0; j < kulonbseg; j++)
                    be[0].ConnectPoint(ki[j]);
                for (int j = 0; j < length; j++)
                    be[j].ConnectPoint(ki[j + kulonbseg]);
            }
            else
            {
                int length = ki.Length;
                for (int j = 0; j < kulonbseg; j++)
                    be[j].ConnectPoint(ki[0]);
                for (int j = 0; j < length; j++)
                   be[j + kulonbseg].ConnectPoint(ki[j]);
            }
            return output.ToArray();
        }

        static MovementPoint[] CurveAndConnect(MovementPoint be, MovementPoint ki, Vector3 beDir, Vector3 kiDir, int iterationMax = 5)
        {
            if (iterationMax < 1 || Vector3.Angle(beDir,kiDir) < 30)
            {
                be.ConnectPoint(ki);
                return new MovementPoint[0];
            }
            Vector3 cross = MyMath.Intersect(be.center, beDir, ki.center, kiDir);
            List<MovementPoint> movementPoints = new List<MovementPoint>();
            bool elso = true;
            for (float i=0.1f; i<1; i+= 0.1f)
            {
                MovementPoint point = new MovementPoint(
                    BezierCurve(be.center, cross, ki.center, i));
                point.setDirection(directionBezierCurve(be.center, cross, ki.center, i));
                if (elso) be.ConnectPoint(point);
                else movementPoints.Last().ConnectPoint(point);
                movementPoints.Add(point);
                elso = false;
            }
            movementPoints.Last().ConnectPoint(ki);
            return movementPoints.ToArray();
        }

        public static Vector3 BezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, float time)
        {
            return (1 - time) * (1 - time) * P0 +
                2 * (1 - time) * time * P1 +
                time * time * P2;
        }

        public static Vector3 directionBezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, float t)
        {
            return 2 * (1 - t) * (P1 - P0) + 2 * t * (P2 - P1);
        }

    }
}
