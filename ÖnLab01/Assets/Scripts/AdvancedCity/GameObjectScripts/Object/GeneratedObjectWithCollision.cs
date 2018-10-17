using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratedObjectWithCollision : GeneratedObject
{
    List<Vector3> colliderMeshVertexes;
    List<List<int>> colliderSubTriangles;
    List<Vector2> colliderUV;
    protected override void Clear()
    {
        base.Clear();
        colliderMeshVertexes = new List<Vector3>();
        colliderUV = new List<Vector2>();
        colliderSubTriangles = new List<List<int>>
        {
            new List<int>()
        };
    }
    protected void AddTriangleCollision(Triangle triangle)
    {
        if (colliderSubTriangles.Count <= triangle.material)
        {
            Debug.Log("Need material : " + triangle.material);
            return;
        }
        Matrix4x4 matrix = gameObject.transform.worldToLocalMatrix;
        Vector3 to = transform.position;
        if (triangle.uvs.Length < 3) return;
        AddColliderPoint(matrix * (triangle.A - to), triangle.uvs[0]);
        AddColliderPoint(matrix * (triangle.B - to), triangle.uvs[1]);
        AddColliderPoint(matrix * (triangle.C - to), triangle.uvs[2]);
    }

    private void AddColliderPoint(Vector3 kp, Vector2 uv)
    {
        if (colliderMeshVertexes.Contains(kp))
        {
            int i = colliderMeshVertexes.IndexOf(kp);
            colliderSubTriangles[0].Add(i);
        }
        else
        {
            colliderSubTriangles[0].Add(colliderMeshVertexes.Count);
            colliderMeshVertexes.Add(kp);
            colliderUV.Add(uv);
        }
    }
    Mesh colliderMesh;
    protected void CreateColliderMesh()
    {
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        if (collider == null) collider = gameObject.AddComponent<MeshCollider>();
        colliderMesh = new Mesh();
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
}
