using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNodeContainer : MonoBehaviour {
  
    public GameObject visual;
    public List<RoadNode2> roads = new List<RoadNode2>();
    int rootObjIndex = 0;
    public float xMin = -20;
    public float zMin = -20;
    public float xMax = 20;
    public float zMax = 20;
    // Use this for initialization
    void Start() {
        roads.Clear();
        roads.Add(new RoadNode2());
        Invoke("Visualization01", 0.01f);
        Invoke("Generating",0.1f);
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
    void Generating(){
        if (rootObjIndex < roads.Count)
        {
            RoadNode2 root = roads[rootObjIndex];
            if (PalyanBelulVane(root))
            {
                List<RoadNode2> newRoads = root.GenerateRoads();
                foreach (RoadNode2 road in newRoads)
                {
                    roads.Add(road);
                    GameObject ki = Instantiate(visual);
                    ki.transform.position = road.position;
                }
            }
            rootObjIndex++;
            Generating();
        }
        else return;
    }
	
}
