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
       // Debug.DrawLine(kozeppont,actual_point,Color.red,100,false);
        //Debug.DrawLine(kozeppont, next_point, Color.red, 100, false);
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
        if (index + 1 < controlPoints.Count)
            next_point = controlPoints[index + 1];
        else
            next_point = controlPoints[0];

        Vector3 elozo_point;
        if (index - 1 >= 0)
            elozo_point = controlPoints[index - 1];
        else
            elozo_point = controlPoints[controlPoints.Count - 1];

        Vector3 irany_elozo = (elozo_point - actual_point).normalized;
        Vector3 irany_kovetkezo = (next_point - actual_point).normalized;

        Vector3 actual_meroleges;
        Vector3 egyik = irany_elozo + irany_kovetkezo;
        Vector3 masik = (irany_elozo + irany_kovetkezo)*(-1);
        Vector3 kozeppont_irany = kozeppont - actual_point;
        if (egyik.sqrMagnitude < 0.01f)
        {
            Vector3 bal = new Vector3(irany_elozo.z, 0, -irany_elozo.x);
            Vector3 jobb = new Vector3(-irany_elozo.z, 0, irany_elozo.x);
            bal.Normalize();
            jobb.Normalize();
            actual_meroleges =  Vector3.Angle(kozeppont_irany, bal) > Vector3.Angle(kozeppont_irany, jobb)? jobb.normalized : bal.normalized;
        }
        else
        {
            actual_meroleges = Vector3.Angle(kozeppont_irany, egyik) > Vector3.Angle(kozeppont_irany, masik) ? masik.normalized : egyik.normalized;
        }

        Vector3 felezo_irany = (elozo_meroleges + actual_meroleges).normalized;

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
        //
        return actual_meroleges;
    }

    Vector3 NextLepes(Vector3 actual_point, Vector3 next_point, Vector3 elozo_point, Vector3 elozo_meroleges)
    {
        elozo_meroleges.Normalize();
        float a = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        float b = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        //float a = (0.5f) * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        //float b = (0.5f) * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;

        Vector3 irany_elozo = (elozo_point - actual_point).normalized;
        Vector3 irany_kovetkezo = (next_point - actual_point).normalized;


        Vector3 actual_meroleges;
        Vector3 egyik = irany_elozo + irany_kovetkezo;
        Vector3 masik = (irany_elozo + irany_kovetkezo) * (-1);
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
            actual_meroleges = Vector3.Angle(kozeppont_irany, egyik) > Vector3.Angle(kozeppont_irany, masik) ? masik.normalized : egyik.normalized;
        }

        Vector3 felezo_irany = (elozo_meroleges + actual_meroleges).normalized;

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
