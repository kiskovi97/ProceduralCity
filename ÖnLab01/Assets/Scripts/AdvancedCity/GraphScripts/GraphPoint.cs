
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    public class GraphPoint
    {
        // ---------------- Basic Initialization -----------
        public Vector3 position;
        public enum POINTTYPE { MAIN, SIDE, TRAM, TRAM_MAIN };
        protected POINTTYPE type;
        public bool IsMainRoad()
        {
            return type == POINTTYPE.MAIN || type == POINTTYPE.TRAM_MAIN;
        }
        public bool IsSideRoad()
        {
            return type == POINTTYPE.SIDE || type == POINTTYPE.TRAM;
        }
        public bool IsTram()
        {
            return type == POINTTYPE.TRAM || type == POINTTYPE.TRAM_MAIN;
        }
        public void SetAsSideRoad()
        {
            type = POINTTYPE.SIDE;
        }
        public void SetAsTram()
        {
            if (type == POINTTYPE.MAIN) type = POINTTYPE.TRAM_MAIN;
            if (type == POINTTYPE.SIDE) type = POINTTYPE.TRAM;
        }
        public void SetType(POINTTYPE in_type)
        {
            type = in_type;
        }
        // elso szomszed az a pont ami letrehozta
        protected List<GraphPoint> neighbours;
        public List<GraphPoint> Neighbours
        {
            get
            {
                List<GraphPoint> uj = new List<GraphPoint>();
                uj.AddRange(neighbours);
                return uj;
            }
        }
        public void AddNeighbour(GraphPoint point)
        {
            if (!neighbours.Contains(point))
                neighbours.Add(point);
        }
        public void RemoveNeighbour(GraphPoint point)
        {
            if (neighbours.Contains(point))
                neighbours.Remove(point);
        }
        public void SetBefore(GraphPoint point)
        {
            if (neighbours == null) neighbours = new List<GraphPoint>();

            if (neighbours.Count == 0)
                neighbours.Add(point);
            else
                neighbours[0] = point;
        }
        public GraphPoint GetBefore()
        {
            if (neighbours == null) return null;
            if (neighbours.Count < 1) return null;
            return neighbours[0];
        }
        public void SwitchNeigbours(GraphPoint newPoint, GraphPoint oldPoint)
        {
            if (neighbours == null) return;
            if (neighbours.Contains(oldPoint))
            {
                int index = neighbours.IndexOf(oldPoint);
                if (neighbours.Contains(newPoint))
                {
                    neighbours.Remove(oldPoint);
                }
                else neighbours[index] = newPoint;
            }
            else return;

        }
        // ora mutato jarasaba rendez
        public GraphPoint()
        {
            neighbours = new List<GraphPoint>();
        }

        public GraphPoint Next(GraphPoint beforePoint, bool right)
        {
            if (neighbours == null) return null;
            if (neighbours.Count < 1) return null;
            if (neighbours.Count == 1) return neighbours[0];
            if (beforePoint == null)
            {
                return neighbours[0];
            }
            if (!neighbours.Contains(beforePoint)) { return null; }

            GraphPoint output = neighbours[0];
            Vector3 beforeDirection = (beforePoint.position - position).normalized;
            float angleNow;
            if (right)
                angleNow = 360;
            else
                angleNow = -360;
            foreach (GraphPoint point in neighbours)
            {
                if (point == beforePoint) continue;
                Vector3 nextDirection = (point.position - position).normalized;
                float newAngle = Vector3.SignedAngle(beforeDirection, nextDirection, Vector3.up);
                if (right)
                {
                    if (newAngle < 0) newAngle += 360;
                }
                else
                {
                    if (newAngle > 0) newAngle -= 360;
                }
                if (angleNow > newAngle && right)
                {
                    output = point;
                    beforeDirection = beforePoint.position - position;
                    angleNow = newAngle;
                }
                if (angleNow < newAngle && !right)
                {
                    output = point;
                    beforeDirection = beforePoint.position - position;
                    angleNow = newAngle;
                }
            }
            if (output == beforePoint) return null;
            return output;
        }

        public void DrawLines(Color color, bool depthtest)
        {
            foreach (GraphPoint neighbour in neighbours)
            {
                Debug.DrawLine(position, neighbour.position, color, 100, depthtest);
            }
        }

        public void DrawLinesHandler(Color color, bool depthtest)
        {
            foreach (GraphPoint neighbour in neighbours)
            {
                Handles.DrawLine(position, neighbour.position);
            }
        }
    }
}
