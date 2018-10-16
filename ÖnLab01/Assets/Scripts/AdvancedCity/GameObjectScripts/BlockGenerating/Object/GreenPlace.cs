﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GreenPlace : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> meshVertexes;
    List<List<int>> subTriangles;
    List<Vector2> UV;
    void Clear()
    {
#if UNITY_EDITOR
        //Only do this in the editor
        MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
        Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
        mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes
#else
     //do this in play mode
     mesh = GetComponent<MeshFilter>().mesh;
#endif
        mesh.Clear();
        meshVertexes = new List<Vector3>();
        UV = new List<Vector2>();
        subTriangles = new List<List<int>>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        int MateraialCount = meshRenderer.sharedMaterials.Length;
        for (int i = 0; i < MateraialCount; i++)
        {
            subTriangles.Add(new List<int>());
        }
    }

    // Update is called once per frame
    public void MakePlace(Vector3[] kontrolpoints)
    {
        Clear();
        GreenPlaceGenerator place = new GreenPlaceGenerator(kontrolpoints);
        foreach (Triangle triangle in place.getTriangles())
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
    public void DestorySelf()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
