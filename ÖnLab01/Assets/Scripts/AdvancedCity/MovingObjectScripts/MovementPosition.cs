using UnityEngine;
using System.Collections;
namespace Assets.Scripts.AdvancedCity
{
    [System.Serializable]
    public class MovementPosition : System.Object
    {
        public Vector3 pos;
        public MovementPoint nextPoint;
        public MovementPoint prevPoint;
        public float Step(float speed)
        {
            Vector3 direction = nextPoint.center - pos;
            if (direction.magnitude < speed || nextPoint == prevPoint)
            {
                while (direction.magnitude < speed && nextPoint != prevPoint)
                {
                    GetNextPoint();
                    direction = nextPoint.center - pos;
                }
                if (nextPoint == null) nextPoint = prevPoint;
                if (nextPoint == prevPoint) speed = 0;
            }
            pos = pos + direction.normalized * speed;
            prevPoint.Draw(true, 1f);
            return speed;
        }
        private float time = 0f;
        public float BezierStep(float speed)
        {
            if (nextPoint == prevPoint)
            {
                GetNextPoint();
                pos = prevPoint.center;
                return 0;
            }
            float length = (nextPoint.center - prevPoint.center).magnitude;
            float step = speed / length;
            time += step;
            if (time >= 1f)
            {
                GetNextPoint();
                time = 0;
                pos = prevPoint.center;
                return speed;
            }
            Vector3 cross;
            Vector3 nDir = nextPoint.direction;
            Vector3 nCenter = nextPoint.center;
            Vector3 eDir = prevPoint.direction;
            Vector3 eCenter = prevPoint.center;
            if (Vector3.Angle(eDir, nDir) < 30)
            {
                cross = (eCenter + nCenter) / 2;
            }
            else
            {
                cross = MyMath.Intersect(eCenter, eDir, nCenter, nDir);
            }
            pos = MovementPoint.BezierCurve(eCenter, cross, nCenter, time);
            prevPoint.Draw(true, 1f);
            return speed;
        }

        public Quaternion GetBezierDir()
        {
            if (nextPoint == prevPoint) return Quaternion.LookRotation(Forward);
            Vector3 cross;
            Vector3 nDir = nextPoint.direction;
            Vector3 nCenter = nextPoint.center;
            Vector3 eDir = prevPoint.direction;
            Vector3 eCenter = prevPoint.center;
            if (Vector3.Angle(eDir, nDir) < 30)
            {
                cross = (eCenter + nCenter) / 2;
                return Quaternion.LookRotation(Forward);
            }
            else
            {
                cross = MyMath.Intersect(eCenter, eDir, nCenter, nDir);
                return Quaternion.LookRotation(MovementPoint.DirectionBezierCurve(eCenter, cross, nCenter, time));
            }
        }

        void GetNextPoint()
        {
            prevPoint = nextPoint;
            nextPoint = nextPoint.GetNextPoint();
            if (nextPoint == null) nextPoint = prevPoint;
        }
        public bool Stopping(float speed)
        {
            float length = (nextPoint.center - pos).magnitude;
            return nextPoint.stopping && (length < speed);
        }
        public Vector3 Forward
        {
            get
            {
                return nextPoint.direction;
            }
        }
    }
}
