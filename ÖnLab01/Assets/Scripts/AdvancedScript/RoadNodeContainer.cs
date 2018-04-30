using System.Collections.Generic;
using UnityEngine;


// This class Makes the Graph
public class RoadNodeContainer : MonoBehaviour {
    // when merging roadNodes we need plus roads too
    class PlusRoad
    {
        public PlusRoad(Vector3 _p, Vector3 _q)
        {
            p = _p; q = _q;
        }
        public Vector3 p;
        public Vector3 q;
    }
  
    public GameObject ControlPointsVisualationObject;
    public GameObject blockObject;
    public GameObject roadObject;
    public RoadGeneratingValues values;
    
    [Header("MainRoads max")]
    public int ReqursiveMax = 400;
    [Space(5)]
    [Header("SideRoads max")]
    public int ReqursiveMaxS = 1000;
    [Space(5)]
    [Header("Roads visalization")]
    public bool DebugLines = true;
    // Roads 
    List<RoadNode> roads = new List<RoadNode>();
    List<RoadNode> sideroads = new List<RoadNode>();
    // plus roads just for not to cross them
    List<PlusRoad> plusroads = new List<PlusRoad>();


    MainRoadObjGenerator generator = new MainRoadObjGenerator();
    // Use this for initialization
    void Start() {
        Debug.Log("PROCESS STARTING...");
        ClearStart();
        Invoke("Step01", 0);
        
    }

    // Clear All Data And create the first Road
    void ClearStart()
    {
        roads.Clear();
        sideroads.Clear();
        plusroads.Clear();
        RoadNode elso = new RoadNode(values.straightFreqMainRoad, values.maxCrossings, values.rotationRandomMainRoad);
        elso.position = new Vector3(0, 0, values.size.zMin + 3);
        roads.Add(elso);
    }

    // City Generating Steps
    void Step01()
    {
        Debug.Log("STEP01 -- Main Road Generating Started");
        GeneratingMainRoads();
        Debug.Log("STEP01 -- Main Road Generating Ended");
        Invoke("Step02", 0);
    }
    void Step02()
    {
        Debug.Log("STEP02 -- Side Road Generating Started");
        GeneratingFirstSideRoads();
        GeneratingMoreSideRoads();
        Debug.Log("STEP02 -- Side Road Generating Ended");
        Invoke("Step03", 0);
    }
    void Step03()
    {
        Debug.Log("STEP03 -- Smooth Started");
        //Cut();
        //Cut();
        SmoothRoads();
        if (DebugLines)
            Visualization01();
        Debug.Log("STEP03 -- Smooth Ended");
        Invoke("Step04",0);
    }
    void Step04()
    {
        Debug.Log("STEP04 -- Generating Blocks Started");
        
        List<RoadNode> all = new List<RoadNode>();
        all.AddRange(roads);
        all.AddRange(sideroads);
        circles = generator.GenerateCircles(all, blockObject,roadObject,values.roadSize);
        for (int i=0; i<circles.Count; i++)
        {
            Invoke("NextCircle", i *0.2f );
        }
        Debug.Log("STEP04 -- Generating Blocks Ended");
    }
    List<List<RoadNode>> circles;
    int z = 0;
    void NextCircle()
    {
        generator.MakeABlock(circles[z]);
        z++;
    }


    void Cut()
    {
        List<RoadNode> expell = new List<RoadNode>();
        foreach (RoadNode road in sideroads)
        {
            if (road.Szomszedok.Count < 2) {
                road.Szomszedok[0].removeSzomszed(road);
                //sideroads.Remove(road);
                expell.Add(road);
           }
        }
        foreach (RoadNode road in expell)
        {
            sideroads.Remove(road);
        }
        
    }
   // ROAD generating functions
    void GeneratingMainRoads()
    {
        for (int i=0; i<ReqursiveMax && i<roads.Count; i++)
        {
            RoadNode root = roads[i];
            List<RoadNode> newRoads = root.GenerateRoads(values.roadsDistancesMainRoad);
            foreach (RoadNode road in newRoads)
            {
                if (Ellenorzes(root, road, true))  roads.Add(road);
                else root.removeSzomszed(road);
            }
        }
    }
    void GeneratingFirstSideRoads()
    {
        sideroads.Clear();
        foreach(RoadNode road in roads)
        {
            List<RoadNode> ki = new List<RoadNode>();
            if (Random.value < values.sideRoadFreq)
                ki = road.GenerateSideRoads(values.roadsDistancesSideRoad, values.straightFreqSideRoad, values.rotationRandomSideRoad);
            
            foreach(RoadNode newroad in ki)
            {
                if (Ellenorzes(road,newroad, false)) sideroads.Add(newroad);
                else road.removeSzomszed(newroad);
            }


        }
    }
    void GeneratingMoreSideRoads()
    {
        for (int i=0; i<ReqursiveMaxS && i<sideroads.Count; i++)
        {
            RoadNode current_road = sideroads[i];
            List<RoadNode> newRoads = current_road.GenerateRoads(values.roadsDistancesSideRoad);

            foreach (RoadNode newroad in newRoads)
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

    // Correction functions
    bool PalyanBelulVane(RoadNode r)
    {
        return !(r.position.x < values.size.xMin || r.position.x > values.size.xMax
              || r.position.z < values.size.zMin || r.position.z > values.size.zMax);
    }
    bool Ellenorzes(RoadNode current_road,RoadNode newroad, bool Javitassal)
    {
        
        if (!PalyanBelulVane(newroad)) return false;
        
        foreach (RoadNode other_road in sideroads)
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
        foreach (RoadNode other_road in roads)
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
        foreach (RoadNode other_road in sideroads)
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
        foreach (RoadNode other_road in roads)
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

    // roads smoothing
    void SmoothRoads()
    {
        foreach (RoadNode road in roads)
        {
            road.Smooth(values.smootIntensity);
        }
        foreach (RoadNode road in sideroads)
        {
            road.Smooth(values.smootIntensity);
        }
    }

    // roads temporary visualization
    void Visualization01()
    {
        foreach (RoadNode road in roads)
        {
            if (ControlPointsVisualationObject != null)
            {
                GameObject ki = Instantiate(ControlPointsVisualationObject);
                ki.transform.position = road.position;
            }
            road.DrawLines(Color.red);
        }
        foreach (RoadNode road in sideroads)
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
