
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class GraphGenerator : MonoBehaviour
    {
        public GameObject ControlPointsVisualationObject;
        public RoadGeneratingValues values;
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
            public PlusEdge(Vector3 _p, Vector3 _q)
            {
                p = _p; q = _q;
            }
            public Vector3 p;
            public Vector3 q;
        }
        
        void Start()
        {
            ClearStart();
            GenerateGraph();
            Visualization01();
        }
        void GenerateGraph()
        {
            ClearStart();
            Debug.Log("STEP01 -- Main Road Generating Started");
            GeneratingMainRoads();
            Debug.Log("STEP01 -- Main Road Generating Ended");
            Debug.Log("STEP02 -- Side Road Generating Started");
            GeneratingFirstSideRoads();
            GeneratingMoreSideRoads();
            Debug.Log("STEP02 -- Side Road Generating Ended");
        }
        void ClearStart()
        {
            roads.Clear();
            sideroads.Clear();
            plusroads.Clear();
        }

        
        List<GraphPoint> roads = new List<GraphPoint>();
        List<GraphPoint> sideroads = new List<GraphPoint>();
        // plus roads just for not to cross them
        List<PlusEdge> plusroads = new List<PlusEdge>();

        void GeneratingMainRoads()
        {
            GraphPoint elso = new GraphPoint();
            elso.position = new Vector3(0, 0, values.size.zMin + 2);
            roads.Add(elso);

            for (int i = 0; i < ReqursiveMax && i < roads.Count; i++)
            {
                GraphPoint root = roads[i];
                List<GraphPoint> newRoads = root.generatePoints(values.roadsDistancesMainRoad,values.straightFreqMainRoad,values.rotationRandomMainRoad,values.maxCrossings);
                foreach (GraphPoint road in newRoads)
                {
                    if (Ellenorzes(root, road, true)) roads.Add(road);
                    else root.removeSzomszed(road);
                }
                if (roads.Count < 2)
                {
                    ReqursiveMax--;
                    i--;
                }
            }
        }
        void GeneratingFirstSideRoads()
        {
            sideroads.Clear();
            foreach (GraphPoint road in roads)
            {
                List<GraphPoint> ki = new List<GraphPoint>();
                if (Random.value < values.sideRoadFreq)
                    ki = road.generateSidePoints(values.roadsDistancesSideRoad);

                foreach (GraphPoint newroad in ki)
                {
                    if (Ellenorzes(road, newroad, false)) sideroads.Add(newroad);
                    else road.removeSzomszed(newroad);
                }


            }
        }
        void GeneratingMoreSideRoads()
        {
            for (int i = 0; i < ReqursiveMaxS && i < sideroads.Count; i++)
            {
                GraphPoint current_road = sideroads[i];
                List<GraphPoint> newRoads = current_road.generatePoints(values.roadsDistancesSideRoad, values.straightFreqSideRoad, values.rotationRandomSideRoad,values.maxCrossings);

                foreach (GraphPoint newroad in newRoads)
                {
                    if (Ellenorzes(current_road, newroad, true))
                    {
                        sideroads.Add(newroad);
                    }
                    else
                    {
                        current_road.removeSzomszed(newroad);
                    }
                }
            }

        }

        bool Ellenorzes(GraphPoint current_road, GraphPoint newroad, bool Javitassal)
        {

            if (!PalyanBelulVane(newroad)) return false;

            foreach (GraphPoint other_road in sideroads)
            {
                if ((newroad.position - other_road.position).magnitude < values.collapseRangeSideRoad)
                {
                    if (Javitassal)
                    {
                        if (KeresztEllenorzes(current_road.position, other_road.position))
                        {
                            current_road.csere(other_road, newroad);
                            other_road.addSzomszed(current_road);
                            plusroads.Add(new PlusEdge(current_road.position, other_road.position));
                        }
                    }

                    return false;
                }
            }
            foreach (GraphPoint other_road in roads)
            {
                if ((newroad.position - other_road.position).magnitude < values.collapseRangeMainRoad)
                {
                    if (Javitassal)
                    {
                        if (KeresztEllenorzes(current_road.position, other_road.position))
                        {
                            // -- Javitas --
                            current_road.csere(other_road, newroad);
                            other_road.addSzomszed(current_road);
                            plusroads.Add(new PlusEdge(current_road.position, other_road.position));
                        }
                    }
                    return false;
                }
            }
            return KeresztEllenorzes(current_road.position, newroad.position);
        }
        bool KeresztEllenorzes(Vector3 p1, Vector3 q1)
        {
            if ((p1 - q1).magnitude < values.CollapseSideRoad) return false;
            foreach (PlusEdge plusroad in plusroads)
            {
                if (p1 == plusroad.p || p1 == plusroad.q || plusroad.q == q1 || plusroad.p == q1) continue;
                if (SegmentFunctions.doIntersect(p1, q1, plusroad.p, plusroad.q))
                {
                    return false;
                }
            }
            foreach (GraphPoint other_road in sideroads)
            {

                if (other_road.getElozo() == null) continue;
                Vector3 p2 = other_road.getElozo().position;
                Vector3 q2 = other_road.position;
                if (p1 == p2 || p1 == q2 || q1 == p2 || q1 == q2) continue;
                if (SegmentFunctions.doIntersect(p1, q1, p2, q2))
                {
                    return false;
                }
            }
            foreach (GraphPoint other_road in roads)
            {

                if (other_road.getElozo() == null) continue;
                Vector3 p2 = other_road.getElozo().position;
                Vector3 q2 = other_road.position;
                if (p1 == p2 || p1 == q2 || q1 == p2 || q1 == q2) continue;
                if (SegmentFunctions.doIntersect(p1, q1, p2, q2))
                {
                    return false;
                }
            }


            return true;
        }
        bool PalyanBelulVane(GraphPoint r)
        {
            return !(r.position.x < values.size.xMin || r.position.x > values.size.xMax
                  || r.position.z < values.size.zMin || r.position.z > values.size.zMax);
        }

        void SmoothGraph()
        {
            foreach (GraphPoint road in roads)
            {
                road.Smooth(values.smootIntensity);
            }
            foreach (GraphPoint road in sideroads)
            {
                road.Smooth(values.smootIntensity);
            }
        }
        void Visualization01()
        {
            foreach (GraphPoint road in roads)
            {
                if (ControlPointsVisualationObject != null)
                {
                    GameObject ki = Instantiate(ControlPointsVisualationObject);
                    ki.transform.position = road.position;
                }
                road.DrawLines(Color.red);
            }
            foreach (GraphPoint road in sideroads)
            {
                if (ControlPointsVisualationObject != null)
                {
                    GameObject ki = Instantiate(ControlPointsVisualationObject);
                    ki.transform.position = road.position;
                }
                road.DrawLines(Color.yellow);
            }

        }
    }
}

