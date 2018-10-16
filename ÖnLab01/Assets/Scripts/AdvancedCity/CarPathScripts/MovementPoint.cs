using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.AdvancedCity
{
    public class MovementPoint
    {
        public Vector3 center;
        public Vector3 direction;
        public bool megallo = false;
        public MovementPoint(Vector3 mov)
        {
            center = mov;
        }
        public void SetDirection(Vector3 dir)
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
        public MovementPoint GetPoint()
        {
            if (outPoints == null) return null;
            if (outPoints.Count < 1) return null;
            return outPoints[0];
        }
        public MovementPoint GetNextPoint()
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
                if (cross == (center + outPoints[i].center) / 2)
                {
                    Debug.DrawLine(center, outPoints[i].center, Color.green, 1000, depthtest);
                }
                else
                {
                    for (float time = 0; time < 1; time += 0.1f)
                    {
                        Debug.DrawLine(BezierCurve(center, cross, outPoints[i].center, time), BezierCurve(center, cross, outPoints[i].center, time + 0.1f), Color.green, 1000, depthtest);
                    }
                }
            }
        }

        public static MovementPoint[] Connect(MovementPoint[] be, MovementPoint[] ki, Vector3 beDir, Vector3 kiDir)
        {
            if (be == null || ki == null || be.Length == 0 || ki.Length == 0) return new MovementPoint[0];
            List<MovementPoint> output = new List<MovementPoint>();
            int kulonbseg = System.Math.Abs(be.Length - ki.Length);
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

        public static MovementPoint[] CurveAndConnect(MovementPoint be, MovementPoint ki, int iterationMax = 5)
        {
            Vector3 beDir = be.direction.normalized;
            Vector3 kiDir = ki.direction.normalized;
            if (iterationMax < 1 || Vector3.Angle(beDir, kiDir) < 30)
            {
                be.ConnectPoint(ki);
                return new MovementPoint[0];
            }
            float plus = 1f / iterationMax;
            Vector3 cross = MyMath.Intersect(be.center, beDir, ki.center, kiDir);
            List<MovementPoint> movementPoints = new List<MovementPoint>();
            for (float i = 0.1f; i < 1; i += plus)
            {
                MovementPoint point = new MovementPoint(
                    BezierCurve(be.center, cross, ki.center, i));
                point.SetDirection(DirectionBezierCurve(be.center, cross, ki.center, i));
                if (movementPoints.Count < 1) be.ConnectPoint(point);
                else movementPoints.Last().ConnectPoint(point);
                movementPoints.Add(point);
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

        public static Vector3 DirectionBezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, float t)
        {
            return 2 * (1 - t) * (P1 - P0) + 2 * t * (P2 - P1);
        }

    }
}
