using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNodeContainer : MonoBehaviour {
  
    public GameObject visual;
    List<RoadNode2> roads = new List<RoadNode2>();
    List<RoadNode2> sideroads = new List<RoadNode2>();

    int rootObjIndex = 0;
    int rootObjIndexS = 0;
    // Palya merete
    public float xMin = -20;
    public float zMin = -20;
    public float xMax = 20;
    public float zMax = 20;
    // merge adatok
    public float kozelseg = 0.3f;
    public float kozelsegS = 0.1f;
    public float RoadsDistances = 2;
    public float SRoadsDistances = 0.5f;
    // main road adatok
    public float straightFreq = 0.9f;
    public int MaxElagazas = 4;
    public float RotationRandom = 0.2f;
    // rekurziv adatok
    public int ReqursiveMax = 300;
    public int ReqursiveMaxS = 600;
    // side road adatok
    public float straightFreqS = 0.9f;
    public float RotationRandomS = 0.1f;
    public float SideRoadfreq = 0.2f;
    // Smooth
    public float smootIntensity = 0.1f;
    // Use this for initialization
    void Start() {
        roads.Clear();
        RoadNode2 elso = new RoadNode2(straightFreq, MaxElagazas, RotationRandom, 2);
        elso.SetPosition(new Vector3(0, 0, zMin + 1));
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
        Debug.Log("End of start side road generation");
        Invoke("Step03", 2);
    }
    void Step03()
    {
        GeneratingMoreSideRoads();
        Debug.Log("End of side road generation");
        SmoothRoads();
        Debug.Log("End of Smooth");
        Visualization01();
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
        if (r.position.x < xMin || r.position.x > xMax)
        {
            return false;
        }
        if (r.position.z < zMin || r.position.z > zMax)
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
            if (PalyanBelulVane(root))
            {
                List<RoadNode2> newRoads = root.GenerateRoads(RoadsDistances);
                foreach (RoadNode2 road in newRoads)
                {
                    bool oks = true;
                    foreach (RoadNode2 other_road in roads)
                    {
                        if ((road.position - other_road.position).sqrMagnitude < kozelseg)
                        {
                            root.Csere(other_road, road);
                            other_road.addSzomszed(root);
                            oks = false;
                            break;
                        }
                    }
                    if (oks)
                    {
                        roads.Add(road);
                    }
                    else
                    {
                        root.removeSzomszed(road);
                    }

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
            if (Random.value < SideRoadfreq)
            {
                ki = road.GenerateSideRoads(SRoadsDistances, straightFreqS,RotationRandomS);
            }
            foreach(RoadNode2 sideroad in ki)
            {
                bool oks = true;
                foreach (RoadNode2 other_road in roads)
                {
                    if ((sideroad.position - other_road.position).sqrMagnitude < kozelsegS)
                    {
                        oks = false;
                        break;
                    }
                }
                foreach (RoadNode2 other_road in sideroads)
                {
                    if ((sideroad.position - other_road.position).sqrMagnitude < kozelsegS)
                    {
                        oks = false;
                        break;
                    }
                }
                if (oks)
                {
                    sideroads.Add(sideroad);
                }
                else
                {
                    road.removeSzomszed(sideroad);
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
        List<RoadNode2> newRoads = current_road.GenerateRoads(SRoadsDistances);

        foreach (RoadNode2 newroad in newRoads)
        {
            bool ok = true;
            if (!PalyanBelulVane(newroad))
            {
                current_road.removeSzomszed(newroad);
                continue;
            }
            foreach (RoadNode2 other_road in roads)
            {
                if ((newroad.position - other_road.position).sqrMagnitude < kozelsegS)
                {
                    current_road.Csere(other_road, newroad);
                    other_road.addSzomszed(current_road);
                    ok = false;
                    break;
                }
                Vector3 p1 = current_road.position;
                if (other_road.getElozo() == null) continue;
                Vector3 p2 = other_road.getElozo().position;
                Vector3 q1 = newroad.position;
                Vector3 q2 = other_road.position;
                if (p1 == p2 || p1 == q2 || p2 == q1) continue;
                if (SegmentFunctions.doIntersect(p1, q1, p2, q2))
                {
                    ok = false;
                    break;
                }
            }
                
            if (ok)
            foreach (RoadNode2 other_road in sideroads)
            {
                if ((newroad.position - other_road.position).sqrMagnitude < kozelsegS)
                {
                    current_road.Csere(other_road, newroad);
                    other_road.addSzomszed(current_road);
                    ok = false;
                    break;
               }

               Vector3 p1 = current_road.position;
               Vector3 q1 = newroad.position;
               if (other_road.getElozo() == null) continue;
               Vector3 p2 = other_road.getElozo().position;
               Vector3 q2 = other_road.position;
               if (p1 == p2 || p1 == q2 || p2 == q1) continue;
               if (SegmentFunctions.doIntersect(p1, q1, p2, q2))
               {
                   ok = false;
                   break;
                }
            }
                
            if (ok)
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
    void SmoothRoads()
    {
        foreach (RoadNode2 road in roads)
        {
            road.Smooth(smootIntensity);
        }
        foreach (RoadNode2 road in sideroads)
        {
            road.Smooth(smootIntensity);
        }
    }
}
