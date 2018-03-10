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
        Visualization01();
        Invoke("Step02", 2);
    }
    void Step02()
    {
        GeneratingStartSideRoads();
        Visualization01();
    }

    void Visualization01()
    {
        foreach (RoadNode2 road in roads)
        {
            GameObject ki = Instantiate(visual);
            ki.transform.position = road.position;
            road.DrawLines();
        }
        foreach (RoadNode2 road in sideroads)
        {
            GameObject ki = Instantiate(visual);
            ki.transform.position = road.position;
            road.DrawLines();
        }

    }
    bool  PalyanBelulVane(RoadNode2 r)
    {
        if (r.position.x < xMin || r.position.x > xMax)
        {
            Debug.Log("Kint Vagy");
            return false;
        }
        if (r.position.z < zMin || r.position.z > zMax)
        {
            Debug.Log("Kint Vagy");
            return false;
        }
        return true;
    }
    void GeneratingMainRoads(){
        Debug.Log("Generating Main Roads");
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
                            RoadNode2 elozo = road.getElozo();
                            elozo.Csere(other_road, road);
                            other_road.addSzomszed(elozo);
                            oks = false;
                            Debug.Log("Javit");
                            break;
                        }
                    }
                    if (oks)
                    {
                        roads.Add(road);
                    }

                }
            }
            rootObjIndex++;
            GeneratingMainRoads();
        }
        else
        {
            Debug.Log("Vege");
            return;
        }
    }
	void GeneratingStartSideRoads()
    {
        Debug.Log("Generating SideRoads");
        sideroads.Clear();
        foreach(RoadNode2 road in roads)
        {
            List<RoadNode2> ki = new List<RoadNode2>();
            if (Random.value < SideRoadfreq)
            {
                ki = road.GenerateSideRoads(SRoadsDistances);
            }
            foreach(RoadNode2 sideroad in ki)
            {
                bool oks = true;
                foreach (RoadNode2 other_road in roads)
                {
                    if ((sideroad.position - other_road.position).sqrMagnitude < kozelsegS)
                    {
                        oks = false;
                        Debug.Log("Kozel van");
                        break;
                    }
                }
                foreach (RoadNode2 other_road in sideroads)
                {
                    if ((sideroad.position - other_road.position).sqrMagnitude < kozelsegS)
                    {
                        oks = false;
                        Debug.Log("Kozel van");
                        break;
                    }
                }
                if (oks)
                {
                    sideroads.Add(sideroad);
                    GameObject visual01 = Instantiate(visual);
                    visual01.transform.position = sideroad.position;
                }
            }


        }
    }

    
}
