using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadNode : MonoBehaviour {
    public float radius = 1.0f; // Meghatarozza, hogy az ut tengelyetol mennyire van messze az ut szele.
    public Vector3 elozo_irany = new Vector3(0, 0, 1);
    Mesh mesh;
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;

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
        uj.Set(elozo_irany.x * -1, elozo_irany.y * -1, elozo_irany.z * -1);
        tovabb_irany.Add(uj);

    }
    // Adatok az objektum letrehozasahoz
    public List<Vector3> vertexes = new List<Vector3>();
    
    public List<Vector3> normals = new List<Vector3>();
    public List<int> triangles = new List<int>();
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
            vertexes.Add(pos + irany + iranyBalra);
            vertexes.Add(pos + irany + iranyJobbra);
            
            normals.Add(normal);
            normals.Add(normal);
        }
        iranyJobbra.Set(elozo_irany.z, elozo_irany.y, elozo_irany.x * -1);
        iranyBalra.Set(elozo_irany.z * -1, elozo_irany.y, elozo_irany.x);
        vertexes.Add(pos + elozo_irany + iranyBalra);
        vertexes.Add(pos + elozo_irany + iranyJobbra);
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
    }
    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertexes.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.SetNormals(normals);
    }

}
