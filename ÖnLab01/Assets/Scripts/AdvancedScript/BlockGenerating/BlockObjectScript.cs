using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class BlockObjectScript : MonoBehaviour {
    
    public RoadGeneratingValues values;
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
    bool ok = true;
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
        myUV.Clear();
    }
    
    List<Vector3> controlPoints = new List<Vector3>();
    List<Vector3> meshVertexes = new List<Vector3>();
    //List<int> triangles = new List<int>();
   
    List<List<int>> subTriangles;
    List<Vector2> myUV = new List<Vector2>();
    bool ElegNagy(List<Vector3> circle)
    {
        float area = 0.0f;
        for (int i=0; i< circle.Count ; i++)
        {
            int j =( i + 1 )% circle.Count;
            area += circle[i].x * circle[j].z ;
            area -= circle[i].z * circle[j].x;
        }
        return Mathf.Abs(area) > values.sizeRatio ;
    }
    public void GenerateBlockMesh(List<Vector3> loading)
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
        if (ElegNagy(loading)) GenerateBlock01();
        GenerateNothing(loading);
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
    void AddRectangle(Vector3 A, Vector3 B, Vector3 C, Vector3 D, int color)
    {
        AddTriangle(B, A, C, color);
        AddTriangle(C, A, D, color);
        myUV.Add(new Vector2(0, 0));
        myUV.Add(new Vector2(1, 0));
        myUV.Add(new Vector2(0, 1));
        myUV.Add(new Vector2(0, 1));
        myUV.Add(new Vector2(1, 0));
        myUV.Add(new Vector2(1, 1));
    }
    void MakeBox(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        float max = Random.value*(values.HouseUpmax - values.HouseUpmin) + values.HouseUpmin;
        
        
        int color = (int)(Random.value*(materials.Count));
        Vector3 up = new Vector3(0, max, 0);
        //down
        AddRectangle(A,B,C,D, color);
        //front
        AddRectangle(A, A + up, B+up,B,color);
        //up
        AddRectangle(A + up, D +up,  C+up, B + up, color);
        //right
        AddRectangle(B + up, C +up,C, B, color);
        //left
        AddRectangle(D + up, A +up,A,D,color);
        //back
        AddRectangle(C + up, D +up,D,C,color);


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
        if (utak.Count >3)
        {
            for (int i = 0; i < utak.Count - 1; i++)
            {
                MakeSideROadHouses(utak[i], utak[i + 1]);
            }
            MakeSideROadHouses(utak[utak.Count - 1], utak[0]);
        }
        
    }

    void GenerateNothing(List<Vector3> circle)
    {
        Vector3 kozeppont = new Vector3(0, 0, 0);
        for(int i=0; i< circle.Count; i++)
        {
            kozeppont = kozeppont + circle[i];
        }
        kozeppont = kozeppont * (1.0f / circle.Count);
        for (int i = 0; i < circle.Count ; i++)
        {
            int j = (i + 1) % circle.Count;
            AddTriangle(circle[i], circle[j], kozeppont, 0);
            myUV.Add(new Vector2(0, 0));
            myUV.Add(new Vector2(1, 0));
            myUV.Add(new Vector2(0, 1));
        }
        
    }

    void MakeSideROadHouses(KontrolPoint elozo, KontrolPoint kovetkezo)
    {

        float a = Random.value * (values.HouseDeepmax - values.HouseDeepmin) + values.HouseDeepmin;
        a = values.minHouse;
        Vector3 meroleges = Meroleges(elozo.nextPoint, kovetkezo.elozoPoint).normalized;
        Vector3 felezo = (elozo.nextPoint + kovetkezo.elozoPoint) / 2;
        float hosz = (kovetkezo.elozoPoint - elozo.nextPoint).magnitude;
        if (hosz< values.minHouse *2)
        {
            Vector3 crossElozo = elozo.nextPoint + (elozo.crossPoint - elozo.nextPoint).normalized * a;
            Vector3 crossKov = kovetkezo.elozoPoint + (kovetkezo.crossPoint - kovetkezo.elozoPoint).normalized * a;
            MakeBox(elozo.nextPoint, kovetkezo.elozoPoint, crossKov, crossElozo);
        } else
        {
            Vector3 irany = (kovetkezo.elozoPoint-elozo.nextPoint).normalized;
            Vector3 kovetkezoPoint = elozo.nextPoint + irany * values.minHouse;
            KontrolPoint kovi = new KontrolPoint(kovetkezoPoint, kovetkezoPoint + meroleges * values.minHouse, elozo.elozoPoint);

            Vector3 crossElozo = elozo.nextPoint + (elozo.crossPoint - elozo.nextPoint).normalized * a;
            Vector3 crossKov = kovetkezoPoint + ((kovetkezoPoint + meroleges * values.minHouse) - kovetkezoPoint).normalized * a;

            MakeBox(elozo.nextPoint, kovetkezoPoint, crossKov, crossElozo);
            MakeSideROadHouses(kovi, kovetkezo);
        }

        
    }

    
    KontrolPoint SarokPoint(KontrolPoint elozo, int index)
    {
        

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
            if (values.minHouse < newHouse)
            {
                newHouse = values.minHouse;
            }
            Vector3 hazPointElozo = actual_point + (elozo.nextPoint - actual_point).normalized * newHouse;

            hosz = (next_point - actual_point).magnitude;
            newHouse = hosz / 2;
            if (values.minHouse < newHouse)
            {
                newHouse = values.minHouse;
            }
            Vector3 hazPointNext = actual_point + (next_point - actual_point).normalized * newHouse;
            Vector3 hazPointCross = hazPointElozo + (next_point - actual_point).normalized * newHouse;
            MakeBox(actual_point, hazPointNext, hazPointCross, hazPointElozo);
            KontrolPoint next = new KontrolPoint(hazPointNext, hazPointCross,hazPointElozo);
            return next;
        } else
        {
            Vector3 kereszt = Kereszt(next_point, elozo.nextPoint, actual_point)* values.minHouse;
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
        if (!ok) return;
        mesh.Clear();
        mesh.vertices = meshVertexes.ToArray();
        mesh.subMeshCount = subTriangles.Count;
        for (int i=0; i<subTriangles.Count; i++)
        {
            mesh.SetTriangles(subTriangles[i].ToArray(), i);
        }
        mesh.SetUVs(0, myUV);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        update = true;
    }
}
