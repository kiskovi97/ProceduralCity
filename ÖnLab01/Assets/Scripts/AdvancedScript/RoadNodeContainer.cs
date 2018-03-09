using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNodeContainer : MonoBehaviour {
  
    public GameObject visual;
    public List<RoadNode2> roads = new List<RoadNode2>();
    List<RoadNode2> sideroads = new List<RoadNode2>();

    int rootObjIndex = 0;
    int rootSideObjectIndex = 0;
    public float xMin = -20;
    public float zMin = -20;
    public float xMax = 20;
    public float zMax = 20;
    public float kozelseg = 0.3f;
    public float straightFreq = 0.9f;
    public int MaxElagazas = 4;
    public int ReqursiveMax = 300;
    public float RotationRandom = 0.2f;
    // Use this for initialization
    void Start() {
        roads.Clear();
        roads.Add(new RoadNode2(straightFreq, MaxElagazas, RotationRandom, 2));
        Visualization01();
        GeneratingMainRoads();
        GeneratingSideRoads();
       // GrowSideRoads();
    }
    void Visualization01()
    {
        foreach (RoadNode2 road in roads)
        {
            GameObject ki = Instantiate(visual);
            ki.transform.position = road.position;
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
        ReqursiveMax--;
        if (ReqursiveMax < 0) return;

        if (rootObjIndex < roads.Count)
        {
            RoadNode2 root = roads[rootObjIndex];
            if (PalyanBelulVane(root))
            {
                List<RoadNode2> newRoads = root.GenerateRoads();
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
                        GameObject ki = Instantiate(visual);
                        ki.transform.position = road.position;
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
	void GeneratingSideRoads()
    {
        Debug.Log("SideRoads");
        sideroads.Clear();
        foreach(RoadNode2 road in roads)
        {
            List<RoadNode2> ki = new List<RoadNode2>();
            if (Random.value <0.2)
            {
                ki = road.GenerateSideRoads();
            }
            foreach(RoadNode2 sideroad in ki)
            {
                bool oks = true;
                foreach (RoadNode2 other_road in roads)
                {
                    if ((sideroad.position - other_road.position).sqrMagnitude < kozelseg/2)
                    {
                        oks = false;
                        Debug.Log("Kozel van");
                        break;
                    }
                }
                foreach (RoadNode2 other_road in sideroads)
                {
                    if ((sideroad.position - other_road.position).sqrMagnitude < kozelseg/2)
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

    void GrowSideRoads()
    {
        ReqursiveMax--;
        if (ReqursiveMax < 0) return;

        if (rootSideObjectIndex < sideroads.Count)
        {
            RoadNode2 root = sideroads[rootSideObjectIndex];
            if (PalyanBelulVane(root))
            {
                List<RoadNode2> newRoads = root.GenerateRoads();
                foreach (RoadNode2 road in newRoads)
                {
                    bool oks = true;
                    foreach (RoadNode2 other_road in sideroads)
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
                        sideroads.Add(road);
                        GameObject ki = Instantiate(visual);
                        ki.transform.position = road.position;
                    }

                }
            }
            rootSideObjectIndex++;
            GrowSideRoads();
        }
        else
        {
            Debug.Log("Vege");
            return;
        }
    }

}
