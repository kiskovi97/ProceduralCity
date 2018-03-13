using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// : MonoBehaviour 
public class MainRoadObjGenerator{
    List<RoadNode2> roads;
    List<List<RoadNode2>> circles = new List<List<RoadNode2>>();
    GameObject blockObject;
 //   // Use this for initialization
 //   void Start () {
 //       roads = new List<RoadNode2>();
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

   public  void GenerateCircles(List<RoadNode2> list, GameObject block)
    {
        blockObject = block;
        roads = list;
        if (roads == null)
        {
            Debug.Log("ERROR Not initializaled Roads");
            return;
        }
        if (list == null) return;
       
        if (roads.Count <= 0) return;
        foreach (RoadNode2 road in list)
        {
            
            List<RoadNode2> sz = road.getSomszedok();
            foreach (RoadNode2 second in sz)
            {
                //GenerateCircle(road, second, true);
                GenerateCircle(road, second, false);
            }
        }
    }
    void GenerateCircle(RoadNode2 root, RoadNode2 second, bool jobbra)
    {
            if (second == null) return;

            List<RoadNode2> circle = new List<RoadNode2>();
            circle.Add(root);
            circle.Add(second);
            bool ok = true;
            int last = circle.Count - 1;
            while (ok)
            {
                //Debug.Log("Circle");
                RoadNode2 nextroad = circle[last].Kovetkezo(circle[last - 1], jobbra);
                if (nextroad == null) return;
                if (nextroad == root) ok = false;
                else
                {
                    foreach (RoadNode2 road in circle)
                    {
                        if (road == nextroad) return;
                    }
                    circle.Add(nextroad);
                    last++;
                }

            }
        ok = true;
        foreach(List<RoadNode2> eddigi in circles)
        {
            if (CircleEqual(eddigi, circle)) ok = false;
        }
        if (ok)
        {
            MakeMesh(circle);
            circles.Add(circle);
        }
            
        
    }

    bool CircleEqual(List<RoadNode2> egyik, List<RoadNode2> masik)
    {
        List<RoadNode2> hosszu = new List<RoadNode2>();
        hosszu.AddRange(masik);
        hosszu.AddRange(masik.GetRange(0, masik.Count - 1));
        int j = 0;
        bool van = false;
        for (int i=0; i< hosszu.Count; i++)
        {
            if (hosszu[i] == egyik[j])
            {
                j++;
                if (j == egyik.Count) return true;
                van = true;
            }
            if (hosszu[i] != egyik[j] && !van) continue;
        }
        if (j == egyik.Count) return true;
        return false;
    }
    void MakeMesh(List<RoadNode2> circle)
    {
        GameObject ki = GameObject.Instantiate(blockObject);
        BlockObjectScript bos = ki.GetComponent<BlockObjectScript>();
        List<Vector3> vertexes = new List<Vector3>();
        foreach (RoadNode2 road in circle)
        {
            vertexes.Add(road.position );
        }
        Vector3 kozeppont = new Vector3(0, 0, 0);
        foreach (RoadNode2 road in circle)
        {
            kozeppont += road.position;
        }
        kozeppont /= vertexes.Count;
        for (int i=0; i<vertexes.Count; i++)
        {
            Vector3 irany = kozeppont - vertexes[i];
            vertexes[i] += irany.normalized*0.3f;
        }
        bos.MakeMeshData(vertexes, kozeppont);
        bos.CreateMesh();
    }
}
