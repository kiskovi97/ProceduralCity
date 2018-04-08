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
    MeshRenderer renderer;
    MeshFilter filter;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

	}
    
    List<Vector3> vertexes = new List<Vector3>();
    List<Vector3> vertexes_masik = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<int> triangles_masik = new List<int>();

    void Add(int i)
    {
       triangles_masik.Add(vertexes_masik.Count);
       vertexes_masik.Add(vertexes[i]);
    }
    public void MakeMeshData(List<Vector3> loading, Vector3 kozeppont)
    {
        filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;
        renderer = GetComponent<MeshRenderer>();
        vertexes.AddRange(loading);
        //vertexes.Add(kozeppont);
        GenerateBlock01(kozeppont);
        //MakeSimpleBlock();
    }
    void AddTriangle(Vector3 A, Vector3 B, Vector3 C,Color color)
    {

        triangles_masik.Add(vertexes_masik.Count);
        vertexes_masik.Add(A);
        triangles_masik.Add(vertexes_masik.Count);
        vertexes_masik.Add(B);
        triangles_masik.Add(vertexes_masik.Count);
        vertexes_masik.Add(C);
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
    void MakeBox(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        float max = Random.value*(HouseUpmax-HouseUpmin) + HouseUpmin;
        Color color = Color.red;
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
    void GenerateBlock01(Vector3 kozeppont)
    {
        Vector3 meroleges = elsoMeroleges(0, kozeppont);
        for (int i=1; i < vertexes.Count; i++)
        {
            meroleges = NextLepes(i, vertexes[i-1], meroleges,kozeppont);
        }
        meroleges = NextLepes(0, vertexes[vertexes.Count - 1], meroleges, kozeppont);
    }
    Vector3 elsoMeroleges(int index, Vector3 kozeppont)
    {

        Vector3 actual_point = vertexes[index];
        Vector3 next_point;
        if (index + 1 < vertexes.Count)
            next_point = vertexes[index + 1];
        else
            next_point = vertexes[0];
        Vector3 elozo_point;
        if (index - 1 > 0)
            elozo_point = vertexes[index - 1];
        else
            elozo_point = vertexes[vertexes.Count - 1];

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
    Vector3 NextLepes(int kovetkezoIndex, Vector3 elozo_point, Vector3 elozo_meroleges, Vector3 kozeppont)
    {
        elozo_meroleges.Normalize();
        float a = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        float b = Random.value * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        //float a = (0.5f) * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        //float b = (0.5f) * (HouseDeepmax - HouseDeepmin) + HouseDeepmin;

        Vector3 actual_point = vertexes[kovetkezoIndex];
        Vector3 next_point;
        if (kovetkezoIndex + 1 < vertexes.Count)
            next_point = vertexes[kovetkezoIndex + 1];
        else
            next_point = vertexes[0];

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
        }
        else
        {
            Vector3 tmp = (actual_point * (1.0f - arany) + elozo_point * arany );
            felezo_point2 = NextLepes(actual_point, next_point, tmp, felezo_irany, kozeppont);
        }
        Vector3 felezo_point = (actual_point * (1.0f - arany) + elozo_point * arany);

        Vector3 EA = elozo_point + elozo_meroleges.normalized * a;
        Vector3 FA = felezo_point + felezo_irany.normalized * a;
        Vector3 FB = felezo_point2 + felezo_irany.normalized * b;
        Vector3 AB = actual_point + actual_meroleges.normalized * b;
        MakeBox(elozo_point, felezo_point, FA, EA);
        //MakeBox(felezo_point2, actual_point, AB ,FB);
        return actual_meroleges;
    }

    Vector3 NextLepes(Vector3 actual_point, Vector3 next_point, Vector3 elozo_point, Vector3 elozo_meroleges, Vector3 kozeppont)
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
            felezo_point2 = NextLepes(actual_point, next_point, tmp, felezo_irany, kozeppont);
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
        mesh.vertices = vertexes_masik.ToArray();
        mesh.triangles = triangles_masik.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.SetColors(colors);
    }
}
