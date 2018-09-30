using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BuildingObject : MonoBehaviour {
    public Texture2D texture;
    MeshFilter meshfilter;
    Mesh mesh;
    List<Vector3> meshVertexes;
    List<List<int>> subTriangles;
    List<Vector2> UV;
    List<Vector3> colliderMeshVertexes;
    List<List<int>> colliderSubTriangles;
    List<Vector2> colliderUV;
    // Use this for initialization
    void Start () {
        meshfilter = GetComponent<MeshFilter>();
        mesh = meshfilter.mesh;
        meshVertexes = new List<Vector3>();
        UV = new List<Vector2>();
        subTriangles = new List<List<int>>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        int MateraialCount = meshRenderer.materials.Length;
        for (int i=0; i< MateraialCount; i++)
        {
            subTriangles.Add(new List<int>());
        }
        colliderMeshVertexes = new List<Vector3>();
        colliderUV = new List<Vector2>();
        colliderSubTriangles = new List<List<int>>();
        for (int i = 0; i < MateraialCount; i++)
        {
            colliderSubTriangles.Add(new List<int>());
        }
    }

    // Update is called once per frame
    public void MakeBuilding(List<Vector3> kontrolpoints, int floorNumber, float floor)
    {
        Start();
        BuildingGenerator building = new BuildingGenerator(kontrolpoints.ToArray(), floor, floorNumber);
        float max = 0;
        foreach (Triangle triangle in building.getTriangles())
        {
            AddTriangle(triangle);
            if (triangle.A.y > max) max = triangle.A.y;
        }

        Triangle[] collision = building.GetCollision();
        foreach (Triangle triangle in collision)
        {
            AddTriangleCollision(triangle);
        }
        CreateMesh();
        CreateColliderMesh();
    }

    public void MakeBase(Vector3 a, Vector3 b, Vector3 c)
    {
        Start();
        TriangleShape triangleShape = new TriangleShape(a, b, c, (int)BlockMaterial.BASE);
        foreach (Triangle triangle in triangleShape.getTriangles())
        {
            AddTriangle(triangle);
        }
        CreateMesh();
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
        AddPoint(matrix * (triangle.A - to), triangle.material, triangle.uvs[0]);
        AddPoint(matrix * (triangle.B - to), triangle.material, triangle.uvs[1]);
        AddPoint(matrix * (triangle.C - to), triangle.material, triangle.uvs[2]);
    }
    private void AddTriangleCollision(Triangle triangle)
    {
        if (colliderSubTriangles.Count <= triangle.material)
        {
            Debug.Log("Need material : " + triangle.material);
            return;
        }
        Matrix4x4 matrix = gameObject.transform.worldToLocalMatrix;
        Vector3 to = transform.position;
        if (triangle.uvs.Length < 3) return;
        AddColliderPoint(matrix * (triangle.A - to), triangle.material, triangle.uvs[0]);
        AddColliderPoint(matrix * (triangle.B - to), triangle.material, triangle.uvs[1]);
        AddColliderPoint(matrix * (triangle.C - to), triangle.material, triangle.uvs[2]);
    }

    private void AddPoint(Vector3 kp, int mat, Vector2 uv)
    {
        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(kp);
        UV.Add(uv);
    }

    private void AddColliderPoint(Vector3 kp, int mat, Vector2 uv)
    {
        if (colliderMeshVertexes.Contains(kp))
        {
            int i = colliderMeshVertexes.IndexOf(kp);
            colliderSubTriangles[mat].Add(i);
        } else
        {
            colliderSubTriangles[mat].Add(colliderMeshVertexes.Count);
            colliderMeshVertexes.Add(kp);
            colliderUV.Add(uv);
        }
    }

    Mesh colliderMesh;
    void CreateMesh()
    {
        mesh.Clear();
        if (meshVertexes == null || meshVertexes.Count < 3)
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
        mesh.SetUVs(0, UV);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void CreateColliderMesh()
    {
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        Mesh mesh1 = new Mesh();
        colliderMesh = mesh1;
        colliderMesh.Clear();
        colliderMesh.vertices = colliderMeshVertexes.ToArray();
        colliderMesh.subMeshCount = colliderSubTriangles.Count;
        for (int i = 0; i < colliderSubTriangles.Count; i++)
        {
            colliderMesh.SetTriangles(colliderSubTriangles[i].ToArray(), i);
        }
        colliderMesh.SetUVs(0, colliderUV);
        colliderMesh.RecalculateBounds();
        colliderMesh.RecalculateNormals();
        collider.sharedMesh = colliderMesh;
        //collider.convex = true;
    }

    public void DestorySelf()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
