using UnityEngine;
using System.Collections.Generic;
namespace Assets.Scripts.AdvancedCity
{
    public class MovementPointContainer : MonoBehaviour
    {
        public List<MovementPoint> points = new List<MovementPoint>();

        public void SetPoints(List<MovementPoint> points)
        {
            this.points = points;
        }

        public void AddPoints(IEnumerable<MovementPoint> inputPoints)
        {
            //this.points.AddRange(points);
            foreach (MovementPoint point in inputPoints)
            {
                if (point == null) return;
                if (!this.points.Contains(point))
                {
                    point.ID = points.Count;
                    points.Add(point);
                } else
                {
                    point.ID = points.IndexOf(point);
                }
            }
        }

        public MovementPoint GetMovementPoint(int id)
        {
            if (id > points.Count - 1) Debug.Log("ERROR");
            return points[id];
        }

        public void SetIDs()
        {
            foreach (MovementPoint point in points)
            {
                if (point == null) return;
                point.SetNextIDs(points);
            }
        }

        // Use this for initialization
        void Awake()
        {
            foreach (MovementPoint point in points)
            {
                if (point == null) return;
                point.SetNexts(points);
            }
            /*foreach (MovementPoint point in points)
            {
                if (point != null)
                    point.Draw(false);
            }*/
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
