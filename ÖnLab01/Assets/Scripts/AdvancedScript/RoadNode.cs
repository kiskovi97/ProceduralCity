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
	}

    List<Vector3> tovabb_irany = new List<Vector3>();
    void MakeIranyok()
    {
        
        Vector3 uj = new Vector3();
        float Rotation = 3.14f * (-120.0f / 180);
        uj.Set(
            elozo_irany.x * Mathf.Cos(Rotation) - elozo_irany.z * Mathf.Sin(Rotation), elozo_irany.y * -1,
            elozo_irany.x * Mathf.Sin(Rotation) + elozo_irany.z * Mathf.Cos(Rotation));
        uj.Normalize();
        tovabb_irany.Add(uj);

        Rotation = 3.14f * (-240.0f / 180);
        Vector3 uj2 = new Vector3();
        uj2.Set(
            elozo_irany.x * Mathf.Cos(Rotation) - elozo_irany.z * Mathf.Sin(Rotation), elozo_irany.y * -1,
            elozo_irany.x * Mathf.Sin(Rotation) + elozo_irany.z * Mathf.Cos(Rotation));
        uj2.Normalize();
        tovabb_irany.Add(uj2);

    }
    // Adatok az objektum letrehozasahoz
    List<Vector3> vertexes = new List<Vector3>();
    
    List<Vector3> normals = new List<Vector3>();
    List<int> triangles = new List<int>();
    // Update is called once per frame
    void MakeMeshData () {
        Vector3 normal = new Vector3(0, 1, 0);
        Vector3 pos = transform.position;
        vertexes.Add(pos);
        normals.Add(normal);
        Vector3 iranyJobbra = new Vector3();
        Vector3 iranyBalra = new Vector3();
        foreach (Vector3 irany in tovabb_irany)
        {
            iranyJobbra.Set(irany.z, irany.y, irany.x * -1);
            iranyBalra.Set(irany.z * -1, irany.y, irany.x);
            vertexes.Add(pos + irany*radius + iranyBalra*radius2);
            vertexes.Add(pos + irany*radius + iranyJobbra * radius2);
            
            normals.Add(normal);
            normals.Add(normal);
        }
        iranyJobbra.Set(elozo_irany.z, elozo_irany.y, elozo_irany.x * -1);
        iranyBalra.Set(elozo_irany.z * -1, elozo_irany.y, elozo_irany.x);
        vertexes.Add(pos + elozo_irany*radius + iranyBalra * radius2);
        vertexes.Add(pos + elozo_irany*radius + iranyJobbra * radius2);
        normals.Add(normal);
        normals.Add(normal);
        for (int i=1; i<vertexes.Count-1; i += 1)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }
        triangles.Add(0);
        triangles.Add(vertexes.Count - 1);
        triangles.Add(1);

        // elozovel osszekotes
        normals.Add(normal);
        normals.Add(normal);
        vertexes.Add(elozo_pont07);
        vertexes.Add(elozo_pont08);
        triangles.Add(vertexes.Count - 1);
        triangles.Add(vertexes.Count - 2);
        triangles.Add(vertexes.Count - 4);

        triangles.Add(vertexes.Count - 2);
        triangles.Add(vertexes.Count - 3);
        triangles.Add(vertexes.Count - 4);


    }
    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertexes.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.SetNormals(normals);
    }

}
