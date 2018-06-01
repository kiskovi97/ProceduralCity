using Assets.Scripts.AdvancedCity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// : MonoBehaviour 
public class MainRoadObjGenerator{
    MyMath math = new MyMath();
    List<RoadNode> roads;
    List<List<RoadNode>> circles = new List<List<RoadNode>>();
    GameObject blockObject;
    GameObject roadObject;
    
    public float MainRatio = 3;
    float roadSize;

    public void GenerateRoadMesh(List<RoadNode> inroads)
    {

        foreach (RoadNode road in inroads)
        {
            road.Rendez();
            List<RoadNode> szomszedok = road.Szomszedok;

            List<List<Vector3>> lista = new List<List<Vector3>>();
            for (int i = 0; i < szomszedok.Count; i++)
            {
                lista.Add(new List<Vector3>());
                for (int j=0; j<4; j++) lista[i].Add(new Vector3(0, 0, 0));
            }

            List<Vector3> kor = new List<Vector3>();
            List<bool> ute = new List<bool>();
            Vector3 ez = road.position;
            bool ezbool = !road.IsSideRoad();
            for (int i=0; i<szomszedok.Count; i++)
            {
                int kov = i + 1;
                if (i == szomszedok.Count - 1) kov = 0;
                Vector3 elozo = szomszedok[i].position;
                Vector3 kovetkezo = szomszedok[kov].position;
                float utelozo = 0.6f + ((!szomszedok[i].IsSideRoad() && ezbool) ? 0.5f : 0.0f);
                float utekov = 0.6f + ((!szomszedok[kov].IsSideRoad() && ezbool) ? 0.5f : 0.0f);

                Vector3 merolegeselozo = Meroleges(ez, elozo).normalized*utelozo;
                Vector3 merolegeskovetkezo = Meroleges(kovetkezo, ez).normalized*utekov;
                Vector3 tmpelozo = ez + (elozo - ez).normalized;
                Vector3 tmpkovetkezo = ez + (kovetkezo - ez).normalized;

                Vector3 P = tmpelozo + merolegeselozo;
                Vector3 V = (elozo - ez).normalized;
                Vector3 Q  = tmpkovetkezo + merolegeskovetkezo;
                Vector3 U = (kovetkezo - ez).normalized;
                Vector3 kereszt = math.Intersect(P, V, Q, U);

                Vector3 meroleges_elozo = math.Intersect(kereszt, merolegeselozo, ez, (elozo - ez).normalized);
                lista[i][2] = meroleges_elozo;
                lista[i][3] = kereszt;
                
                Vector3 meroleges_kov = math.Intersect(kereszt, merolegeskovetkezo, ez, (kovetkezo - ez).normalized);
                lista[kov][0] = meroleges_kov;
                lista[kov][1] = kereszt;
            }
            for (int i=0; i<lista.Count; i++)
            {
                Vector3 szomszed = szomszedok[i].position;
                float a = Vector3.Dot((szomszed - ez).normalized, lista[i][0] - ez);
                float b = Vector3.Dot((szomszed - ez).normalized, lista[i][2] - ez);
                if (a>b)
                {
                    ute.Add(true);
                    Vector3 masikkereszt = math.Intersect(lista[i][3], (szomszed - ez).normalized, lista[i][1], lista[i][0] - lista[i][1]);
                    kor.Add(lista[i][1]);
                    kor.Add(masikkereszt);
                    //Debug.DrawLine(lista[i][3], masikkereszt, Color.blue, 1000, false);
                    //Debug.DrawLine(lista[i][1], masikkereszt, Color.green, 1000, false);
                } else
                {
                    Vector3 masikkereszt = math.Intersect(lista[i][1], (szomszed - ez).normalized, lista[i][3], lista[i][2] - lista[i][3]);
                    ute.Add(false);
                    kor.Add(lista[i][1]);
                    kor.Add(masikkereszt);
                    //Debug.DrawLine(lista[i][1], masikkereszt, Color.blue, 1000, false);
                    //Debug.DrawLine(lista[i][3], masikkereszt, Color.green, 1000, false);
                }
            }
            for (int i=0; i<kor.Count; i++)
            {
                int kov = i + 1;
                if (kov>=kor.Count) kov = 0;
                if (i % 2 == 0 && ute[i/2])
                    Debug.DrawLine(kor[i],kor[kov], Color.blue, 1000, false);
                else
                if (i % 2 == 1 && !ute[i / 2])
                    Debug.DrawLine(kor[i], kor[kov], Color.blue, 1000, false);
                else
                    Debug.DrawLine(kor[i], kor[kov], Color.green, 1000, false);
                /*
                    ute jelzi, hogy a kor listaban az elso vagy masdik ut amelyik az ut resze 
                    a masik fele a kiegeszito resz
                */
            }
        }
    }
    // The Main And First to Call Function
    public List<List<RoadNode>> GenerateCircles(List<RoadNode> list, GameObject block, GameObject roadobj, float _roadSize)
    {
        roadSize = _roadSize;
        blockObject = block;
        roadObject = roadobj;
        roads = list;
        if (roads == null)
        {
            Debug.Log("ERROR Not initializaled Roads");
            return null;
        }
        if (list == null) return null;
       
        if (roads.Count <= 0) return null;
        foreach (RoadNode road in list)
        {
            List<RoadNode> sz = road.Szomszedok;
            foreach (RoadNode second in sz)
            {
                GenerateCircle(road, second, false);
            }
        }
        //foreach (List<RoadNode> circle in circles)
        //{
        //    MakeABlock(circle);
        //}
        return circles;
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


    public void MakeABlock(List<RoadNode> circle)
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
            if (!next_node.IsSideRoad() && !actual_node.IsSideRoad()) return A + mer * roadSize * MainRatio;
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
