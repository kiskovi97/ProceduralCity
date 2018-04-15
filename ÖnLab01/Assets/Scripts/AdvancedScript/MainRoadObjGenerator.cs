using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// : MonoBehaviour 
public class MainRoadObjGenerator{
    List<RoadNode2> roads;
    List<List<RoadNode2>> circles = new List<List<RoadNode2>>();
    GameObject blockObject;
    public float roadSize = 0.2f;


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
        if (circle.Count <= 2) return;
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
            vertexes.Add(road.position);
        }
        Vector3 kozeppont = new Vector3(0, 0, 0);
        foreach (RoadNode2 road in circle)
        {
            kozeppont += road.position;
        }
        kozeppont /= vertexes.Count;
        /*for (int i=0; i<vertexes.Count; i++)
        {
            Vector3 irany = kozeppont - vertexes[i];
            vertexes[i] += irany.normalized*0.3f;
        }*/
        bos.GenerateBlockMesh(beljebb(vertexes), kozeppont);
        bos.CreateMesh();
    }

    List<Vector3> beljebb(List<Vector3> eredeti)
    {
        List<Vector3> uj = new List<Vector3>();
        uj.Add(Kereszt(eredeti[0], eredeti[eredeti.Count - 1], eredeti[1]));
        for (int i=1; i<eredeti.Count-1; i++)
        {
            uj.Add(Kereszt(eredeti[i], eredeti[i - 1], eredeti[i + 1]));
        }
        uj.Add(Kereszt(eredeti[eredeti.Count - 1], eredeti[eredeti.Count - 2], eredeti[0]));
        
        return uj;
    }
    Vector3 Kereszt(Vector3 actual,Vector3 elozo, Vector3 next)
    {
        float angle = Vector3.SignedAngle(elozo - actual, next - actual, new Vector3(0, 1, 0))/2.0f;
        float tan = Mathf.Tan(angle / 180.0f * 3.14f)*-1;
        Vector3 mer = Meroleges(actual, next).normalized;
        if (tan <= 0)
        {
            Vector3 elozoIrany = (elozo - actual) * -1;
            Vector3 nextIrany = (next - actual) * -1;
            angle = Vector3.SignedAngle(elozoIrany, nextIrany, new Vector3(0, 1, 0)) / 2.0f;
            tan = Mathf.Tan(angle / 180.0f * 3.14f) * -1;
            if (tan <= 0)
            {
                return actual+mer*roadSize;
            }
            float a = roadSize / tan;
            Vector3 A = actual + (nextIrany).normalized * a;
            
            return A + mer * roadSize;

        } else
        {
            float a = roadSize / tan;
            Vector3 A = actual + (next - actual).normalized * a;
            return A + mer * roadSize;
        }
        
        //return A;
    }
    Vector3 Meroleges(Vector3 actual_point, Vector3 next_point)
    {
        Vector3 next_irany = (next_point - actual_point).normalized;
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Vector3 meroleges = (rotation * next_irany).normalized;
        return meroleges;
    }
}
