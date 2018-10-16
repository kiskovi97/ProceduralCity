using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadPhysicalObject : MonoBehaviour
{

    public GameObject sideRoadObject;
    List<Vector2> ControlPoints = new List<Vector2>();
    Mesh mesh;
    List<Vector3> meshVertexes = new List<Vector3>();
    List<List<int>> subTriangles;
    List<Vector2> myUV = new List<Vector2>();
    List<Material> materials;
    public void CreateRoadMesh(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2, int savok, bool tram, bool sideway, float zebra, float otherzebra)
    {
        Vector2[] tomb =
        {
            new Vector2(0,0),
            new Vector2(1.0f,0)
        };
        ControlPoints.AddRange(tomb);
        subTriangles = new List<List<int>>();
#if UNITY_EDITOR
        MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
        Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
        mesh = mf.mesh = meshCopy;
#else
     //do this in play mode
     mesh = GetComponent<MeshFilter>().mesh;
#endif
        mesh.Clear();
        for (int i = 0; i < GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
        {
            subTriangles.Add(new List<int>());
        }
        RoadGenerator roadGenerator = new RoadGenerator(s1, s2, k1, k2, savok, sideway, zebra, otherzebra);
        foreach (Triangle triangle in roadGenerator.getTriangles())
        {
            AddTriangle(triangle);
        }
    }
    public void AddRoadMesh(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2, int savok, bool tram, bool sideway, float zebra, float otherzebra)
    {
        RoadGenerator roadGenerator = new RoadGenerator(s1, s2, k1, k2, savok, sideway, zebra, otherzebra);
        foreach (Triangle triangle in roadGenerator.getTriangles())
        {
            AddTriangle(triangle);
        }
    }
    public void CreateCrossingMesh(List<Vector3> pointList)
    {
        subTriangles = new List<List<int>>();
        mesh = GetComponent<MeshFilter>().mesh;
        materials = new List<Material>();
        materials.AddRange(GetComponent<MeshRenderer>().materials);
        for (int i = 0; i < materials.Count; i++)
        {
            subTriangles.Add(new List<int>());
        }
        CrossingGenerator crossing = new CrossingGenerator(pointList.ToArray());
        foreach (Triangle triangle in crossing.getTriangles())
        {
            AddTriangle(triangle);
        }
    }

    public void AddCrossingMesh(List<Vector3> pointList)
    {
        CrossingGenerator crossing = new CrossingGenerator(pointList.ToArray());
        foreach (Triangle triangle in crossing.getTriangles())
        {
            AddTriangle(triangle);
        }
    }

    bool AddTriangle(Vector3 A, Vector3 B, Vector3 C, int mat)
    {
        if (A == B || B == C || C == A) return false;
        float area = MyMath.Area(A, B, C);
        if (area < 0.0001f) return false;
        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(A);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(B);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(C);
        return true;

    }

    private void AddTriangle(Triangle triangle)
    {
        if (subTriangles.Count <= triangle.material)
        {
            Debug.Log("Need material : " + triangle.material);
            return;
        }
        Matrix4x4 matrix = gameObject.transform.worldToLocalMatrix;
        Vector3 to = transform.position;
        if (triangle.uvs.Length < 3) return;
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.A - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.B - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.C - to));
        myUV.Add(triangle.uvs[0]);
        myUV.Add(triangle.uvs[1]);
        myUV.Add(triangle.uvs[2]);
    }
    public void CreateMesh()
    {
        if (meshVertexes == null) return;
        mesh.Clear();
        if (meshVertexes.Count < 3)
        {
            Destroy(this.gameObject, 0.1f);
            return;
        }
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
