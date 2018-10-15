
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    [RequireComponent(typeof(RoadGeneratingValues))]
    class GraphGenerator : MonoBehaviour
    {
        public GameObject ControlPointsVisualationObject;
        private RoadGeneratingValues values;
        public void SetValues(RoadGeneratingValues values)
        {
            this.values = values;
        }
        [Header("MainRoads max")]
        public int ReqursiveMax = 400;
        [Space(5)]
        [Header("SideRoads max")]
        public int ReqursiveMaxS = 1000;
        [Space(5)]
        [Header("Roads visalization")]
        public bool DebugLines = true;
        class PlusEdge
        {
            public PlusEdge(Vector3 p, Vector3 q)
            {
                this.one = p; this.other = q;
            }
            public Vector3 one;
            public Vector3 other;
        }

        List<InteractiveGraphPoint> mainPoints = new List<InteractiveGraphPoint>();
        List<InteractiveGraphPoint> sidePoints = new List<InteractiveGraphPoint>();
        // plus roads just for not to cross them
        List<PlusEdge> plusroads = new List<PlusEdge>();
        List<GraphPoint> circle;

        void Start()
        {
            values = GetComponent<RoadGeneratingValues>();
            if (values == null) throw new System.Exception("No Values Binded");
        }

        public List<GraphPoint> GenerateGraph(bool visual = false, bool depth = false)
        {
            ClearStart();
            GeneratingMainRoads();
            GeneratingFirstSideRoads();
            GeneratingMoreSideRoads();
            if (visual)
            {
                Visualization(depth);
            }
            ClearDeadEnds();
            List<GraphPoint> output = new List<GraphPoint>();
            foreach (InteractiveGraphPoint point in mainPoints)
            {
                point.Order();
                output.Add(point);
            }
            foreach (InteractiveGraphPoint point in sidePoints)
            {
                point.Order();
                output.Add(point);
            }
            CircleGenerator circleGenerator = new CircleGenerator();
            circle = circleGenerator.MaxCircle(output);
            foreach (GraphPoint point in circle)
            {
                point.SetAsTram();
            }
            return output;
        }

        public void EditorDraw()
        {
            foreach (InteractiveGraphPoint point in mainPoints)
            {
                if (ControlPointsVisualationObject != null)
                {
                    GameObject ki = Instantiate(ControlPointsVisualationObject);
                    ki.transform.position = point.position;
                }
                point.DrawLinesHandler(Color.red, false);
            }
            foreach (InteractiveGraphPoint point in sidePoints)
            {
                if (ControlPointsVisualationObject != null)
                {
                    GameObject ki = Instantiate(ControlPointsVisualationObject);
                    ki.transform.position = point.position;
                }
                point.DrawLinesHandler(Color.yellow, false);
            }
        }

        public void Visualization(bool depthtest)
        {
            foreach (InteractiveGraphPoint point in mainPoints)
            {
                if (ControlPointsVisualationObject != null)
                {
                    GameObject ki = Instantiate(ControlPointsVisualationObject);
                    ki.transform.position = point.position;
                }
                point.DrawLines(Color.red, depthtest);
            }
            foreach (InteractiveGraphPoint point in sidePoints)
            {
                if (ControlPointsVisualationObject != null)
                {
                    GameObject ki = Instantiate(ControlPointsVisualationObject);
                    ki.transform.position = point.position;
                }
                point.DrawLines(Color.yellow, depthtest);
            }

        }

        void ClearStart()
        {
            mainPoints.Clear();
            sidePoints.Clear();
            plusroads.Clear();
        }

        void GeneratingMainRoads()
        {
            InteractiveGraphPoint elso = new InteractiveGraphPoint
            {
                position = values.StartingPoint()
            };
            mainPoints.Add(elso);

            for (int i = 0; i < ReqursiveMax && i < mainPoints.Count; i++)
            {
                InteractiveGraphPoint root = mainPoints[i];
                List<InteractiveGraphPoint> newRoads = root.GeneratePoints(values.roadsDistancesMainRoad, values.straightFreqMainRoad, values.rotationRandomMainRoad, values.maxCrossings);
                foreach (InteractiveGraphPoint road in newRoads)
                {
                    if (Check(root, road, true)) mainPoints.Add(road);
                    else root.RemoveNeighbour(road);
                }
                if (mainPoints.Count < 2)
                {
                    ReqursiveMax--;
                    i--;
                }
            }
        }
        void GeneratingFirstSideRoads()
        {
            sidePoints.Clear();
            foreach (InteractiveGraphPoint point in mainPoints)
            {
                List<InteractiveGraphPoint> output = new List<InteractiveGraphPoint>();
                if (Random.value < values.sideRoadFreq)
                    output = point.GenerateSidePoints(values.roadsDistancesSideRoad);

                foreach (InteractiveGraphPoint newPoint in output)
                {
                    if (Check(point, newPoint, false)) sidePoints.Add(newPoint);
                    else point.RemoveNeighbour(newPoint);
                }


            }
        }
        void GeneratingMoreSideRoads()
        {
            for (int i = 0; i < ReqursiveMaxS && i < sidePoints.Count; i++)
            {
                InteractiveGraphPoint currentPoint = sidePoints[i];
                List<InteractiveGraphPoint> newPoints = currentPoint.GeneratePoints(values.roadsDistancesSideRoad, values.straightFreqSideRoad, values.rotationRandomSideRoad, values.maxCrossings);

                foreach (InteractiveGraphPoint newPoint in newPoints)
                {
                    if (Check(currentPoint, newPoint, true))
                    {
                        sidePoints.Add(newPoint);
                    }
                    else
                    {
                        currentPoint.RemoveNeighbour(newPoint);
                    }
                }
            }

        }

        readonly long maxAngle = 60;

        bool Check(GraphPoint currentPoint, GraphPoint newPoint, bool withRepairs)
        {

            if (!values.WithinRange(newPoint.position)) return false;

            foreach (GraphPoint otherPoint in sidePoints)
            {
                if (otherPoint != newPoint && (newPoint.position - otherPoint.position).magnitude < values.collapseRangeSideRoad)
                {
                    if (withRepairs && Check(currentPoint, otherPoint, false))
                    {
                        if (CrossingCheck(currentPoint.position, otherPoint.position))
                        {
                            currentPoint.SwitchNeigbours(otherPoint, newPoint);
                            otherPoint.AddNeighbour(currentPoint);
                            plusroads.Add(new PlusEdge(currentPoint.position, otherPoint.position));
                        }
                    }
                    return false;
                }
            }
            foreach (GraphPoint otherPoint in mainPoints)
            {
                if (otherPoint != newPoint && (newPoint.position - otherPoint.position).magnitude < values.collapseRangeMainRoad)
                {
                    if (withRepairs && Check(currentPoint, otherPoint, false))
                    {
                        if (CrossingCheck(currentPoint.position, otherPoint.position))
                        {
                            currentPoint.SwitchNeigbours(otherPoint, newPoint);
                            otherPoint.AddNeighbour(currentPoint);
                            plusroads.Add(new PlusEdge(currentPoint.position, otherPoint.position));
                        }
                    }
                    return false;
                }
            }
            return CrossingCheck(currentPoint.position, newPoint.position);
        }

        bool CrossingCheck(PlusEdge road, Vector3 one, Vector3 other)
        {
            if (one == road.one)
            {
                if (other == road.other) return true;
                float angle = Vector3.Angle(other - one, road.other - one);
                angle = Mathf.Abs(angle);
                if (angle < maxAngle) return false;
                return true;
            }
            if (one == road.other)
            {
                if (other == road.one) return true;
                float angle = Vector3.Angle(other - one, road.one - one);
                angle = Mathf.Abs(angle);
                if (angle < maxAngle) return false;
                return true;
            }
            if (road.other == other)
            {
                if (one == road.one) return true;
                float angle = Vector3.Angle(one - other, road.one - other);
                angle = Mathf.Abs(angle);
                if (angle < maxAngle) return false;
                return true;
            }
            if (road.one == other)
            {
                if (one == road.other) return true;
                float angle = Vector3.Angle(one - other, road.other - other);
                angle = Mathf.Abs(angle);
                if (angle < maxAngle) return false;
                return true;
            }

            if (SegmentFunctions.DoIntersect(one, other, road.one, road.other))
            {
                return false;
            }
            return true;
        }

        bool CrossingCheck(Vector3 one, Vector3 other)
        {
            if ((one - other).magnitude < values.CollapseSideRoad) return false;
            foreach (PlusEdge road in plusroads)
            {
                if (!CrossingCheck(road, one, other)) return false;
                
            }
            foreach (GraphPoint point in sidePoints)
            {

                if (point.GetBefore() == null) continue;
                Vector3 p2 = point.GetBefore().position;
                Vector3 q2 = point.position;
                if (!CrossingCheck(new PlusEdge(p2, q2), one, other)) return false;
            }
            foreach (GraphPoint point in mainPoints)
            {

                if (point.GetBefore() == null) continue;
                Vector3 p2 = point.GetBefore().position;
                Vector3 q2 = point.position;
                if (!CrossingCheck(new PlusEdge(p2, q2), one, other)) return false;
            }


            return true;
        }


        void SmoothGraph()
        {
            foreach (InteractiveGraphPoint road in mainPoints)
            {
                road.Smooth(values.smootIntensity);
            }
            foreach (InteractiveGraphPoint road in sidePoints)
            {
                road.Smooth(values.smootIntensity);
            }
        }

        void ClearDeadEnds()
        {
            if (mainPoints.Count == 0 && sidePoints.Count == 0) return;
            bool torles = false;
            List<InteractiveGraphPoint> list = new List<InteractiveGraphPoint>();
            foreach (InteractiveGraphPoint point in mainPoints)
            {
                if (point.IsDeadEnd())
                {
                    point.RemoveFromNeighbours();
                    torles = true;
                    list.Add(point);
                }
            }
            foreach (InteractiveGraphPoint point in list)
            {
                mainPoints.Remove(point);
            }
            list.Clear();
            foreach (InteractiveGraphPoint point in sidePoints)
            {
                if (point.IsDeadEnd())
                {
                    point.RemoveFromNeighbours();
                    torles = true;
                    list.Add(point);
                }
            }
            foreach (InteractiveGraphPoint point in list)
            {
                sidePoints.Remove(point);
            }
            list.Clear();
            if (torles) ClearDeadEnds();
        }
    }
}

