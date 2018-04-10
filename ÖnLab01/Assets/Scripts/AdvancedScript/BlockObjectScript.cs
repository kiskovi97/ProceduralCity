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
    
    Mesh mesh;
    List<Material> materials;
    // Use this for initialization
    void Start () {
        
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
        
        
        int color = (int)(Random.value*5);
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
        Vector3 meroleges = elsoMeroleges(0);
        for (int i=1; i < controlPoints.Count; i++)
        {
            //Vector3 merolegese = Kereszt(i);
            meroleges = NextLepes(i, meroleges);
        }
        meroleges = NextLepes(0, meroleges);
    }
    Vector3 elsoMeroleges(int index)
    {

        Vector3 actual_point = controlPoints[index];
        Vector3 next_point;
        if (index + 1 < controlPoints.Count)
            next_point = controlPoints[index + 1];
        else
            next_point = controlPoints[0];

        Vector3 elozo_point;
        if (index - 1 > 0)
            elozo_point = controlPoints[index - 1];
        else
            elozo_point = controlPoints[controlPoints.Count - 1];

        Vector3 irany_elozo = (elozo_point - actual_point).normalized;
        Vector3 irany_kovetkezo = (next_point - actual_point).normalized;
        Vector3 actual_meroleges;
        Vector3 egyik = irany_elozo + irany_kovetkezo;
        Vector3 kozeppont_irany = kozeppont - actual_point;
        if (egyik.sqrMagnitude < 0.01f)
        {
            Vector3 bal = new Vector3(irany_elozo.z, 0, -irany_elozo.x);
            Vector3 jobb = new Vector3(-irany_elozo.z, 0, irany_elozo.x);
            bal.Normalize();
            jobb.Normalize();
            actual_meroleges = Vector3.Angle(kozeppont_irany, bal) > Vector3.Angle(kozeppont_irany, jobb) ? jobb.normalized : bal.normalized;

        }
        else
        {
            Vector3 masik = (irany_elozo + irany_kovetkezo) * (-1);
            actual_meroleges = Vector3.Angle(kozeppont_irany, egyik) > Vector3.Angle(kozeppont_irany, masik) ? masik.normalized : egyik.normalized;
        }
        return actual_meroleges;
    }
    Vector3 NextLepes(int index, Vector3 elozo_meroleges)
    {
        elozo_meroleges.Normalize();
        float a = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        float b = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;

        Vector3 actual_point = controlPoints[index];
        Vector3 next_point;
        if (index + 1 < controlPoints.Count) next_point = controlPoints[index + 1];
        else next_point = controlPoints[0];

        Vector3 elozo_point;
        if (index - 1 >= 0) elozo_point = controlPoints[index - 1];
        else elozo_point = controlPoints[controlPoints.Count - 1];

        Vector3 actual_meroleges = Kereszt(next_point,elozo_point,actual_point);
        Vector3 felezo_irany = Meroleges(elozo_point,actual_point);

        float hosz = (elozo_point - actual_point).magnitude;
        float arany = (hosz - minHouse) / hosz;
        Vector3 felezo_point2;
        if (arany < 0.5f || arany > 1.0f)
        {
            arany = 0.5f;
            felezo_point2 = (actual_point * (1.0f - arany) + elozo_point * arany);
            Vector3 FB = felezo_point2 + felezo_irany.normalized * b;
            Vector3 AB = actual_point + actual_meroleges.normalized * b;
            MakeBox(felezo_point2, actual_point, AB, FB);
        }
        else
        {
            Vector3 tmp = (actual_point * (1.0f - arany) + elozo_point * arany );
            felezo_point2 = NextLepes(actual_point, next_point, tmp, felezo_irany);
        }
        Vector3 felezo_point = (actual_point * (1.0f - arany) + elozo_point * arany);

        Vector3 EA = elozo_point + elozo_meroleges.normalized * a;
        Vector3 FA = felezo_point + felezo_irany.normalized * a;
        MakeBox(elozo_point, felezo_point, FA, EA);
        return actual_meroleges;
    }

    Vector3 NextLepes(Vector3 actual_point, Vector3 next_point, Vector3 elozo_point, Vector3 elozo_meroleges)
    {

        float a = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        float b = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;

        Vector3 irany_elozo = (elozo_point - actual_point).normalized;
        Vector3 irany_kovetkezo = (next_point - actual_point).normalized;


        Vector3 actual_meroleges = Meroleges(elozo_point, actual_point);

        Vector3 felezo_irany = Meroleges(elozo_point, actual_point);

        float hosz = (elozo_point - actual_point).magnitude;
        float arany = (hosz - minHouse) / hosz;
        Vector3 felezo_point2;
        if (arany < 0.5f || arany > 1.0f)
        {
            arany = 0.5f;
            felezo_point2 = (actual_point * (1.0f - arany) + elozo_point * arany);

            Vector3 FB = felezo_point2 + felezo_irany.normalized * b;
            Vector3 AB = actual_point + actual_meroleges.normalized * b;
            MakeBox(felezo_point2, actual_point, AB ,FB);
        }
        else
        {
            Vector3 tmp = (actual_point * (1.0f - arany) + elozo_point * arany);
            felezo_point2 = NextLepes(actual_point, next_point, tmp, felezo_irany);
        }
        Vector3 felezo_point = (actual_point * (1.0f - arany) + elozo_point * arany);
        Vector3 EA = elozo_point + elozo_meroleges.normalized * a;
        Vector3 FA = felezo_point + felezo_irany.normalized * a;
        MakeBox(elozo_point, felezo_point, FA, EA);
        return felezo_point2;
    }

    
    Vector3 Kereszt(Vector3 next_point, Vector3 elozo_point, Vector3 actual_point)
    {
        Vector3 next_irany = (next_point - actual_point).normalized;
        Vector3 elozo_irany = (elozo_point - actual_point).normalized;
        Vector3 kereszt = (next_irany + elozo_irany).normalized;
        if (befele(actual_point + kereszt * 0.01f, kereszt))
            Debug.DrawLine(actual_point, actual_point + kereszt * 0.5f, Color.green, 100);
        else
        {
            Debug.DrawLine(actual_point, actual_point + kereszt * 0.5f, Color.black, 100);
            kereszt *= -1;
            Debug.DrawLine(actual_point, actual_point + kereszt * 0.5f, Color.green, 100);
        }

        Debug.DrawLine(actual_point, actual_point + next_irany * 0.2f, Color.red, 100);
        return kereszt;
    }

    Vector3 Meroleges(Vector3 actual_point, Vector3 next_point)
    {
        Vector3 next_irany = (next_point - actual_point).normalized;
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Vector3 meroleges = (rotation * next_irany).normalized;

        Debug.DrawLine(actual_point, actual_point + meroleges * 0.5f, Color.red, 100);
        Debug.DrawLine(actual_point, actual_point + next_irany * 0.2f, Color.red, 100);
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
    List<Color> colors=new List<Color>();
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
        mesh.SetColors(colors);
    }
}
