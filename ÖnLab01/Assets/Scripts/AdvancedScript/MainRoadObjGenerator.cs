using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// : MonoBehaviour 
public class MainRoadObjGenerator{
    List<RoadNode> roads;
    List<List<RoadNode>> circles = new List<List<RoadNode>>();
    GameObject blockObject;
    GameObject roadObject;
    public float roadSize = 0.1f;
    public float MainRatio = 3;

    // The Main And First to Call Function
    public void GenerateCircles(List<RoadNode> list, GameObject block, GameObject roadobj)
    {
        blockObject = block;
        roadObject = roadobj;
        roads = list;
        if (roads == null)
        {
            Debug.Log("ERROR Not initializaled Roads");
            return;
        }
        if (list == null) return;
       
        if (roads.Count <= 0) return;
        foreach (RoadNode road in list)
        {
            List<RoadNode> sz = road.Szomszedok;
            foreach (RoadNode second in sz)
            {
                GenerateCircle(road, second, false);
            }
        }
        foreach (List<RoadNode> circle in circles)
        {
            MakeABlock(circle);
        }
    }
    // Search for a new Circle
    void GenerateCircle(RoadNode root, RoadNode second, bool jobbra)
    {
        if (second == null) return;

        List<RoadNode> circle = new List<RoadNode>();
        circle.Add(root);
        circle.Add(second);
        bool ok = true;
        int last = circle.Count - 1;
            while (ok)
            {
                RoadNode nextroad = circle[last].Kovetkezo(circle[last - 1], jobbra);
                if (nextroad == null) return;
                if (nextroad == root) ok = false;
                else
                {
                    foreach (RoadNode road in circle)
                    {
                        if (road == nextroad) return;
                    }
                    circle.Add(nextroad);
                    last++;
                }

            }
        if (circle.Count <= 2) return;
        ok = true;
        foreach(List<RoadNode> eddigi in circles)
        {
            if (CircleEqual(eddigi, circle)) ok = false;
        }
        if (ok)
        {
            circles.Add(circle);
        }
        
    }
    // 2 Circle is equal?
    bool CircleEqual(List<RoadNode> egyik, List<RoadNode> masik)
    {
        List<RoadNode> hosszu = new List<RoadNode>();
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


    void MakeABlock(List<RoadNode> circle)
    {
        GameObject ki = Object.Instantiate(blockObject);
        BlockObjectScript bos = ki.GetComponent<BlockObjectScript>();
        List<Vector3> vertexes = new List<Vector3>();
        foreach (RoadNode road in circle)
        {
            vertexes.Add(road.position);
        }
        List<Vector3> beljebbCircle = beljebb(circle);
        for (int i=0; i<circle.Count-1; i++)
        {
           GameObject road = Object.Instantiate(roadObject);
            RoadPhysicalObject rpo = road.GetComponent<RoadPhysicalObject>();
            rpo.GenerateBlockMesh(beljebbCircle[i], beljebbCircle[i + 1], circle[i].position, circle[i + 1].position);
            rpo.CreateMesh();
        }
        GameObject road2 = Object.Instantiate(roadObject);
        RoadPhysicalObject rpo2 = road2.GetComponent<RoadPhysicalObject>();
        rpo2.GenerateBlockMesh(beljebbCircle[circle.Count - 1], beljebbCircle[0], circle[circle.Count - 1].position, circle[0].position);
        rpo2.CreateMesh();

        bos.GenerateBlockMesh(beljebbCircle);
        bos.CreateMesh();
    }

    List<Vector3> beljebb(List<RoadNode> eredeti)
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
    Vector3 Kereszt(RoadNode actual_node, RoadNode elozo_node, RoadNode next_node)
    {
        Vector3 actual = actual_node.position;
        Vector3 elozo = elozo_node.position;
        Vector3 next = next_node.position;

        float angle = Vector3.SignedAngle(elozo - actual, next - actual, new Vector3(0, 1, 0)) / 2.0f;
        float tan = Mathf.Tan(angle / 180.0f * 3.14f) * -1;
        Vector3 mer = Meroleges(actual, next).normalized;
        if (tan <= 0)
        {
            Vector3 elozoIrany = (elozo - actual) * -1;
            Vector3 nextIrany = (next - actual) * -1;
            angle = Vector3.SignedAngle(elozoIrany, nextIrany, new Vector3(0, 1, 0)) / 2.0f;
            tan = Mathf.Tan(angle / 180.0f * 3.14f) * -1;
            if (tan <= 0)
            {
                if (!next_node.IsSideRoad() && !actual_node.IsSideRoad()) return actual + mer * roadSize * MainRatio;
                return actual + mer * roadSize;
            }
            float a = roadSize / tan;

            if (!elozo_node.IsSideRoad() && !actual_node.IsSideRoad()) a*= MainRatio;
            Vector3 A = actual + (nextIrany).normalized * a;
            if (!next_node.IsSideRoad() && !actual_node.IsSideRoad()) return A + mer * roadSize * MainRatio;
            return A + mer * roadSize;

        }
        else
        {
            float a = roadSize / tan;
            if (!elozo_node.IsSideRoad() && !actual_node.IsSideRoad()) a *= MainRatio;
            Vector3 A = actual + (next - actual).normalized * a;
            if (!actual_node.IsSideRoad() && !actual_node.IsSideRoad()) return A + mer * roadSize * MainRatio;
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
