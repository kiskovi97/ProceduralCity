using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// : MonoBehaviour 
public class MainRoadObjGenerator{
    List<RoadNode2> roads;
 //   // Use this for initialization
 //   void Start () {
 //       roads = new List<RoadNode2>();
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

   public  void GenerateCircles(List<RoadNode2> list)
    {
        //roads = new List<RoadNode2>();
        //roads.AddRange(list);
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
                GenerateCircle(road, second, true);
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
            Vector3 kozeppont = new Vector3(0, 0, 0);
            foreach (RoadNode2 road in circle)
            {
                kozeppont += road.position;
            }
            kozeppont /= circle.Count;
            foreach (RoadNode2 road in circle)
            {
                Debug.DrawLine(kozeppont, road.position, Color.green, 100, false);
            }
        
    }
}
