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
    public void MakeBuilding(List<Vector3> kontrolpoints, int min, int max, float floor, RoadGeneratingValues values)
    {
        Start();
        float positionValue = (values.getTextureValue(kontrolpoints[0])* 2);
        int floorNumber = (int)((Random.value*0.75 + 0.25) * (max - min) * positionValue) + min;
        Building building = new Building(kontrolpoints, floor, floorNumber);
        foreach (Triangle triangle in building.getTriangles())
        {
            AddTriangle(triangle);
        }
        CollisionBox box = new CollisionBox(kontrolpoints);
        foreach (Triangle triangle in box.getTriangles())
        {
            AddTriangleCollision(triangle);
        }
        CreateMesh();
        CreateColliderMesh();
    }

    public void MakeBase(Vector3 a, Vector3 b, Vector3 c)
    {
        Start();
        TriangleShape triangleShape = new TriangleShape(a, b, c, (int)MaterialEnum.BASE);
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
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.A - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.B - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.C - to));
        UV.Add(triangle.uvs[0]);
        UV.Add(triangle.uvs[1]);
        UV.Add(triangle.uvs[2]);
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
        colliderSubTriangles[triangle.material].Add(colliderMeshVertexes.Count);
        colliderMeshVertexes.Add(matrix * (triangle.A - to));
        colliderSubTriangles[triangle.material].Add(colliderMeshVertexes.Count);
        colliderMeshVertexes.Add(matrix * (triangle.B - to));
        colliderSubTriangles[triangle.material].Add(colliderMeshVertexes.Count);
        colliderMeshVertexes.Add(matrix * (triangle.C - to));
        colliderUV.Add(triangle.uvs[0]);
        colliderUV.Add(triangle.uvs[1]);
        colliderUV.Add(triangle.uvs[2]);
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
        collider.convex = true;
    }

    public void DestorySelf()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
