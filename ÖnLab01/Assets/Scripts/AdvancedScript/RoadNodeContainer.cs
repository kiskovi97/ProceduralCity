using System.Collections.Generic;
using UnityEngine;

public class RoadNodeContainer : MonoBehaviour {

    class PlusRoad
    {
        public PlusRoad(Vector3 _p, Vector3 _q)
        {
            p = _p; q = _q;
        }
        public Vector3 p;
        public Vector3 q;
    }
  
    public GameObject visual;
    public GameObject blockObject;
    public RoadGeneratingValues values;

    // rekurziv adatok
    public int ReqursiveMax = 400;
    public int ReqursiveMaxS = 1000;
    List<RoadNode2> roads = new List<RoadNode2>();
    List<RoadNode2> sideroads = new List<RoadNode2>();
    List<PlusRoad> plusroads = new List<PlusRoad>();
    MainRoadObjGenerator generator = new MainRoadObjGenerator();
    int rootObjIndex = 0;
    int rootObjIndexS = 0;
    // Use this for initialization
    void Start() {
        roads.Clear();
        RoadNode2 elso = new RoadNode2(values.straightFreqMainRoad, values.maxCrossings, values.rotationRandomMainRoad, 2);
        elso.SetPosition(new Vector3(0, 0, values.size.zMin + 1));
        roads.Add(elso);
        Invoke("Step01", 1);
        
    }

    void Step01()
    {
        GeneratingMainRoads();
        Debug.Log("End of main road generation");
        Invoke("Step02", 2);
    }
    void Step02()
    {
        GeneratingStartSideRoads();
        Invoke("Step03", 2);
    }
    void Step03()
    {
        GeneratingMoreSideRoads();
        SmoothRoads();
        Visualization01();
        Invoke("Step04",3.5f);
    }
    void Step04()
    {
        List<RoadNode2> all = new List<RoadNode2>();
        Debug.Log("NEXT");
        all.AddRange(roads);
        all.AddRange(sideroads);
        generator.GenerateCircles(all, blockObject);
    }

    void Visualization01()
    {
        foreach (RoadNode2 road in roads)
        {
            GameObject ki = Instantiate(visual);
            ki.transform.position = road.position;
            road.DrawLines(Color.red);
        }
        foreach (RoadNode2 road in sideroads)
        {
            GameObject ki = Instantiate(visual);
            ki.transform.position = road.position;
            road.DrawLines(Color.yellow);
        }

    }
    bool PalyanBelulVane(RoadNode2 r)
    {
        if (r.position.x < values.size.xMin || r.position.x > values.size.xMax)
        {
            return false;
        }
        if (r.position.z < values.size.zMin || r.position.z > values.size.zMax)
        {
            return false;
        }
        return true;
    }
    void GeneratingMainRoads(){
        ReqursiveMax--;
        if (ReqursiveMax < 0) return;
        if (rootObjIndex < roads.Count)
        {
            RoadNode2 root = roads[rootObjIndex];
            List<RoadNode2> newRoads = root.GenerateRoads(values.roadsDistancesMainRoad);
            foreach (RoadNode2 road in newRoads)
            {
                if (Ellenorzes(root,road, true))
                {
                    roads.Add(road);
                }
                else
                {
                    root.removeSzomszed(road);
                }

            }
            
            rootObjIndex++;
            GeneratingMainRoads();
        }
        else
        {
            return;
        }
    }
	void GeneratingStartSideRoads()
    {
        sideroads.Clear();
        foreach(RoadNode2 road in roads)
        {
            List<RoadNode2> ki = new List<RoadNode2>();
            if (Random.value < values.sideRoadFreq)
            {
                ki = road.GenerateSideRoads(values.roadsDistancesSideRoad, values.straightFreqSideRoad, values.rotationRandomSideRoad);
            }
            foreach(RoadNode2 newroad in ki)
            {
                
                if (Ellenorzes(road,newroad, false))
                {
                    sideroads.Add(newroad);
                }
                else
                {
                    road.removeSzomszed(newroad);
                }
            }


        }
    }
    void GeneratingMoreSideRoads()
    {
        if (rootObjIndexS >= sideroads.Count)
        {
            return;
        }

        RoadNode2 current_road = sideroads[rootObjIndexS];
        List<RoadNode2> newRoads = current_road.GenerateRoads(values.roadsDistancesSideRoad);

        foreach (RoadNode2 newroad in newRoads)
        {
            if (Ellenorzes(current_road,newroad, true))
            {
                sideroads.Add(newroad);
            }
            else
            {
                current_road.removeSzomszed(newroad);
            }
        }


        if (ReqursiveMaxS > 0)
        {
            ReqursiveMaxS--;
            rootObjIndexS++;
            GeneratingMoreSideRoads();
        }

    }
    bool Ellenorzes(RoadNode2 current_road,RoadNode2 newroad, bool Javitassal)
    {
        
        if (!PalyanBelulVane(newroad)) return false;
        
        foreach (RoadNode2 other_road in sideroads)
        {
            if ((newroad.position - other_road.position).sqrMagnitude < values.collapseRangeSideRoad)
            {
                if (Javitassal)
                {
                    if (KeresztEllenorzes(current_road.position, other_road.position))
                    {
                        current_road.Csere(other_road, newroad);
                        other_road.addSzomszed(current_road);
                        plusroads.Add(new PlusRoad(current_road.position, other_road.position));
                    }
                }

                return false;
            }
        }
        foreach (RoadNode2 other_road in roads)
        {
            if ((newroad.position - other_road.position).sqrMagnitude < values.collapseRangeMainRoad)
            {
                if (Javitassal)
                {
                    if (KeresztEllenorzes(current_road.position, other_road.position))
                    {
                        // -- Javitas --
                        current_road.Csere(other_road, newroad);
                        other_road.addSzomszed(current_road);
                        plusroads.Add(new PlusRoad(current_road.position, other_road.position));
                    }
                }
                return false;
            }
        }
        return KeresztEllenorzes(current_road.position,newroad.position);
    }
    bool KeresztEllenorzes(Vector3 p1 , Vector3 q1)
    {
        foreach (PlusRoad plusroad in plusroads)
        {
            if (p1 == plusroad.p || p1 == plusroad.q || plusroad.q == q1 || plusroad.p == q1) continue;
            if (SegmentFunctions.doIntersect(p1, q1, plusroad.p, plusroad.q))
            {
                return false;
            }
        }
        foreach (RoadNode2 other_road in sideroads)
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
        foreach (RoadNode2 other_road in roads)
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
    void SmoothRoads()
    {
        foreach (RoadNode2 road in roads)
        {
            road.Smooth(values.smootIntensity);
        }
        foreach (RoadNode2 road in sideroads)
        {
            road.Smooth(values.smootIntensity);
        }
    }
}
