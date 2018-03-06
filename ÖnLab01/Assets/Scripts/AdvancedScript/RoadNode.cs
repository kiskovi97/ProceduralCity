using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadNode : MonoBehaviour {
    public float radius = 1.0f; // Meghatarozza, hogy az ut tengelyetol mennyire van messze az ut szele.
    public float radius2 = 0.5f;
    public Vector3 elozo_irany = new Vector3(0, 0, 1);
    public Vector3 elozo_pont07 = new Vector3(1, 0, 10);
    public Vector3 elozo_pont08 = new Vector3(-1, 0, 10);
    public float grow = 1.0f;
    Mesh mesh;
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        elozo_irany.Normalize();
    }
    
	// Use this for initialization
	void Start () {
        MakeIranyok();
        MakeMeshData();
        CreateMesh();
        if (grow>0)
            CreateChilds();
	}

    List<Vector3> tovabb_irany = new List<Vector3>();
    void MakeIranyok()
    {
        int elagazasok = (int)(Random.value *8);
        if (elagazasok < 5) elagazasok = 1;
        else elagazasok -= 4;
        Debug.Log(elagazasok);
        for (int i=1; i<elagazasok+1; i++)
        {
            Vector3 uj = new Vector3();
            float Rotation = 3.14f * ( 2 * (-i/(elagazasok+1.0f)));
            Rotation += -0.2f + Random.value * 0.4f;
            uj.Set(
            elozo_irany.x * Mathf.Cos(Rotation) - elozo_irany.z * Mathf.Sin(Rotation), elozo_irany.y * -1,
            elozo_irany.x * Mathf.Sin(Rotation) + elozo_irany.z * Mathf.Cos(Rotation));
            tovabb_irany.Add(uj.normalized);
        }

        
    }
    // Adatok az objektum letrehozasahoz
    List<Vector3> vertexes = new List<Vector3>();
    
    List<int> triangles = new List<int>();
    // Update is called once per frame
    void MakeMeshData () {
        MakeCenter();
        MakeElozoUt();
    }
    void MakeElozoUt()
    {
        vertexes.Add(elozo_pont07);
        vertexes.Add(elozo_pont08);
        triangles.Add(vertexes.Count - 1);
        triangles.Add(vertexes.Count - 2);
        triangles.Add(vertexes.Count - 4);

        triangles.Add(vertexes.Count - 2);
        triangles.Add(vertexes.Count - 3);
        triangles.Add(vertexes.Count - 4);
    }
    void MakeCenter()
    {
        
        vertexes.Add(new Vector3(0,0,0));
        Vector3 iranyJobbra = new Vector3();
        Vector3 iranyBalra = new Vector3();
        foreach (Vector3 irany in tovabb_irany)
        {
            iranyJobbra.Set(irany.z, irany.y, irany.x * -1);
            iranyBalra.Set(irany.z * -1, irany.y, irany.x);
            vertexes.Add(irany * radius + iranyBalra * radius2);
            vertexes.Add(irany * radius + iranyJobbra * radius2);

        }
        iranyJobbra.Set(elozo_irany.z, elozo_irany.y, elozo_irany.x * -1);
        iranyBalra.Set(elozo_irany.z * -1, elozo_irany.y, elozo_irany.x);
        vertexes.Add(elozo_irany * radius + iranyBalra * radius2);
        vertexes.Add(elozo_irany * radius + iranyJobbra * radius2);
        for (int i = 1; i < vertexes.Count - 1; i += 1)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }
        triangles.Add(0);
        triangles.Add(vertexes.Count - 1);
        triangles.Add(1);
    }
    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertexes.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
    void CreateChilds()
    {

        foreach (Vector3 irany in tovabb_irany)
        {
            GameObject uj = Instantiate(this.gameObject);
            uj.transform.position += irany * 30;
            RoadNode n = uj.GetComponent<RoadNode>();
            n.grow = grow - 0.1f;
            Vector3 iranyJobbra = new Vector3();
            Vector3 iranyBalra = new Vector3();
            iranyJobbra.Set(irany.z, irany.y, irany.x * -1);
            iranyBalra.Set(irany.z * -1, irany.y, irany.x);

            n.elozo_irany = (transform.position - uj.transform.position).normalized;
            n.elozo_pont07 = (transform.position + irany.normalized * radius + iranyBalra * radius2) - uj.transform.position;
            n.elozo_pont08 = (transform.position + irany.normalized * radius + iranyJobbra * radius2) - uj.transform.position;
        }

        
    }

}
