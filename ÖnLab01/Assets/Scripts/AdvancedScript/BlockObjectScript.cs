using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class BlockObjectScript : MonoBehaviour {
    public float minHouse = 1.0f;
    public float HouseUpmin = 3.0f;
    public float HouseUpmax = 5.0f;
    public float HouseDeepmin = 5.0f;
    public float HouseDeepmax = 6.0f;
    private bool update = false;
    class KontrolPoint
    {
        public Vector3 nextPoint;
        public Vector3 crossPoint;
        public Vector3 elozoPoint;
        public KontrolPoint(Vector3 nextP, Vector3 crossP, Vector3 elozoP)
        {
            nextPoint = nextP;
            crossPoint = crossP;
            elozoPoint = elozoP;
        }
    }
    List<KontrolPoint> utak = new List<KontrolPoint>();
    Mesh mesh;
    List<Material> materials;
    // Use this for initialization
    void Start() {

    }
    

    public void Clear()
    {
        utak.Clear();
        materials.Clear();
        controlPoints.Clear();
        meshVertexes.Clear();
        subTriangles.Clear();
    }
    
    List<Vector3> controlPoints = new List<Vector3>();
    List<Vector3> meshVertexes = new List<Vector3>();
    //List<int> triangles = new List<int>();
   
    List<List<int>> subTriangles;
    Vector3 kozeppont;
    public void GenerateBlockMesh(List<Vector3> loading, Vector3 _kozeppont)
    {

        subTriangles = new List<List<int>>();
        mesh = GetComponent<MeshFilter>().mesh;
        materials = new List<Material>();
        materials.AddRange(GetComponent<MeshRenderer>().materials);

        for (int i = 0; i < materials.Count; i++)
        {
            subTriangles.Add(new List<int>());
        }
        controlPoints.AddRange(loading);
        kozeppont = _kozeppont;
        GenerateBlock01();
    }
    void AddTriangle(Vector3 A, Vector3 B, Vector3 C,int mat)
    {

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(A);
        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(B);
        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(C);
    }
    void MakeBox(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        float max = Random.value*(HouseUpmax-HouseUpmin) + HouseUpmin;
        
        
        int color = (int)(Random.value*(materials.Count));
        Vector3 up = new Vector3(0, max, 0);
        AddTriangle(B,A, C, color);
        AddTriangle(C, A, D, color);
        //front
        AddTriangle( A + up,A, B + up, color);
        AddTriangle(B + up,A, B, color);
        //up
        AddTriangle(D + up, A + up, B + up, color);
        AddTriangle(C + up, D + up, B + up, color);
        //right
        AddTriangle(C + up, B + up, B, color);
        AddTriangle(C + up, B, C, color);
        //left
        AddTriangle(A + up, D + up, A, color);
        AddTriangle(A, D + up, D, color);
        //back
        AddTriangle(D + up, C + up, D, color);
        AddTriangle(C + up,C,  D, color);
        

    }
    void GenerateBlock01()
    {
        KontrolPoint elozo = new KontrolPoint(controlPoints[0], controlPoints[0], controlPoints[0]);
        for (int i=1; i < controlPoints.Count; i++)
        {
            elozo = SarokPoint(elozo, i);
            utak.Add(elozo);
        }
        elozo = SarokPoint(elozo, 0);
        utak.Add(elozo);
        for (int i=0; i<utak.Count-1; i++)
        {
            MakeSideROadHouses(utak[i], utak[i+1]);
        }
        MakeSideROadHouses(utak[utak.Count-1], utak[0]);
    }

    void MakeSideROadHouses(KontrolPoint elozo, KontrolPoint kovetkezo)
    {
        Vector3 meroleges = Meroleges(elozo.nextPoint, kovetkezo.elozoPoint).normalized;
        Vector3 felezo = (elozo.nextPoint + kovetkezo.elozoPoint) / 2;
        float hosz = (kovetkezo.elozoPoint - elozo.nextPoint).magnitude;
        if (hosz<minHouse*2)
        {
            MakeBox(elozo.nextPoint, kovetkezo.elozoPoint, kovetkezo.crossPoint, elozo.crossPoint);
        } else
        {
            Vector3 irany = (kovetkezo.elozoPoint-elozo.nextPoint).normalized;
            Vector3 kovetkezoPoint = elozo.nextPoint + irany * minHouse;
            KontrolPoint kovi = new KontrolPoint(kovetkezoPoint, kovetkezoPoint + meroleges * minHouse, elozo.elozoPoint);
            MakeBox(elozo.nextPoint, kovetkezoPoint, kovetkezoPoint + meroleges * minHouse, elozo.crossPoint);
            MakeSideROadHouses(kovi, kovetkezo);
        }

        
    }

    
    KontrolPoint SarokPoint(KontrolPoint elozo, int index)
    {
        
        float a = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        float b = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;

        Vector3 actual_point = controlPoints[index];
        Vector3 next_point;
        if (index + 1 < controlPoints.Count) next_point = controlPoints[index + 1];
        else next_point = controlPoints[0];
        Vector3 next_irany = (next_point - actual_point).normalized;
        Vector3 elozo_irany = (elozo.nextPoint - actual_point).normalized;

        float szog  = Vector3.SignedAngle(next_irany, elozo_irany,new Vector3(0,1,0));
        if (szog > 0 && 120 > szog)
        {
            float hosz = (elozo.nextPoint - actual_point).magnitude;
            float newHouse = hosz / 2;
            if (minHouse < newHouse)
            {
                newHouse = minHouse;
            }
            Vector3 hazPointElozo = actual_point + (elozo.nextPoint - actual_point).normalized * newHouse;

            hosz = (next_point - actual_point).magnitude;
            newHouse = hosz / 2;
            if (minHouse < newHouse)
            {
                newHouse = minHouse;
            }
            Vector3 hazPointNext = actual_point + (next_point - actual_point).normalized * newHouse;
            Vector3 hazPointCross = hazPointElozo + (next_point - actual_point).normalized * newHouse;
            MakeBox(actual_point, hazPointNext, hazPointCross, hazPointElozo);
            KontrolPoint next = new KontrolPoint(hazPointNext, hazPointCross,hazPointElozo);
            return next;
        } else
        {
            Vector3 kereszt = Kereszt(next_point, elozo.nextPoint, actual_point)*minHouse;
            KontrolPoint next = new KontrolPoint(actual_point, actual_point + kereszt, actual_point);
            return next;
        }
        
    }
    
    Vector3 Kereszt(Vector3 next_point, Vector3 elozo_point, Vector3 actual_point)
    {
        Vector3 next_irany = (next_point - actual_point).normalized;
        Vector3 elozo_irany = (elozo_point - actual_point).normalized;
        Vector3 kereszt = (next_irany + elozo_irany).normalized;
        if ((next_irany + elozo_irany).magnitude<0.01f)
        {
            return Meroleges(actual_point, next_point);
        }
        if (!befele(actual_point + kereszt * 0.01f, kereszt)) 
        {
            kereszt *= -1;
        }
        return kereszt;
    }

    Vector3 Meroleges(Vector3 actual_point, Vector3 next_point)
    {
        Vector3 next_irany = (next_point - actual_point).normalized;
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Vector3 meroleges = (rotation * next_irany).normalized;
        return meroleges;
    }
    
    bool befele(Vector3 from, Vector3 dir)
    {
        int db = 0;
        for (int i=0; i<controlPoints.Count-1; i++)
        {
            if(Metszie(controlPoints[i], controlPoints[i + 1], from, dir)) db++;
        }
        if (Metszie(controlPoints[0], controlPoints[controlPoints.Count-1], from, dir)) db++;
        return db % 2 == 1;
    }

    bool Metszie(Vector3 A, Vector3 B, Vector3 P, Vector3 direction)
    {
        // 2Dbe konvertalas XZ sikra levetitve
        direction.Normalize();
        float ax = A.x;
        float ay = A.z;
        float bx = B.x;
        float by = B.z;
        float px = P.x;
        float py = P.z;
        float vx = direction.x;
        float vy = direction.z;
        if ((vx * (ay - by) - vy * (ax - bx)) == 0)
        {
            Debug.Log("ZeroDivide");
            return false;
        }
        float q = (vx * (py - by) - vy * (px - bx))/
                    (vx * (ay - by) - vy * (ax - bx));
        float t = 0;
        if (vx == 0)
        {
            if (vy == 0)
            {
                Debug.Log("ZeroDivide");
                return false;
            }
            else
            {
                t = (q * ay + (1 - q) * by - py) / vy;
            }
        } else
        {
            t = (q * ax + (1 - q) * bx - px) / vx;
        }

        if (t < 0) return false;
        if (q < 0) return false;
        if (q > 1) return false;
        return true;
    }
    
    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = meshVertexes.ToArray();
        mesh.subMeshCount = subTriangles.Count;
        for (int i=0; i<subTriangles.Count; i++)
        {
            mesh.SetTriangles(subTriangles[i].ToArray(), i);
        }
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        update = true;
    }
}
