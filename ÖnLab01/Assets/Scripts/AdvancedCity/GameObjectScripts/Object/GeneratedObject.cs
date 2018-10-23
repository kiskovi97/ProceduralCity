using UnityEngine;
using System.Collections.Generic;

public class GeneratedObject : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> meshVertexes = new List<Vector3>();
    List<List<int>> subTriangles;
    List<Vector2> myUV = new List<Vector2>();
    protected virtual void Clear()
    {
        meshVertexes = new List<Vector3>();
        myUV = new List<Vector2>();
        subTriangles = new List<List<int>>();
        for (int i = 0; i < GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
        {
            subTriangles.Add(new List<int>());
        }
        GetMesh();
    }
    private void GetMesh()
    {
#if UNITY_EDITOR
        MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
        Mesh meshCopy = Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
        mesh = mf.mesh = meshCopy;
#else
        mesh = GetComponent<MeshFilter>().mesh;
#endif
    }
    protected void AddTriangle(Triangle triangle)
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
            DestorySelf();
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

    public virtual void DestorySelf()
    {
        DestroyImmediate(gameObject);
    }
}
