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
        mesh = GetComponent<MeshFilter>().mesh;
        vertexes.AddRange(loading);
        //vertexes.Add(kozeppont);
        GenerateBlock01(kozeppont);
        //MakeSimpleBlock();
    }
    void AddTriangle(Vector3 A, Vector3 B, Vector3 C)
    {

        triangles_masik.Add(vertexes_masik.Count);
        vertexes_masik.Add(A);
        triangles_masik.Add(vertexes_masik.Count);
        vertexes_masik.Add(B);
        triangles_masik.Add(vertexes_masik.Count);
        vertexes_masik.Add(C);
    }
    void MakeBox(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        float max = Random.value*(HouseUpmax-HouseUpmin) + HouseUpmin;
        Vector3 up = new Vector3(0, max, 0);
        AddTriangle(B,A, C);
        AddTriangle(C, A, D);
        //front
        AddTriangle( A + up,A, B + up);
        AddTriangle(B + up,A, B);
        //up
        AddTriangle(D + up, A + up, B + up);
        AddTriangle(C + up, D + up, B + up);
        //right
        AddTriangle(C + up, B + up, B);
        AddTriangle(C + up, B, C);
        //left
        AddTriangle(A + up, D + up, A);
        AddTriangle(A, D + up, D);
        //back
        AddTriangle(D + up, C + up, D);
        AddTriangle(C + up,C,  D);
        

    }
    void GenerateBlock01(Vector3 kozeppont)
    {
        //MakeBox(vertexes[0],vertexes[1],vertexes[2],vertexes[3]);
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
        float a = Random.value* (HouseDeepmax-HouseDeepmin) + HouseDeepmin;
        float b = Random.value* (HouseDeepmax - HouseDeepmin) + HouseDeepmin;
        
        Vector3 actual_point = vertexes[kovetkezoIndex];
        Vector3 next_point;
        if (kovetkezoIndex + 1 < vertexes.Count)
            next_point = vertexes[kovetkezoIndex + 1];
        else
            next_point = vertexes[0];

        Vector3 irany_elozo = (elozo_point - actual_point).normalized;
        Vector3 irany_kovetkezo = (next_point - actual_point).normalized;
        Vector3 felezo_point = (actual_point + elozo_point) * 0.5f;
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

        Vector3 EA = elozo_point + elozo_meroleges.normalized * a;
        Vector3 FA = felezo_point + felezo_irany.normalized * a;
        Vector3 FB = felezo_point + felezo_irany.normalized * b;
        Vector3 AB = actual_point + actual_meroleges.normalized * b;
        //Debug.DrawLine(actual_point, AB, Color.red, 100, false);
        //Debug.DrawLine(felezo_point, FB, Color.red, 100, false);
        MakeBox(elozo_point, felezo_point, FA, EA);
        MakeBox(felezo_point, actual_point, AB ,FB);
        return actual_meroleges;

    }
    

    void MakeSimpleBlock()
    {
        for (int i = 0; i < vertexes.Count - 2; i++)
        {
            triangles.Add(i + 1);
            triangles.Add(i);
            triangles.Add(vertexes.Count - 1);

            Add(i + 1);
            Add(i);
            Add(vertexes.Count - 1);
        }
        triangles.Add(0);
        triangles.Add(vertexes.Count - 2);
        triangles.Add(vertexes.Count - 1);

        Add(0);
        Add(vertexes.Count - 2);
        Add(vertexes.Count - 1);

        int elso = vertexes.Count;
        float magassag = Random.value;
        for (int i = 0; i < elso; i++)
        {
            vertexes.Add(vertexes[i] + new Vector3(0, 1, 0) * magassag);
        }
        for (int i = 0; i < elso - 2; i++)
        {
            triangles.Add(i + elso);
            triangles.Add(i);
            triangles.Add(i + 1);


            triangles.Add(i + elso);
            triangles.Add(i + 1);
            triangles.Add(i + elso + 1);

            triangles.Add(i + elso);
            triangles.Add(i + elso + 1);
            triangles.Add(elso + elso - 1);

            // ----
            Add(i + elso);
            Add(i);
            Add(i + 1);


            Add(i + elso);
            Add(i + 1);
            Add(i + elso + 1);

            Add(i + elso);
            Add(i + elso + 1);
            Add(elso + elso - 1);
        }

        triangles.Add(elso + elso - 2);
        triangles.Add(elso - 2);
        triangles.Add(0);

        triangles.Add(elso + elso - 2);
        triangles.Add(0);
        triangles.Add(elso);

        triangles.Add(vertexes.Count - 2);
        triangles.Add(elso);
        triangles.Add(vertexes.Count - 1);

        //-------
        Add(elso + elso - 2);
        Add(elso - 2);
        Add(0);

        Add(elso + elso - 2);
        Add(0);
        Add(elso);

        Add(vertexes.Count - 2);
        Add(elso);
        Add(vertexes.Count - 1);
    }
    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertexes_masik.ToArray();
        mesh.triangles = triangles_masik.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
