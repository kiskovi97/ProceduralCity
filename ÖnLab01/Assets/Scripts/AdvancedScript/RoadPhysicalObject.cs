using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadPhysicalObject : MonoBehaviour {
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    Mesh mesh;
    List<Vector3> meshVertexes = new List<Vector3>();
    List<List<int>> subTriangles;
    List<Vector2> myUV = new List<Vector2>();
    List<Material> materials;

    // Szelso es Kozepso pontok
    Vector3 S1, S2, K1, K2;

    public void GenerateBlockMesh(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2)
    {

        subTriangles = new List<List<int>>();
        mesh = GetComponent<MeshFilter>().mesh;
        materials = new List<Material>();
        materials.AddRange(GetComponent<MeshRenderer>().materials);
        for (int i = 0; i < materials.Count; i++)
        {
            subTriangles.Add(new List<int>());
        }
        S1 = s1;
        S2 = s2;
        K1 = k1;
        K2 = k2;
        GenerateMesh();
    }
    void AddTriangle(Vector3 A, Vector3 B, Vector3 C, int mat)
    {

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(A);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(B);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(C);

    }
    void GenerateMesh()
    {
        AddTriangle(S2, S1, K1,0);
        AddTriangle(K1, K2, S2,0);
        myUV.Add(new Vector2(0, 0));
        myUV.Add(new Vector2(1, 0));
        myUV.Add(new Vector2(0, 1));
        myUV.Add(new Vector2(0, 1));
        myUV.Add(new Vector2(1, 0));
        myUV.Add(new Vector2(1, 1));
    }
    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = meshVertexes.ToArray();
        mesh.subMeshCount = subTriangles.Count;
        for (int i = 0; i < subTriangles.Count; i++)
        {
            mesh.SetTriangles(subTriangles[i].ToArray(), i);
        }
        mesh.SetUVs(0, myUV);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
